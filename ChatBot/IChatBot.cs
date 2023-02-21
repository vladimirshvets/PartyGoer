using System.Security.Principal;

namespace PartyGoer.ChatBot;

/// <summary>
/// Chat bot interface.
/// </summary>
public interface IChatBot
{
    public delegate void MessageReceivedEventHandler(
        IChatBot sender, BotMessageReceivedEventArgs e);

    /// <summary>
    /// Invoked when a message is received.
    /// </summary>
    public event MessageReceivedEventHandler? MessageReceived;

    /// <summary>
    /// Start listening to messages.
    /// </summary>
    /// <param name="cts">Cancellation token source</param>
    public void StartBot(CancellationTokenSource cts);

    /// <summary>
    /// Stop listening to messages.
    /// </summary>
    /// <param name="cts">Cancellation token source</param>
    public void StopBot(CancellationTokenSource cts);

    /// <summary>
    /// Checks the connection between application and bot.
    /// </summary>
    /// <param name="cts">Cancellation token source</param>
    public Task TestConnectionAsync(CancellationTokenSource cts);

    /// <summary>
    /// Sends text message to the chat.
    /// </summary>
    /// <param name="cts">Cancellation token source</param>
    /// <param name="chatId">Chat ID</param>
    /// <param name="messageText">Message text</param>
    /// <param name="disableNotification">Disable notification</param>
    /// <param name="replyToMessageId">Message ID to reply or null</param>
    public Task SendTextMessageAsync(
        long chatId,
        string messageText,
        bool disableNotification,
        int? replyToMessageId,
        CancellationTokenSource cts);

    /// <summary>
    /// Sends text message to the chat.
    /// </summary>
    /// <param name="cts">Cancellation token source</param>
    /// <param name="chatId">Chat ID</param>
    /// <param name="stickerFileId">Sticker file ID</param>
    /// <param name="disableNotification">Disable notification</param>
    /// <param name="replyToMessageId">Message ID to reply or null</param>
    public Task SendStickerAsync(
        long chatId,
        string stickerFileId,
        bool disableNotification,
        int? replyToMessageId,
        CancellationTokenSource cts);
}
