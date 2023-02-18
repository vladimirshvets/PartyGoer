
using System.Collections.Specialized;
using System.Configuration;
using PartyGoer.ChatBot;
using ConsoleApp;
using NLog;

LogManager.LoadConfiguration("..//..//..//nlog.config");
Logger logger = LogManager.GetCurrentClassLogger();
logger.Debug($"Starting up");

// Read settings from config file.
NameValueCollection tgBotGeneralSettings =
    ConfigurationManager.GetSection("telegramBotSettings/general")
    as NameValueCollection;
string tgAccessToken = tgBotGeneralSettings.Get("accessToken");

// Create chat bot instance.
ChatBotFactory botFactory = new ChatBotFactory();
IChatBot tgBot = botFactory.GetChatBot("telegram", tgAccessToken);

using CancellationTokenSource cts = new();
tgBot.TestConnectionAsync(cts);
tgBot.StartBot(cts);

Console.ReadLine();

tgBot.StopBot(cts);
