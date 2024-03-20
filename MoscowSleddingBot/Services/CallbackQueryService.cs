using MoscowSleddingBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using MoscowSleddingBot.Additional;
using MoscowSleddingBot.Actions;

namespace MoscowSleddingBot.Services;

public class CallbackQueryService: ICallbackQueryService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ILogger<CallbackQueryService> _logger;

    public CallbackQueryService(ITelegramBotClient telegramBotClient, ILogger<CallbackQueryService> logger)
    {
        _telegramBotClient = telegramBotClient;
        _logger = logger;
    }

    public async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {   
        _logger.LogInformation("Received message type: {MessageType}", callbackQuery.Message!.Type);

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
            _ => BotActions.SendUnknowCallbackQueryDataActionAsync(_telegramBotClient, callbackQuery, cancellationToken)
        };

        Message sentMessage = await action;
        _logger.LogInformation("The message was sent with id {SentMessageId} on {DateTime}", sentMessage.MessageId, DateTime.Now);
    }
}