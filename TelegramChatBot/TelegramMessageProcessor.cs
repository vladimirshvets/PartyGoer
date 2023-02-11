using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PartyGoer.TelegramChatBot;

public class TelegramMessageProcessor
{
    private NameValueCollection _helloStickers;

    private NameValueCollection _beerStickers;

    private NameValueCollection _foodStickers;

    public TelegramMessageProcessor()
    {
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
            Console.WriteLine($"Received a sticker {message.Sticker.SetName} " +
                $"fileId = {message.Sticker.FileId}");
        }

        // Only process text messages.
        if (message.Text is not { } messageText)
        {
            return false;
        }

        Console.WriteLine($"Chat {chatId} | Received: {messageText}");

        return true;
    }
}
