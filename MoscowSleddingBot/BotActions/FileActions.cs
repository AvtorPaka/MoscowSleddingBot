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
    /// The method for getting the original file
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="callbackQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> SendOriginalFile(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery.Message == null) { return await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken, "<b>Sorry</b>, I have nothing tell you about this."); }

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
                stream.Dispose();
                stream.Close();
            }
            else
            {
                JSONProcessing jsonWriter2 = new JSONProcessing($"{pathToLoadData}.json");
                Stream stream2 = jsonWriter2.Write(lstTmpData, out bool isJsonWriteCorrect);
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

    /// <summary>
    /// Method for downloading data in csv format
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="callbackQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> DownloadCSVDataAction(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery.Message == null) { return await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken, "<b>Sorry</b>, I have nothing tell you about this."); }

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
    /// Method for downloading data in json format
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="callbackQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> DownloadJsonDataAction(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery.Message == null) { return await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken, "<b>Sorry</b>, I have nothing tell you about this."); }

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
}