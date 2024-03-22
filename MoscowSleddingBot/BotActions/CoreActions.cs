using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MoscowSleddingBot.Actions;

public static partial class BotActions
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
}