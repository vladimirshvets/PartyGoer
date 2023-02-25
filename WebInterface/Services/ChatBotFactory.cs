using PartyGoer.ChatBot;
using PartyGoer.TelegramChatBot;

namespace WebInterface.Services;

public class ChatBotFactory
{
	/// <summary>
	/// Returns new chat bot instance based on specified type.
	/// </summary>
	/// <param name="botType">Bot type</param>
	/// <param name="accessToken">Access token</param>
	/// <returns>The instance of selected bot.</returns>
	public IChatBot? GetChatBot(
		string botType, string accessToken)
	{
		return botType switch
		{
			TelegramChatBot.APP_ID => new TelegramChatBot(accessToken),
            _ => null
		};
	}
}

