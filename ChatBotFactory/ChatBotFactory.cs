using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PartyGoer.ChatBot;
using TelegramBot = PartyGoer.TelegramChatBot;

namespace PartyGoer.ChatBotFactory;

public class ChatBotFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ChatBotFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Returns new chat bot instance based on specified type.
    /// </summary>
    /// <param name="botType">Bot type</param>
    /// <param name="accessToken">Access token</param>
    /// <returns>The instance of selected bot.</returns>
    public IChatBot? GetChatBot(
        string botType, string accessToken)
    {
        switch (botType)
        {
            case TelegramBot.TelegramChatBot.APP_ID:
                var logger = _serviceProvider
                    .GetService<ILogger<TelegramBot.TelegramChatBot>>();
                return new TelegramBot.TelegramChatBot(accessToken, logger);

            default:
                return null;
		};
	}
}
