
using System.Collections.Specialized;
using System.Configuration;
using PartyGoer.ChatBot;
using ConsoleApp;

// Read settings from config file.
NameValueCollection tgBotGeneralSettings =
    ConfigurationManager.GetSection("telegramBotSettings/general")
    as NameValueCollection;
string tgAccessToken = tgBotGeneralSettings.Get("accessToken");

// Create chat bot instance.
ChatBotFactory botFactory = new ChatBotFactory();
IChatBot tgBot = botFactory.GetChatBot("telegram", tgAccessToken);

using CancellationTokenSource cts = new();
tgBot.TestConnection();
tgBot.StartBot(cts);

Console.ReadLine();

tgBot.StopBot(cts);
