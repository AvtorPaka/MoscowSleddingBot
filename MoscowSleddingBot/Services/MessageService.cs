using MoscowSleddingBot.Actions;
using MoscowSleddingBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using MoscowSleddingBot.Additional;

namespace MoscowSleddingBot.Services;

/// <summary>
/// A class for processing text queries
/// </summary>
public class MessageService : IMessageService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly ILogger<MessageService> _logger;

    public MessageService(ITelegramBotClient telegramBotClient, ILogger<MessageService> logger)
    {
        _telegramBotClient = telegramBotClient;
        _logger = logger;
    }

    /// <summary>
    /// A method for processing a text request and determining its type
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("---Received message - Type: {mType} - ID: {messageID} - ChatID : {chatID} - DateTime : {date}\n---UserData : {userData} ", message.Type,
         message.MessageId, message.Chat.Id, DateTime.Now, message.From == null ? "NoData" : message.From.ToString());

        Task handelrMessage = message.Type switch
        {
            MessageType.Text => BotOnMessageTextHandler(_telegramBotClient, message, cancellationToken),
            MessageType.Document => BotOnMessageDocumentHandler(_telegramBotClient, message, cancellationToken),
            _ => BotActions.SendUnknowMessageTextActionAsync(_telegramBotClient, message, cancellationToken)
        };

        await handelrMessage;
    }

    /// <summary>
    /// The method of processing a document request
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task BotOnMessageDocumentHandler(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
    {
        Message sentMessage = await BotActions.LoadFileFromUserAction(telegramBotClient, message, cancellationToken);

        _logger.LogInformation("Sent message - Type: {mType} - ID: {messageID} - ChatID : {chatID} - DateTime : {date} ", sentMessage.Type, sentMessage.MessageId, sentMessage.Chat.Id, DateTime.Now);
    }

    /// <summary>
    /// The method of processing a text request
    /// </summary>
    /// <param name="telegramBotClient"></param>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task BotOnMessageTextHandler(ITelegramBotClient telegramBotClient, Message message, CancellationToken cancellationToken)
    {
        Task<Message> action = message.Text switch
        {
            "/start" => BotActions.SendStartText(telegramBotClient, message, cancellationToken),
            _ => System.IO.File.Exists(DirectoryHelper.GetDirectoryFromEnvironment("PathToLoadedData", $"{message.Chat.Id}_{message.From!.Username}_TmpChoise.txt")) ? 
            BotActions.FilterDataAction(telegramBotClient, message, cancellationToken) : BotActions.SendUnknowMessageTextActionAsync(telegramBotClient, message, cancellationToken)
        };

        Message sentMessage = await action;

        _logger.LogInformation("Sent message - Type: {mType} - ID: {messageID} - ChatID : {chatID} - DateTime : {date} ", sentMessage.Type, sentMessage.MessageId, sentMessage.Chat.Id, DateTime.Now);
    }
}