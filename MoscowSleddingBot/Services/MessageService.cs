using MoscowSleddingBot.Actions;
using MoscowSleddingBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MoscowSleddingBot.Services;

public class MessageService : IMessageService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ILogger<MessageService> _logger;

    public MessageService(ITelegramBotClient telegramBotClient, ILogger<MessageService> logger)
    {
        _telegramBotClient = telegramBotClient;
        _logger = logger;
    }

    public async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Receive message type: {MessageType}", message.Type);

        Task handelrMessage = message.Type switch
        {
            MessageType.Text => BotOnMessageTextHandler(_telegramBotClient, message, cancellationToken),
            // MessageType.Document => BotOnMessageDocumentHandler(telegramBotClient, message, cancellationToken),
            _ => BotActions.SendUnknowMessageTextResponse(_telegramBotClient, message, cancellationToken)
        };

        await handelrMessage;

        static async Task BotOnMessageTextHandler(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
        {
            Task<Message> action = message.Text switch
            {
                "/start" => BotActions.SendStartText(telegramBotClient, message, cancellationToken),
                _ => BotActions.SendUnknowMessageTextResponse(telegramBotClient, message, cancellationToken)
            };

            Message sentMessage = await action;
        }

        //Регистрация документа работает
        // static async Task BotOnMessageDocumentHandler(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
        // {

        // }
    }
}