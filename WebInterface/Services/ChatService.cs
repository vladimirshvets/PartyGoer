using PartyGoer.ChatBot;
using PartyGoer.ChatBotFactory;
using WebInterface.Models;
using WebInterface.Models.Settings;
using WebInterface.Repositories;

namespace WebInterface.Services;

public class ChatService : IHostedService
{
    /// <summary>
    /// Bot configuration data.
    /// </summary>
    private readonly BotConfiguration _botConfig;

    //private readonly ChatBotProcessor _processor;

    /// <summary>
    /// Chat repository.
    /// </summary>
    private readonly ChatRepository _chatRepository;

    /// <summary>
    /// Service provider.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    private readonly ILogger<ChatService> _logger;

    public ChatService(
        BotConfiguration botConfig,
        //ChatBotProcessor chatBotProcessor,
        ChatRepository chatRepository,
        IServiceProvider serviceProvider,
        ILogger<ChatService> logger)
    {
        _botConfig = botConfig;
        //_processor = chatBotProcessor;
        _chatRepository = chatRepository;
        _serviceProvider = serviceProvider;
        _logger = logger;
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
        ChatBotFactory botFactory = new ChatBotFactory(_serviceProvider);
        IChatBot bot =
            botFactory.GetChatBot(botType, accessToken) ??
            throw new NullReferenceException(
                $"Cannot instantiate a bot of specified type: {botType}");

        using CancellationTokenSource cts = new();
        await bot.TestConnectionAsync(cts);

        bot.MessageReceived += HandleReceivedMessage;
        bot.StartBot(cts);
    }

    /// <summary>
    /// Handle received message.
    /// </summary>
    /// <param name="sender">Chat bot instance</param>
    /// <param name="e">Event arguments</param>
    private void HandleReceivedMessage(
        IChatBot sender, BotMessageReceivedEventArgs e)
    {
        BotMessage message = e.BotMessage;

        _logger.LogDebug("HANDLER: NEW MESSAGE RECEIVED");
        //_logger.LogDebug($"Chat {message.ChatId} " +
        //    $"(title: {message.ChatTitle}) " +
        //    $"From {message.UserId} {message.UserNickname} " +
        //    $"/ {message.UserFirstname} {message.UserLastname} " +
        //    $"| Message Id: {message.MessageId}" +
        //    $"| Message Text: {message.Text} " +
        //    $"| Received sticker: {message.Sticker}");

        Chat? chat =
            _chatRepository.GetChatAsync(message.AppId, message.ChatId).Result;
        if (chat == null)
        {
            // ToDo:
            // Remove hardcoded values.
            chat = new Chat()
            {
                AppId = message.AppId,
                ChatId = message.ChatId,
                Type = "private",
                Title = message.ChatTitle,
                FullName = message.UserFullname,
                IsAuthorized = true,
                IsBeingListened = true
            };

            _chatRepository.SaveChatAsync(chat);
        }

        // ToDo:
        // Call message processor.
    }
}
