using PartyGoer.ChatBot;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PartyGoer.TelegramChatBot;

public class TelegramChatBot : IChatBot
{
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
    /// Message processor.
    /// </summary>
    private TelegramMessageProcessor _messageProcessor;

    /// <summary>
    /// Bot constructor.
    /// </summary>
    /// <param name="accessToken">Bot access token.</param>
    public TelegramChatBot(string accessToken)
    {
        _accessToken = accessToken;
        _client = new TelegramBotClient(_accessToken);
        _receiverOptions = new()
        {
            // Receive all update types.
            AllowedUpdates = Array.Empty<UpdateType>()
        };
        _messageProcessor = new TelegramMessageProcessor();
    }

    public async Task<bool> TestConnection()
    {
        var me = await _client.GetMeAsync();
        Console.WriteLine($"Test: User {me.Id}, name is {me.FirstName}.");
        return true;
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
        Console.WriteLine($"Start listening for @{me.Result.Username}");
    }

    public void StopBot(CancellationTokenSource cts)
    {
        // Send cancellation request to stop bot.
        cts.Cancel();
    }

    /// <summary>
    /// Handle chat updates.
    /// </summary>
    /// <param name="botClient">Bot client</param>
    /// <param name="update">Chat update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>true if the update has been processed successfully.</returns>
    private async Task<bool> HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        return await _messageProcessor.ProcessAsync(
            botClient, update, cancellationToken);
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

        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }
}
