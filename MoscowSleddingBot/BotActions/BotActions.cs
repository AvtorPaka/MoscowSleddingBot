using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using MoscowSleddingBot.Additional;
using DataLibrary;
using System.Text;

namespace MoscowSleddingBot.Actions;

public static class BotActions
{   

    /// <summary>
    /// The method for processing the initial request to the bot
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> SendStartText(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
    {
        InlineKeyboardMarkup getNewFileInlineKeyBoard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>{
                new InlineKeyboardButton[1] {InlineKeyboardButton.WithCallbackData(text: $"{Char.ConvertFromUtf32(int.Parse("1F4E9", System.Globalization.NumberStyles.HexNumber))} Don't have one.", callbackData:"/originalFile")}
            });

        return await telegramBotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "<b>Hi</b>, send me .json or .csv file with data about ice hills.",
            parseMode: ParseMode.Html,
            replyMarkup: getNewFileInlineKeyBoard,
            cancellationToken: cancellationToken
        );
    }

    /// <summary>
    /// A method for processing an unidentified message
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> SendUnknowMessageTextActionAsync(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
    {
        return await telegramBotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "<b>Sorry</b>, I have nothing to tell you about this.",
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }

    /// <summary>
    /// A method for processing and sending error information
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public static async Task<Message> VariableErrorMessageAction(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken,
     string errorMessage = $"<s>&#10071</s> <b>Error</b> occured. <s>&#10071</s>")
    {
        return await telegramBotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: errorMessage,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }

    /// <summary>
    /// A method for processing and sending error information
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="callbackQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public static async Task<Message> VariableErrorCallbackAction(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken,
    string errorMessage = $"<s>&#10071</s> <b>Error</b> occured. <s>&#10071</s>")
    {
        await telegramBotClient.AnswerCallbackQueryAsync(callbackQuery.Id);

        return await telegramBotClient.SendTextMessageAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            text: errorMessage,
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }

    /// <summary>
    /// A method for processing an unidentified callback
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="callbackQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> SendUnknowCallbackQueryDataActionAsync(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        await telegramBotClient.AnswerCallbackQueryAsync(callbackQuery.Id);

        return await telegramBotClient.SendTextMessageAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            text: "<b>Sorry</b>, I have nothing tell you about this.",
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }

    /// <summary>
    /// The method for getting the original file
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="callbackQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> SendOriginalFile(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery.Message == null) { return await SendUnknowCallbackQueryDataActionAsync(telegramBotClient, callbackQuery, cancellationToken); }

        string pathToOriginalFile = DirectoryHelper.GetDirectoryFromEnvironment("OriginalFilePath");

        await telegramBotClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: $"{Char.ConvertFromUtf32(int.Parse("23F3", System.Globalization.NumberStyles.HexNumber))} Downloading original file.",
            cancellationToken: cancellationToken
            );

        try
        {
            await using FileStream fs = System.IO.File.OpenRead(pathToOriginalFile!);

            return await telegramBotClient.SendDocumentAsync(
             chatId: callbackQuery.Message!.Chat.Id,
             document: InputFile.FromStream(stream: fs, fileName: "MoscowIceHills.csv"),
             caption: "Here you have the <b>original</b> file.\nNow you can send it to me and start looking for a place to sled <s>&#128759</s>",
             parseMode: ParseMode.Html,
             cancellationToken: cancellationToken
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}\nException occured while trying to handle {callbackQuery.Data}.");
        }

        return await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken,
         "<s>&#10071</s><b>Failed</b> to download the original file. <s>&#10071</s> ");
    }

    /// <summary>
    /// The method for sending the bot operation menu
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> FileMenuAction(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
    {
        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>()
        {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData(text: $"{Char.ConvertFromUtf32(int.Parse("1F50D", System.Globalization.NumberStyles.HexNumber))} Show data", callbackData: "/showData"),
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData(text: $"{Char.ConvertFromUtf32(int.Parse("1F503", System.Globalization.NumberStyles.HexNumber))} Sort data", callbackData: "/sortMenu"),
                InlineKeyboardButton.WithCallbackData(text: $"{Char.ConvertFromUtf32(int.Parse("1F517", System.Globalization.NumberStyles.HexNumber))} Filter data", callbackData: "/filterMenu")
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData(text: $"{Char.ConvertFromUtf32(int.Parse("1F4E6", System.Globalization.NumberStyles.HexNumber))} Download data", callbackData: "/downloadMenu")
            }
        });

        return await telegramBotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "What do you want <b>me</b> to do?",
            parseMode: ParseMode.Html,
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken
        );
    }

    /// <summary>
    /// A method for sending a field selection menu for an action
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="callbackQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="isSorting"></param>
    /// <returns></returns>
    public static async Task<Message> FieldDataMenu(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken, bool isSorting)
    {   
        InlineKeyboardMarkup inlineKeyboard = isSorting ? SortHelper.CreateSortingMarkup() : FilterHelper.CreateFilteringMarkup();
        string textActionToShow;
        if (isSorting) {textActionToShow =$"<b>Select</b> the field by which the data will be <b>sorted</b> and sorting direction:\n<s>&#11015</s>Arrow down - <b>ascending</b>\n<s>&#11014</s>Arrow up - <b>descending</b>";}
        else {textActionToShow = $"<b>Select</b> the field by which the data will be <b>filtered</b>";}

        return await telegramBotClient.EditMessageTextAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            messageId: callbackQuery.Message.MessageId,
            text: textActionToShow,
            parseMode: ParseMode.Html,
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken
        );
    }

    /// <summary>
    /// A method for displaying unique filtering fields
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="callbackQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> FilterFieldValuesAction(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery.Message == null || callbackQuery.Data == null) { return await SendUnknowCallbackQueryDataActionAsync(telegramBotClient, callbackQuery, cancellationToken); }

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
            if (!isConvCorrect) {throw new Exception("Failed to convert .json to data collection.");}
            
            replyUniqueFieldsKeyboard = FilterHelper.CreateValuesMarkup(lstWithData, callbackData, out bool isNoData);

            System.IO.File.WriteAllText(tmpPathToSaveChoise, callbackData);

            if (isNoData)
            {
                if (System.IO.File.Exists(tmpPathToSaveChoise)) {System.IO.File.Delete(tmpPathToSaveChoise);}
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
        if (message.Text == null) {return await VariableErrorMessageAction(telegramBotClient, message, cancellationToken, "Choose the value from keyboard.");}

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
            if (!isConvCorrect) {throw new Exception("Failed to convert .json to data collection.");}

            List<IceHillData> lstFilteredData = FilterHelper.FilterDataByFieldAndParam(lstWithData, fieldToFilter, message.Text);

            JSONProcessing jsonDataRewriter = new JSONProcessing(pathToLoadData);
            Stream streamToClose = jsonDataRewriter.Write(lstFilteredData, out bool isWCS);
            streamToClose.Close();
            if (!isWCS) {throw new Exception("Failed to rewrite filtered data to the local dir.");}
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
            if (System.IO.File.Exists(tmpPathToSaveChoise)) {System.IO.File.Delete(tmpPathToSaveChoise);}
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
        if (callbackQuery.Message == null || callbackQuery.Data == null) { return await SendUnknowCallbackQueryDataActionAsync(telegramBotClient, callbackQuery, cancellationToken); }

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

    /// <summary>
    /// Method for displaying data
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="callbackQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> ShowDataAction(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery.Message == null) { return await SendUnknowCallbackQueryDataActionAsync(telegramBotClient, callbackQuery, cancellationToken); }

        string userName = callbackQuery.From.Username!.ToString();
        string chatId = callbackQuery.Message.Chat.Id.ToString();
        string pathToLoadData = DirectoryHelper.GetDirectoryFromEnvironment("PathToLoadedData", $"{chatId}_{userName}.json");

        if (!System.IO.File.Exists(pathToLoadData))
        {
            return await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken, "Seems like you didn't upload the file.\nI have nothing to work with <s>&#128577</s>");
        }
        try
        {
            string[] lstDataStringLines = System.IO.File.ReadAllLines(pathToLoadData);
            StringBuilder unformatedJsonLine = new StringBuilder(string.Join("\n", lstDataStringLines));
            string formatedJsonLines;
            if (unformatedJsonLine.Length > 4096) {formatedJsonLines = $"```json\n{unformatedJsonLine.ToString(0, 4096)}\n```";}
            else {formatedJsonLines = $"```json\n{unformatedJsonLine}\n```";}

            await telegramBotClient.EditMessageTextAsync(
                chatId: callbackQuery.Message.Chat.Id,
                messageId: callbackQuery.Message.MessageId,
                text: formatedJsonLines,
                parseMode: ParseMode.MarkdownV2,
                cancellationToken: cancellationToken
            );

            if (unformatedJsonLine.Length > 4096)
            {
                await telegramBotClient.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: "I <b>can't</b> show all of the data due to it's size.\nIf you want to see all of it you can download the file.",
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken
                );
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}\nException occured while trying to handle {callbackQuery.Data}.");

            await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken,
            "<s>&#10071</s><b>Failed</b> to show the file");
        }

        return await FileMenuAction(telegramBotClient, callbackQuery.Message, cancellationToken);
    }

    /// <summary>
    /// Method for downloading data in json format
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="callbackQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> DownloadJsonDataAction(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery.Message == null) { return await SendUnknowCallbackQueryDataActionAsync(telegramBotClient, callbackQuery, cancellationToken); }

        string userName = callbackQuery.From.Username!.ToString();
        string chatId = callbackQuery.Message.Chat.Id.ToString();
        string pathToLoadData = DirectoryHelper.GetDirectoryFromEnvironment("PathToLoadedData", $"{chatId}_{userName}.json");

        await telegramBotClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: $"{Char.ConvertFromUtf32(int.Parse("23F3", System.Globalization.NumberStyles.HexNumber))} Downloading file.",
            cancellationToken: cancellationToken
            );

        if (!System.IO.File.Exists(pathToLoadData))
        {
            return await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken, "Seems like you didn't upload the file.\nI have nothing to work with <s>&#128577</s>");
        }
        try
        {
            await using FileStream fs = System.IO.File.OpenRead(pathToLoadData!);

            await telegramBotClient.SendDocumentAsync(
             chatId: callbackQuery.Message!.Chat.Id,
             document: InputFile.FromStream(stream: fs, fileName: $"{userName}.json"),
             caption: "<s>&#9989</s> And here it is!",
             parseMode: ParseMode.Html,
             cancellationToken: cancellationToken
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}\nException occured while trying to handle {callbackQuery.Data}.");

            await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken,
            "<s>&#10071</s><b>Failed</b> to download the file. Try again later");
        }

        return await FileMenuAction(telegramBotClient, callbackQuery.Message, cancellationToken);
    }

    /// <summary>
    /// Method for downloading data in csv format
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="callbackQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> DownloadCSVDataAction(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery.Message == null) { return await SendUnknowCallbackQueryDataActionAsync(telegramBotClient, callbackQuery, cancellationToken); }

        string userName = callbackQuery.From.Username!.ToString();
        string chatId = callbackQuery.Message.Chat.Id.ToString();
        string pathToLoadData = DirectoryHelper.GetDirectoryFromEnvironment("PathToLoadedData", $"{chatId}_{userName}.json");

        await telegramBotClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: $"{Char.ConvertFromUtf32(int.Parse("23F3", System.Globalization.NumberStyles.HexNumber))} Downloading file.",
            cancellationToken: cancellationToken
            );

        if (!System.IO.File.Exists(pathToLoadData))
        {
            return await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken, "Seems like you haven't uploaded any data yet.\nI have nothing to work with <s>&#128577</s>");
        }

        string pathToServeCsvLoading = DirectoryHelper.GetDirectoryFromEnvironment("PathToLoadedData", $"{chatId}_{userName}_tmp.csv");
        try
        {
            await using FileStream fs = System.IO.File.OpenRead(pathToLoadData!);
            JSONProcessing jsonToDataConv = new JSONProcessing();
            List<IceHillData> lstWithData = jsonToDataConv.Read(fs, out bool isConvCorrect);
            fs.Close();
            if (!isConvCorrect)
            {
                throw new Exception("Failed to convert .json to data collection.");
            }

            CSVProcessing csvWriter2 = new CSVProcessing(pathToServeCsvLoading);
            Stream streamCSVLoading = csvWriter2.Write(lstWithData, out bool isWC2);
            streamCSVLoading.Close();

            if (!isWC2) { throw new Exception("Failed to write data collection to temporary .csv file."); }

            await using FileStream fsCsv = System.IO.File.OpenRead(pathToServeCsvLoading!);

            await telegramBotClient.SendDocumentAsync(
             chatId: callbackQuery.Message!.Chat.Id,
             document: InputFile.FromStream(stream: fsCsv, fileName: $"{userName}.csv"),
             caption: "<s>&#9989</s> And here it is!",
             parseMode: ParseMode.Html,
             cancellationToken: cancellationToken
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}\nException occured while trying to handle {callbackQuery.Data}.");
            await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken,
            "<s>&#10071</s><b>Failed</b> to download the file. Try again later");
        }
        finally
        {
            if (System.IO.File.Exists(pathToServeCsvLoading))
            {
                System.IO.File.Delete(pathToServeCsvLoading);
            }
        }

        return await FileMenuAction(telegramBotClient, callbackQuery.Message, cancellationToken);
    }

    /// <summary>
    /// Method for displaying the data loading menu
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="callbackQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> DownloadMenuAction(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery.Message == null) { return await SendUnknowCallbackQueryDataActionAsync(telegramBotClient, callbackQuery, cancellationToken); }

        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
        {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData(text: $"{Char.ConvertFromUtf32(int.Parse("1F4D1", System.Globalization.NumberStyles.HexNumber))} .csv", callbackData: "/downloadCSV"),
                InlineKeyboardButton.WithCallbackData(text: $"{Char.ConvertFromUtf32(int.Parse("2668", System.Globalization.NumberStyles.HexNumber))} .json", callbackData: "/downloadJSON"),
            }
        });

        return await telegramBotClient.EditMessageTextAsync(
            chatId: callbackQuery.Message.Chat.Id,
            messageId: callbackQuery.Message.MessageId,
            text: "Select data format",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken
        );
    }

    /// <summary>
    /// a method for downloading data from the user
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> LoadFileFromUserAction(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
    {
        if (message.Document == null) { return await VariableErrorMessageAction(telegramBotClient, message, cancellationToken, "<b>Nothing</b> has been given as a document."); }

        var document = message.Document;
        var fileId = document.FileId;
        var fileInfo = await telegramBotClient.GetFileAsync(fileId, cancellationToken);
        var filePath = fileInfo.FilePath;
        if (filePath == null) { return await VariableErrorMessageAction(telegramBotClient, message, cancellationToken, $"<b>Failed</b> to download {document.FileName}, try again later."); }
        string userName = message.From!.Username!.ToString();
        string chatId = message.Chat.Id.ToString();
        string pathToLoadData = DirectoryHelper.GetDirectoryFromEnvironment("PathToLoadedData", $"{chatId}_{userName}");

        try
        {

            await using FileStream streamToLoadFile = new FileStream(pathToLoadData, FileMode.Create, FileAccess.Write);
            await telegramBotClient.DownloadFileAsync(
                filePath: filePath,
                destination: streamToLoadFile,
                cancellationToken: cancellationToken
            );
            streamToLoadFile.Close();

            bool isCSVCorrect;
            List<IceHillData> lstTmpData = new List<IceHillData>();
            using FileStream streamToCSV = new FileStream(pathToLoadData, FileMode.Open, FileAccess.Read);
            {
                CSVProcessing csvObj = new CSVProcessing();
                lstTmpData = csvObj.Read(streamToCSV, out isCSVCorrect);
            };
            streamToCSV.Close();

            if (!isCSVCorrect)
            {
                bool isJsonCorrect;
                List<IceHillData> lstTmpData2 = new List<IceHillData>();
                using FileStream streamToJson = new FileStream(pathToLoadData, FileMode.Open, FileAccess.Read);
                {
                    JSONProcessing jsonObj = new JSONProcessing();
                    lstTmpData2 = jsonObj.Read(streamToJson, out isJsonCorrect);
                }
                streamToJson.Close();

                if (!isJsonCorrect)
                {
                    InlineKeyboardMarkup getNewFileInlineKeyBoard = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>{
                    new InlineKeyboardButton[1] {InlineKeyboardButton.WithCallbackData(text: $"{Char.ConvertFromUtf32(int.Parse("1F4E9", System.Globalization.NumberStyles.HexNumber))} Download original .csv file.", callbackData:"/originalFile")}
                    });

                    return await telegramBotClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "The file should be either <b>.json</b> or <b>.csv</b>.\nIf you don't have one use button below.",
                        parseMode: ParseMode.Html,
                        replyMarkup: getNewFileInlineKeyBoard,
                        cancellationToken: cancellationToken
                    );
                }

                JSONProcessing jsonWriter = new JSONProcessing($"{pathToLoadData}.json");
                Stream stream = jsonWriter.Write(lstTmpData2, out bool isJsonWriteCorrect);
                Console.WriteLine($"{pathToLoadData}.json was succesfully downloaded to the local directory.");
                stream.Dispose();
                stream.Close();
            }
            else
            {
                JSONProcessing jsonWriter2 = new JSONProcessing($"{pathToLoadData}.json");
                Stream stream2 = jsonWriter2.Write(lstTmpData, out bool isJsonWriteCorrect);
                Console.WriteLine($"{pathToLoadData}.json was succesfully downloaded to the local directory.");
                stream2.Dispose();
                stream2.Close();
            }
        }
        catch (Exception)
        {
            Console.WriteLine($"Error occured in LoadFileFromUserAction. {DateTime.Now}");
            return await VariableErrorMessageAction(telegramBotClient, message, cancellationToken, $"<b>Failed</b> to download <b>{document.FileName}</b>, try again later.");
        }
        finally
        {
            if (System.IO.File.Exists(pathToLoadData))
            {
                System.IO.File.Delete(pathToLoadData);
            }
        }

        await telegramBotClient.SendTextMessageAsync(
            chatId: chatId,
            text: $"<b>{document.FileName}</b> was <b>succesfuly</b> uploaded.",
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );

        return await FileMenuAction(telegramBotClient, message, cancellationToken);
    }
}