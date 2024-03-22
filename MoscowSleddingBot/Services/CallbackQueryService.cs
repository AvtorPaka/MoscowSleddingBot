using MoscowSleddingBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using MoscowSleddingBot.Additional;
using MoscowSleddingBot.Actions;

namespace MoscowSleddingBot.Services;

/// <summary>
/// A class for processing callback queries
/// </summary>
public class CallbackQueryService: ICallbackQueryService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ILogger<CallbackQueryService> _logger;

    public CallbackQueryService(ITelegramBotClient telegramBotClient, ILogger<CallbackQueryService> logger)
    {
        _telegramBotClient = telegramBotClient;
        _logger = logger;
    }

    /// <summary>
    /// Method of processing callback requests
    /// </summary>
    /// <param name="callbackQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {   
       _logger.LogInformation(">Received callback => Data: {mType} | MessageType : {dmType} | ID : {messageID} | ChatID : {chatID} | DateTime : {date} | UserData : {userData}",
        callbackQuery.Data, callbackQuery.Message!.Type, callbackQuery.Message!.Chat.Id, callbackQuery.Message.MessageId, DateTime.Now, callbackQuery.From);;

        Task<Message> action = callbackQuery.Data switch
        {
            "/originalFile" => BotActions.SendOriginalFile(_telegramBotClient, callbackQuery, cancellationToken),
            "/downloadMenu" => BotActions.DownloadMenuAction(_telegramBotClient, callbackQuery, cancellationToken),
            "/downloadJSON" => BotActions.DownloadJsonDataAction(_telegramBotClient, callbackQuery, cancellationToken),
            "/downloadCSV" => BotActions.DownloadCSVDataAction(_telegramBotClient, callbackQuery, cancellationToken),
            "/showData" => BotActions.ShowDataAction(_telegramBotClient, callbackQuery, cancellationToken),
            "/sortMenu" => BotActions.FieldDataMenu(_telegramBotClient, callbackQuery, cancellationToken, true),
            string curData when SortHelper.lstWithSortingHeaders.Contains(curData) => BotActions.SortDataAction(_telegramBotClient, callbackQuery, cancellationToken),
            "/filterMenu" => BotActions.FieldDataMenu(_telegramBotClient, callbackQuery, cancellationToken, false),
            string curData when FilterHelper.lstWithFilterHeaders.Contains(curData) => BotActions.FilterFieldValuesAction(_telegramBotClient, callbackQuery, cancellationToken),
            _ => BotActions.VariableErrorCallbackAction(_telegramBotClient, callbackQuery, cancellationToken, "<b>Sorry</b>, I have nothing tell you about this.")
        };

        Message sentMessage = await action;
        _logger.LogInformation(">>Sent message => Type: {mType} | ID: {messageID} | ChatID : {chatID} | DateTime : {date}",
         sentMessage.Type, sentMessage.MessageId, sentMessage.Chat.Id, DateTime.Now);
    }
}