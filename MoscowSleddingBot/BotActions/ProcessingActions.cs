using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using MoscowSleddingBot.Additional;
using DataLibrary;

namespace MoscowSleddingBot.Actions;

public static partial class BotActions
{
    /// <summary>
    /// A method for displaying unique filtering fields
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="callbackQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> FilterFieldValuesAction(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery.Message == null || callbackQuery.Data == null) { return await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken, "<b>Sorry</b>, I have nothing tell you about this."); }

        string userName = callbackQuery.From.Username!.ToString();
        string chatId = callbackQuery.Message.Chat.Id.ToString();
        string pathToLoadData = DirectoryHelper.GetDirectoryFromEnvironment("PathToLoadedData", $"{chatId}_{userName}.json");
        string tmpPathToSaveChoise = DirectoryHelper.GetDirectoryFromEnvironment("PathToLoadedData", $"{chatId}_{userName}_TmpChoise.txt");

        string callbackData = callbackQuery.Data;
        ReplyKeyboardMarkup replyUniqueFieldsKeyboard;


        if (!System.IO.File.Exists(pathToLoadData))
        {
            return await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken, "Seems like you didn't upload the file.\nI have nothing to work with <s>&#128577</s>");
        }
        try
        {
            await using FileStream fs = System.IO.File.OpenRead(pathToLoadData!);
            JSONProcessing jsonToDataConv = new JSONProcessing();
            List<IceHillData> lstWithData = jsonToDataConv.Read(fs, out bool isConvCorrect);
            fs.Close();
            if (!isConvCorrect) { throw new Exception("Failed to convert .json to data collection."); }

            replyUniqueFieldsKeyboard = FilterHelper.CreateValuesMarkup(lstWithData, callbackData, out bool isNoData);

            System.IO.File.WriteAllText(tmpPathToSaveChoise, callbackData);

            if (isNoData)
            {
                if (System.IO.File.Exists(tmpPathToSaveChoise)) { System.IO.File.Delete(tmpPathToSaveChoise); }
                return await telegramBotClient.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: "This field dont have any data to filter, select another field.",
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}\nException occured while trying to handle {callbackQuery.Data}.");
            return await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken,
            $"<s>&#10071</s><b>Failed</b> to get values from field <b>{callbackData}</b>. Try again later");
        }

        await telegramBotClient.DeleteMessageAsync(
            chatId: callbackQuery.Message.Chat.Id,
            messageId: callbackQuery.Message.MessageId,
            cancellationToken: cancellationToken
        );

        return await telegramBotClient.SendTextMessageAsync(
            chatId: callbackQuery.Message.Chat.Id,
            text: $"<b>Select</b> the value from the keyboard by which the data on the <b>{callbackData}</b> field will be filtered",
            parseMode: ParseMode.Html,
            replyMarkup: replyUniqueFieldsKeyboard,
            cancellationToken: cancellationToken
        );
    }

    /// <summary>
    /// A method for filtering data
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> FilterDataAction(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
    {
        if (message.Text == null) { return await VariableErrorMessageAction(telegramBotClient, message, cancellationToken, "Choose the value from keyboard."); }

        string userName = message.From!.Username!.ToString();
        string chatId = message.Chat.Id.ToString();
        string pathToLoadData = DirectoryHelper.GetDirectoryFromEnvironment("PathToLoadedData", $"{chatId}_{userName}.json");
        string tmpPathToSaveChoise = DirectoryHelper.GetDirectoryFromEnvironment("PathToLoadedData", $"{chatId}_{userName}_TmpChoise.txt");

        if (!System.IO.File.Exists(pathToLoadData) || !System.IO.File.Exists(tmpPathToSaveChoise))
        {
            return await VariableErrorMessageAction(telegramBotClient, message, cancellationToken, "Seems like you didn't upload the file.\nI have nothing to work with <s>&#128577</s>");
        }

        string? fieldToFilter = null;
        try
        {
            fieldToFilter = System.IO.File.ReadAllText(tmpPathToSaveChoise);

            await using FileStream fs = System.IO.File.OpenRead(pathToLoadData!);
            JSONProcessing jsonToDataConv = new JSONProcessing();
            List<IceHillData> lstWithData = jsonToDataConv.Read(fs, out bool isConvCorrect);
            fs.Close();
            if (!isConvCorrect) { throw new Exception("Failed to convert .json to data collection."); }

            List<IceHillData> lstFilteredData = FilterHelper.FilterDataByFieldAndParam(lstWithData, fieldToFilter, message.Text);

            JSONProcessing jsonDataRewriter = new JSONProcessing(pathToLoadData);
            Stream streamToClose = jsonDataRewriter.Write(lstFilteredData, out bool isWCS);
            streamToClose.Close();
            if (!isWCS) { throw new Exception("Failed to rewrite filtered data to the local dir."); }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}\nException occured while trying to handle {message.Text}.");

            await telegramBotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"<s>&#10071</s><b>Failed</b> to filter data by field <b>{fieldToFilter ?? "None"}</b>. Try again later",
            parseMode: ParseMode.Html,
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken
            );

            return await FileMenuAction(telegramBotClient, message, cancellationToken);
        }
        finally
        {
            if (System.IO.File.Exists(tmpPathToSaveChoise)) { System.IO.File.Delete(tmpPathToSaveChoise); }
        }

        await telegramBotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: $"<s>&#9989</s> Data was <b>successfully</b> filtered.\n<b>Field</b> : {fieldToFilter ?? "None"}\n<b>Filter value</b> : {message.Text}",
            parseMode: ParseMode.Html,
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken
        );

        return await FileMenuAction(telegramBotClient, message, cancellationToken);
    }


    /// <summary>
    /// A method for sorting data
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="callbackQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> SortDataAction(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery.Message == null || callbackQuery.Data == null) { return await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken, "<b>Sorry</b>, I have nothing tell you about this."); }

        string userName = callbackQuery.From.Username!.ToString();
        string chatId = callbackQuery.Message.Chat.Id.ToString();
        string pathToLoadData = DirectoryHelper.GetDirectoryFromEnvironment("PathToLoadedData", $"{chatId}_{userName}.json");

        await telegramBotClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: $"{Char.ConvertFromUtf32(int.Parse("1F503", System.Globalization.NumberStyles.HexNumber))} Sorting data.",
            cancellationToken: cancellationToken
            );

        string callbackData = callbackQuery.Data;
        string trimmedCallbackData = callbackData[..^2];
        bool needToReverse = !(callbackData.Split('.')[^1] == "A");
        string sortingWay = callbackData.Split('.')[^1] == "A" ? "Ascending" : "Descending";

        if (!System.IO.File.Exists(pathToLoadData))
        {
            return await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken, "Seems like you didn't upload the file.\nI have nothing to work with <s>&#128577</s>");
        }
        try
        {
            await using FileStream fs = System.IO.File.OpenRead(pathToLoadData!);
            JSONProcessing jsonToDataConv = new JSONProcessing();
            List<IceHillData> lstWithData = jsonToDataConv.Read(fs, out bool isConvCorrect);
            fs.Close();
            if (!isConvCorrect) {throw new Exception("Failed to convert .json to data collection.");}

            List<IceHillData> lstSortedData = SortHelper.SortDataByField(lstWithData, trimmedCallbackData, needToReverse);

            JSONProcessing jsonDataRewriter = new JSONProcessing(pathToLoadData);
            Stream streamToClose = jsonDataRewriter.Write(lstSortedData, out bool isWCS);
            streamToClose.Close();
            if (!isWCS) {throw new Exception("Failed to rewrite sorted data to the local dir.");}
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}\nException occured while trying to handle {callbackQuery.Data}.");
            return await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken,
            $"<s>&#10071</s><b>Failed</b> to sort data by field <b>{trimmedCallbackData}</b>. Try again later");
        }

        await telegramBotClient.EditMessageTextAsync(
            chatId: callbackQuery.Message.Chat.Id,
            messageId: callbackQuery.Message.MessageId,
            text: $"<s>&#9989</s> Data was <b>successfully</b> sorted.\n<b>Field</b> : {trimmedCallbackData}\n<b>Sorting direction</b> : {sortingWay}",
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );

        return await FileMenuAction(telegramBotClient, callbackQuery.Message, cancellationToken);
    }
}