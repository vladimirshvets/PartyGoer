using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Threading;
using NLog;
using PartyGoer.ChatBot;
using PartyGoer.Infotainment;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace WebInterface.Models.Bot;

public class ChatBotProcessor
{
    //private NameValueCollection _helloStickers;

    //private NameValueCollection _beerStickers;

    //private NameValueCollection _foodStickers;

    /// <summary>
    /// Instance of logger.
    /// </summary>
    private Logger _logger;

    public ChatBotProcessor()
    {
        _logger = LogManager.GetCurrentClassLogger();

        // Set stickers.
        // ToDo:
        // Remove ConfigurationManager package.
        // Load a collection of stickers from database for specific chat.
        //_helloStickers =
        //    ConfigurationManager.GetSection("telegramBotSettings/stickers/hello")
        //    as NameValueCollection;
        //_beerStickers =
        //    ConfigurationManager.GetSection("telegramBotSettings/stickers/beer")
        //    as NameValueCollection;
        //_foodStickers =
        //    ConfigurationManager.GetSection("telegramBotSettings/stickers/food")
        //    as NameValueCollection;
    }

    /// <summary>
    /// Create and start a bot of specified type.
    /// </summary>
    /// <param name="botType"></param>
    /// <param name="accessToken"></param>
    /// <returns></returns>
	public async Task InitializeBot(string botType, string accessToken)
	{
        ChatBotFactory botFactory = new ChatBotFactory();
        IChatBot bot =
            botFactory.GetChatBot(botType, accessToken) ??
            throw new NullReferenceException(
                $"Cannot instantiate a bot of specified type: {botType}");

        using CancellationTokenSource cts = new();
        await bot.TestConnectionAsync(cts);

        bot.MessageReceived += HandleReceivedMessage;
        bot.StartBot(cts);
    }

    private void HandleReceivedMessage(
        object sender, BotMessageReceivedEventArgs e)
    {
        BotMessage message = e.BotMessage;

        _logger.Debug("NEW MESSAGE RECEIVED");
        _logger.Debug($"Chat {message.ChatId} " +
            $"(title: {message.ChatTitle}) " +
            $"From {message.UserId} {message.UserNickname} " +
            $"/ {message.UserFirstname} {message.UserLastname} " +
            $"| Message Id: {message.MessageId}" +
            $"| Message Text: {message.Text} " +
            $"| Received sticker: {message.Sticker}");

        //if (false)
        //{
        //    Console.WriteLine($"Unknown chat {chatId}");
        //    return;
        //}

        //Random random = new Random();
        //if (messageText == "/healthcheck" || messageText.ToLower().Contains("привет"))
        //{
        //    await botClient.SendStickerAsync(
        //            chatId: chatId,
        //            sticker: _helloStickers.Get(random.Next(_helloStickers.Count)),
        //            cancellationToken: cancellationToken);
        //}
        //else if (Regex.IsMatch(messageText, @"^/birthday(\s([A_Za_zА_Яа_я]{1,}))?"))
        //{
        //    string cgn =
        //        BirthdayService.GetCongratulation(messageText.Substring(9));
        //    await botClient.SendTextMessageAsync(
        //        chatId: chatId,
        //        text: cgn,
        //        cancellationToken: cancellationToken);
        //}
        //else if (messageText == "/beer")
        //{
        //    await botClient.SendStickerAsync(
        //            chatId: chatId,
        //            sticker: _beerStickers.Get(random.Next(_beerStickers.Count)),
        //            cancellationToken: cancellationToken);
        //}
        //else if (messageText == "/kazantip")
        //{
        //    await botClient.SendPhotoAsync(
        //            chatId: chatId,
        //            photo: "https://images.glavred.info/2017_01/thumb_files/1200x0/1483352484-27546162.jpg",
        //            caption: "Ты чё, мужик? Подожди месяцок",
        //            parseMode: ParseMode.Html,
        //            cancellationToken: cancellationToken);
        //}
        //else if (messageText == "/events")
        //{
        //    string events = await CalendarEventService.GetTodaysEventsAsync();
        //    await botClient.SendTextMessageAsync(
        //        chatId: chatId,
        //        text: events,
        //        cancellationToken: cancellationToken);
        //}
        //else
        //{
        //    //if (random.Next(100) < 5)
        //    //{
        //    //    await botClient.SendTextMessageAsync(
        //    //        chatId: chatId,
        //    //        text: "Хватит постить кринж!)",
        //    //        cancellationToken: cancellationToken);
        //    //}
        //}
    }
}
