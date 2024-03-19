using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Runtime.InteropServices;

namespace MoscowSleddingBot.Actions;

public static class BotActions
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

    public static async Task<Message> SendUnknowMessageTextResponse(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
    {
        return await telegramBotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "<b>Sorry</b>, I don't know what to tell you about this.",
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }

    public static async Task<Message> UnknowCallbackQueryDataHandlerAsync(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        await telegramBotClient.AnswerCallbackQueryAsync(callbackQuery.Id);

        return await telegramBotClient.SendTextMessageAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            text: "<b>Sorry</b>, I don't know what to tell you about this.",
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }

    public static async Task<Message> SendOriginalFile(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        string? pathToOriginalFile;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) { pathToOriginalFile = Environment.GetEnvironmentVariable("OriginalFilePathWin"); }
        else { pathToOriginalFile = Environment.GetEnvironmentVariable("OriginalFilePathUNIX"); }

        try
        {
            await using FileStream fs = System.IO.File.OpenRead(pathToOriginalFile!);

            await telegramBotClient.AnswerCallbackQueryAsync(callbackQueryId: callbackQuery.Id,
             text: $"{Char.ConvertFromUtf32(int.Parse("23F3", System.Globalization.NumberStyles.HexNumber))} Downloading original file.",
            cancellationToken: cancellationToken);

            return await telegramBotClient.SendDocumentAsync(
             chatId: callbackQuery.Message!.Chat.Id,
             document: InputFile.FromStream(stream: fs, fileName: "MoscowIceHills.csv"),
             caption: "Original file <b>successfully</b> downloaded.",
             parseMode: ParseMode.Html,
             cancellationToken: cancellationToken
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}\nException occured while trying to handle {callbackQuery.Data}.");
        }

        await telegramBotClient.AnswerCallbackQueryAsync(callbackQueryId: callbackQuery.Id, cancellationToken: cancellationToken);

        return await telegramBotClient.SendTextMessageAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            text: "<s>&#10071</s> <b>Failed</b> to download the original file. <s>&#10071</s> ",
            parseMode: ParseMode.Html,
            cancellationToken: cancellationToken
        );
    }


}