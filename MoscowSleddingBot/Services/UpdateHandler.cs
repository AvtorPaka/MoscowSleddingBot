using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using MoscowSleddingBot.Interfaces;

namespace MoscowSleddingBot.Services;
/// <summary>
/// The bot request handler class
/// </summary>
public class UpdateHandler: IUpdateHandler
{
    private readonly ILogger<UpdateHandler> _logger;
    private readonly IMessageService _messageService;
    private readonly ICallbackQueryService _callbackQueryService;

    public UpdateHandler(ILogger<UpdateHandler> logger, IMessageService messageService, ICallbackQueryService callbackQueryService)
    {
        _logger = logger;
        _messageService = messageService;
        _callbackQueryService = callbackQueryService;
    }

    /// <summary>
    /// Request processing method
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="update"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task HandleUpdateAsync(ITelegramBotClient telegramBotClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            Task handler = update.Type switch
            {
                UpdateType.Message => _messageService.BotOnMessageReceived(update.Message!, cancellationToken),
                UpdateType.EditedMessage => _messageService.BotOnMessageReceived(update.Message!, cancellationToken),
                UpdateType.CallbackQuery => _callbackQueryService.BotOnCallbackQueryReceived(update.CallbackQuery!, cancellationToken),
                _ => UnknownUpdateHandlerAsync(telegramBotClient, update, cancellationToken)
            };

            await handler;
        }
        catch (Exception ex)
        {
            _logger.LogInformation("{ExceptionMessage}\nException occured while handling {updateType}" ,ex.Message, update.Type);
        }
    }

    /// <summary>
    /// Method for the case of an unknown request type
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="update"></param>
    /// <param name="cToken"></param>
    /// <returns></returns>
    private Task UnknownUpdateHandlerAsync(ITelegramBotClient telegramBotClient, Update update, CancellationToken cToken)
    {   
        _logger.LogInformation("Unknow update type occured: {updateType}", update.Type);
        return Task.CompletedTask;
    }

    /// <summary>
    /// A method for handling errors during request processing
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="exception"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        string ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation("Handled error {ErrorMessage}", ErrorMessage);

        if (exception is RequestException) {await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);}
    }
}