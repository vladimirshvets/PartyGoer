using Microsoft.Extensions.Logging;
using PartyGoer.ChatBot;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PartyGoer.TelegramChatBot;

/// <summary>
/// Implementation of Telegram chat bot.
/// </summary>
public class TelegramChatBot : IChatBot
{
    /// <summary>
    /// Messaging app identifier.
    /// </summary>
    public const string APP_ID = "telegram";

    public string GetAppId => APP_ID;

    public event IChatBot.MessageReceivedEventHandler? MessageReceived;

    /// <summary>
    /// Bot access token.
    /// </summary>
    private readonly string _accessToken;

    /// <summary>
    /// Bot client instance.
    /// </summary>
    private TelegramBotClient _client;

    /// <summary>
    /// Message receiving options.
    /// </summary>
    private ReceiverOptions _receiverOptions;

    /// <summary>
    /// Instance of logger.
    /// </summary>
    private ILogger<TelegramChatBot>? _logger;

    /// <summary>
    /// Telegram bot constructor.
    /// </summary>
    /// <param name="accessToken">Bot access token.</param>
    /// <param name="logger">Instance of logger.</param>
    public TelegramChatBot(
        string accessToken, ILogger<TelegramChatBot>? logger = null)
    {
        _accessToken = accessToken;
        _client = new TelegramBotClient(_accessToken);
        _receiverOptions = new()
        {
            // Receive all update types.
            AllowedUpdates = Array.Empty<UpdateType>()
        };
        _logger = logger;
    }

    public void StartBot(CancellationTokenSource cts)
    {
        // StartReceiving does not block the caller thread.
        // Receiving is done on the ThreadPool.
        _client.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: _receiverOptions,
            cancellationToken: cts.Token
        );

        var me = _client.GetMeAsync();
        _logger?.LogDebug($"Start listening for @{me.Result.Username}");
    }

    public void StopBot(CancellationTokenSource cts)
    {
        // Send cancellation request to stop bot.
        cts.Cancel();
    }

    public async Task TestConnectionAsync(CancellationTokenSource cts)
    {
        var me = await _client.GetMeAsync();
        _logger?.LogDebug($"Test: User {me.Id}, name is {me.FirstName}.");
    }

    public async Task SendTextMessageAsync(
        long chatId,
        string messageText,
        bool disableNotification,
        int? replyToMessageId,
        CancellationTokenSource cts)
    {
        await _client.SendTextMessageAsync(
            chatId: chatId,
            text: messageText,
            disableNotification: disableNotification,
            replyToMessageId: replyToMessageId,
            cancellationToken: cts.Token);
    }

    public async Task SendStickerAsync(
        long chatId,
        string stickerFileId,
        bool disableNotification,
        int? replyToMessageId,
        CancellationTokenSource cts)
    {
        await _client.SendStickerAsync(
            chatId: chatId,
            sticker: stickerFileId,
            disableNotification: disableNotification,
            replyToMessageId: replyToMessageId,
            cancellationToken: cts.Token);
    }

    /// <summary>
    /// Handle chat updates.
    /// </summary>
    /// <param name="botClient">Bot client</param>
    /// <param name="update">Chat update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>true if the update has been processed successfully.</returns>
    private async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        // Only process Message updates:
        // https://core.telegram.org/bots/api#message
        if (update.Message is not { } message)
        {
            return;
        }

        // Only process text messages.
        if (message.Text is not { } messageText)
        {
            return;
        }

        _logger?.LogDebug($"Chat {message.Chat.Id} " +
            $"(title: {message.Chat.Title}) " +
            $"| From {message.From?.Id} {message.From?.Username} " +
            $"/ {message.From?.FirstName} {message.From?.LastName} " +
            $"| Message Id: {message.MessageId} " +
            $"| Type: {message.Type} " +
            $"| Text: {messageText} " +
            $"| Sticker: {message.Sticker?.SetName} " +
            $"(fileId = {message.Sticker?.FileId})");

        BotMessage botMessage = new MessageMapper().Map(message);
        var args = new BotMessageReceivedEventArgs(botMessage);
        MessageReceived?.Invoke(this, args);
    }

    /// <summary>
    /// Handle errors.
    /// </summary>
    /// <param name="botClient">Bot client</param>
    /// <param name="exception">Exception to handle</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Completed task.</returns>
    private Task HandlePollingErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n" +
                   $"{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger?.LogCritical(errorMessage);
        return Task.CompletedTask;
    }
}
