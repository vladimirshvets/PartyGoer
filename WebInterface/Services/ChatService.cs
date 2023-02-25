using PartyGoer.ChatBot;
using WebInterface.Models.Settings;

namespace WebInterface.Services;

public class ChatService : IHostedService
{
    /// <summary>
    /// Bot configuration data
    /// </summary>
    private readonly BotConfiguration _botConfig;

    //private readonly IServiceProvider _serviceProvider;

    private readonly ChatBotProcessor _processor;

    public ChatService(
        BotConfiguration botConfig,
        ChatBotProcessor chatBotProcessor)
    {
        _botConfig = botConfig;
        //_serviceProvider = serviceProvider;
        //_processor = new ChatBotProcessor(_serviceProvider);
        _processor = chatBotProcessor;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_botConfig.Enabled)
        {
            InitializeBot(_botConfig.AppId, _botConfig.AccessToken);
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Create and start a bot of specified type.
    /// </summary>
    /// <param name="botType"></param>
    /// <param name="accessToken"></param>
    /// <returns></returns>
	private async Task InitializeBot(string botType, string accessToken)
    {
        ChatBotFactory botFactory = new ChatBotFactory();
        IChatBot bot =
            botFactory.GetChatBot(botType, accessToken) ??
            throw new NullReferenceException(
                $"Cannot instantiate a bot of specified type: {botType}");

        using CancellationTokenSource cts = new();
        await bot.TestConnectionAsync(cts);

        bot.MessageReceived += _processor.HandleReceivedMessage;
        bot.StartBot(cts);
    }
}
