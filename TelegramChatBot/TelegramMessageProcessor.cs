using System.Collections.Specialized;
using System.Configuration;
using System.Text.RegularExpressions;
using NLog;
using PartyGoer.Infotainment;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PartyGoer.TelegramChatBot;

public class TelegramMessageProcessor
{
    private NameValueCollection _helloStickers;

    private NameValueCollection _beerStickers;

    private NameValueCollection _foodStickers;

    /// <summary>
    /// Instance of logger.
    /// </summary>
    private Logger _logger;

    public TelegramMessageProcessor()
    {
        _logger = LogManager.GetCurrentClassLogger();

        // Set stickers.
        // ToDo:
        // Remove ConfigurationManager package.
        // Load a collection of stickers from database for specific chat.
        _helloStickers =
            ConfigurationManager.GetSection("telegramBotSettings/stickers/hello")
            as NameValueCollection;
        _beerStickers =
            ConfigurationManager.GetSection("telegramBotSettings/stickers/beer")
            as NameValueCollection;
        _foodStickers =
            ConfigurationManager.GetSection("telegramBotSettings/stickers/food")
            as NameValueCollection;
    }

    /// <summary>
    /// Process received messages.
    /// </summary>
    /// <param name="botClient">Bot client</param>
    /// <param name="update">Chat update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>true if the update has been processed successfully.</returns>
    public async Task<bool> ProcessAsync(ITelegramBotClient botClient,
		Update update, CancellationToken cancellationToken)
	{
        // Only process Message updates:
        // https://core.telegram.org/bots/api#message
        if (update.Message is not { } message)
        {
            return false;
        }

        var chatId = message.Chat.Id;
        //if (false)
        //{
        //    Console.WriteLine($"Unknown chat {chatId}");
        //    return;
        //}

        if (message.Type == MessageType.Sticker)
        {
            _logger.Debug($"Chat {chatId} " +
                $"From {message.From.Id} {message.From.Username} " +
                $"/ {message.From.FirstName} {message.From.LastName} " +
                $"| Message Id: {message.MessageId}" +
                $"| Received sticker: {message.Sticker.SetName}, " +
                $"fileId = {message.Sticker.FileId}");
        }

        // Only process text messages.
        if (message.Text is not { } messageText)
        {
            return false;
        }

        _logger.Debug($"Chat {chatId} " +
            $"From {message.From.Id} {message.From.Username} " +
            $"/ {message.From.FirstName} {message.From.LastName} " +
            $"| Message Id: {message.MessageId}" +
            $"| Received: {messageText}");

        Random random = new Random();
        if (messageText == "/healthcheck" || messageText.ToLower().Contains("привет"))
        {
            await botClient.SendStickerAsync(
                    chatId: chatId,
                    sticker: _helloStickers.Get(random.Next(_helloStickers.Count)),
                    cancellationToken: cancellationToken);
        }
        else if (Regex.IsMatch(messageText, @"^/birthday(\s([A_Za_zА_Яа_я]{1,}))?"))
        {
            string cgn = 
                BirthdayService.GetCongratulation(messageText.Substring(9));
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: cgn,
                cancellationToken: cancellationToken);
        }
        else if (messageText == "/beer")
        {
            await botClient.SendStickerAsync(
                    chatId: chatId,
                    sticker: _beerStickers.Get(random.Next(_beerStickers.Count)),
                    cancellationToken: cancellationToken);
        }
        else if (messageText == "/kazantip")
        {
            await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: "https://images.glavred.info/2017_01/thumb_files/1200x0/1483352484-27546162.jpg",
                    caption: "Ты чё, мужик? Подожди месяцок",
                    parseMode: ParseMode.Html,
                    cancellationToken: cancellationToken);
        }
        else if (messageText == "/events")
        {
            string events = await CalendarEventService.GetTodaysEventsAsync();
            await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: events,
                cancellationToken: cancellationToken);
        }
        else
        {
            if (random.Next(100) < 5)
            {
                await botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Хватит постить кринж!)",
                    cancellationToken: cancellationToken);
            }
        }

        return true;
    }
}
