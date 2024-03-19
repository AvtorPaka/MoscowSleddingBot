using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Runtime.InteropServices;
using MoscowSleddingBot.Additional;
using DataLibrary;

namespace MoscowSleddingBot.Actions;

public static partial class BotActions
{
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

    public static async Task<Message> SendUnknowMessageTextActionAsync(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
    {
        return await telegramBotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "<b>Sorry</b>, I have nothing to tell you about this.",
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }

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

    public static async Task<Message> SendOriginalFile(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery.Message == null) { return await SendUnknowCallbackQueryDataActionAsync(telegramBotClient, callbackQuery, cancellationToken); }

        string pathToOriginalFile = DirectoryHelper.GetDirectoryFromEnvironment("OriginalFilePath");

        try
        {
            await using FileStream fs = System.IO.File.OpenRead(pathToOriginalFile!);

            await telegramBotClient.AnswerCallbackQueryAsync(callbackQueryId: callbackQuery.Id,
             text: $"{Char.ConvertFromUtf32(int.Parse("23F3", System.Globalization.NumberStyles.HexNumber))} Downloading original file.",
            cancellationToken: cancellationToken);

            return await telegramBotClient.SendDocumentAsync(
             chatId: callbackQuery.Message!.Chat.Id,
             document: InputFile.FromStream(stream: fs, fileName: "MoscowIceHills.csv"),
             caption: "Here you have the <b>original</b> file. Now you can send it to me and start looking for a place to sled <s>&#128759</s>",
             parseMode: ParseMode.Html,
             cancellationToken: cancellationToken
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}\nException occured while trying to handle {callbackQuery.Data}.");
        }

        return await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken,
         "<s>&#10071</s> <b>Failed</b> to download the original file. <s>&#10071</s> ");
    }

    public static async Task<Message> LoadFileFromUserAction(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
    {
        if (message.Document == null) { return await VariableErrorMessageAction(telegramBotClient, message, cancellationToken, "<b>Nothing</b> has been given as a document."); }

        var document = message.Document;
        var fileId = document.FileId;
        var fileInfo = await telegramBotClient.GetFileAsync(fileId, cancellationToken);
        var filePath = fileInfo.FilePath;
        if (filePath == null) { return await VariableErrorMessageAction(telegramBotClient, message, cancellationToken, $"<b>Failed</b> to download {document.FileName}, try again later."); }
        string userId = message.From!.Username!.ToString();
        string chatId = message.Chat.Id.ToString();
        string pathToLoadData = DirectoryHelper.GetDirectoryFromEnvironment("PathToLoadedData", $"{chatId}_{userId}");

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
                        text: "The file should be either <b>.json</b> or <b>.csv</b>. If you don't have one use button below.",
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
                //testing - it's working but got some trouble with Thread access

                // CSVProcessing csvWriter2 = new CSVProcessing($"{pathToLoadData}.csv");
                // Stream stream3 = csvWriter2.Write(lstTmpData, out bool isWC2);
                // stream3.Dispose();
                // stream3.Close();

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

        return await telegramBotClient.SendTextMessageAsync(
            chatId: chatId,
            text: $"<b>{document.FileName}</b> was <b>succesfuly</b> uploaded.",
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }
}