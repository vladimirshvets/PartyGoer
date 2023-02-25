using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using NLog;
using PartyGoer.ChatBot;
using WebInterface.Data;
using WebInterface.Models;
using WebInterface.Repositories;

namespace WebInterface.Services;

public class ChatBotProcessor
{
    //private NameValueCollection _helloStickers;

    //private NameValueCollection _beerStickers;

    //private NameValueCollection _foodStickers;

    private readonly ChatRepository _chatRepository;

    /// <summary>
    /// Instance of logger.
    /// </summary>
    private Logger _logger;

    public ChatBotProcessor(ChatRepository chatRepository)
    {
        //var context = serviceProvider.GetService<ApplicationDbContext>();
        //var cache = serviceProvider.GetService<AppCache>();
        //_chatRepository = new ChatRepository(context, cache);
        _chatRepository = chatRepository;
        _logger = LogManager.GetCurrentClassLogger();

        // Set stickers.
        // ToDo:
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
    /// Handle received message.
    /// </summary>
    /// <param name="sender">Chat bot instance</param>
    /// <param name="e">Event arguments</param>
    public void HandleReceivedMessage(
        IChatBot sender, BotMessageReceivedEventArgs e)
    {
        BotMessage message = e.BotMessage;

        _logger.Debug("HANDLER: NEW MESSAGE RECEIVED");
        //_logger.Debug($"Chat {message.ChatId} " +
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
