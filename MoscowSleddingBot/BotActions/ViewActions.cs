using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using MoscowSleddingBot.Additional;
using System.Text;

namespace MoscowSleddingBot.Actions;

public static partial class BotActions
{
    /// <summary>
    /// Method for displaying data in json format to the user
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="callbackQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> ShowDataAction(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery.Message == null) { return await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken, "<b>Sorry</b>, I have nothing tell you about this."); }

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
            if (unformatedJsonLine.Length > 4096) { formatedJsonLines = $"```json\n{unformatedJsonLine.ToString(0, 4096)}\n```"; }
            else { formatedJsonLines = $"```json\n{unformatedJsonLine}\n```"; }

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
    /// Method for displaying the data loading menu
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="callbackQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<Message> DownloadMenuAction(ITelegramBotClient telegramBotClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        if (callbackQuery.Message == null) { return await VariableErrorCallbackAction(telegramBotClient, callbackQuery, cancellationToken, "<b>Sorry</b>, I have nothing tell you about this."); }

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
        if (isSorting) { textActionToShow = $"<b>Select</b> the field by which the data will be <b>sorted</b> and sorting direction:\n<s>&#11015</s>Arrow down - <b>ascending</b>\n<s>&#11014</s>Arrow up - <b>descending</b>"; }
        else { textActionToShow = $"<b>Select</b> the field by which the data will be <b>filtered</b>"; }

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
}