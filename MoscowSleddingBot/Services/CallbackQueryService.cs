using MoscowSleddingBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
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
        Task<Message> action = callbackQuery.Data switch
        {
            "/originalFile" => BotActions.SendOriginalFile(_telegramBotClient, callbackQuery, cancellationToken),
            _ => BotActions.UnknowCallbackQueryDataHandlerAsync(_telegramBotClient, callbackQuery, cancellationToken)
        };

        Message sentMessage = await action;
    }
}