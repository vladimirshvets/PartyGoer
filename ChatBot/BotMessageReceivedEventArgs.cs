namespace PartyGoer.ChatBot;

public class BotMessageReceivedEventArgs : EventArgs
{
	public BotMessage BotMessage { get; set; }

	public BotMessageReceivedEventArgs(BotMessage message)
	{
		BotMessage = message;
	}
}
