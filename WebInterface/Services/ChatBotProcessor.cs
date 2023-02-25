using PartyGoer.ChatBot;
using WebInterface.Models;
using WebInterface.Repositories;

namespace WebInterface.Services;

public class ChatBotProcessor
{
    //private NameValueCollection _helloStickers;

    //private NameValueCollection _beerStickers;

    //private NameValueCollection _foodStickers;

    /// <summary>
    /// Instance of logger.
    /// </summary>
    private ILogger<ChatBotProcessor> _logger;

    public ChatBotProcessor(ILogger<ChatBotProcessor> logger)
    {
        _logger = logger;

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

    public void ProcessMessage(BotMessage message)
    {
        _logger.LogInformation("Parsing received message...");

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
