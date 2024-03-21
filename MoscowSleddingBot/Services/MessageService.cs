using MoscowSleddingBot.Actions;
using MoscowSleddingBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using MoscowSleddingBot.Additional;

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
        _logger.LogInformation("Received message type: {MessageType}", message.Type);

        Task handelrMessage = message.Type switch
        {
            MessageType.Text => BotOnMessageTextHandler(_telegramBotClient, message, cancellationToken),
            MessageType.Document => BotOnMessageDocumentHandler(_telegramBotClient, message, cancellationToken),
            _ => BotActions.SendUnknowMessageTextActionAsync(_telegramBotClient, message, cancellationToken)
        };

        await handelrMessage;
    }
    private async Task BotOnMessageDocumentHandler(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
    {
        Message sentMessage = await BotActions.LoadFileFromUserAction(telegramBotClient, message, cancellationToken);

        _logger.LogInformation("The message was sent with id {SentMessageId} on {DateTime}", sentMessage.MessageId, DateTime.Now);
    }

    private async Task BotOnMessageTextHandler(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
    {
        Task<Message> action = message.Text switch
        {
            "/start" => BotActions.SendStartText(telegramBotClient, message, cancellationToken),
            _ => System.IO.File.Exists(DirectoryHelper.GetDirectoryFromEnvironment("PathToLoadedData", $"{message.Chat.Id}_{message.From!.Username}_TmpChoise.txt")) ? 
            BotActions.FilterDataAction(telegramBotClient, message, cancellationToken) : BotActions.SendUnknowMessageTextActionAsync(telegramBotClient, message, cancellationToken)
        };

        Message sentMessage = await action;

        _logger.LogInformation("The message was sent with id {SentMessageId} on {DateTime}", sentMessage.MessageId, DateTime.Now);
    }
}