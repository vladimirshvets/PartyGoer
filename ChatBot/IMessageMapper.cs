namespace PartyGoer.ChatBot;

/// <summary>
/// Message mapper interface.
/// </summary>
/// <typeparam name="T">Type of original message.</typeparam>
public interface IMessageMapper<T>
{
	/// <summary>
	/// Map original message to general format.
	/// </summary>
	/// <param name="original">Original message</param>
	/// <returns>Mapped message.</returns>
	public BotMessage Map(T original);
}
