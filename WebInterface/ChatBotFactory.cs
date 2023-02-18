using PartyGoer.ChatBot;
using PartyGoer.TelegramChatBot;

namespace WebInterface
{
	public class ChatBotFactory
	{
		/// <summary>
		/// Returns new chat bot instance based on specified type.
		/// </summary>
		/// <param name="botType">Bot type</param>
		/// <param name="accessToken">Access token</param>
		/// <returns>The instance of selected bot.</returns>
		public IChatBot GetChatBot(
			string botType, string accessToken)
		{
			switch (botType)
			{
				case "telegram":
					return new TelegramChatBot(accessToken);

				default:
					return null;
			}
		}
	}
}

