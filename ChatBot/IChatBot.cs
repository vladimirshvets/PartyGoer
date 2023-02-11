namespace PartyGoer.ChatBot;

/// <summary>
/// Chat bot interface.
/// </summary>
public interface IChatBot
{
    /// <summary>
    /// Checks the connection between application and bot.
    /// </summary>
    /// <returns>true if the connection can be established.</returns>
    public Task<bool> TestConnection();

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
}
