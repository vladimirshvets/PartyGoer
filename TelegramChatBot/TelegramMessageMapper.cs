using PartyGoer.ChatBot;
using Telegram.Bot.Types;

namespace PartyGoer.TelegramChatBot;

/// <summary>
/// Telegram message mapper.
/// </summary>
public class TelegramMessageMapper : IMessageMapper<Message>
{
    public BotMessage Map(Message original)
    {
        return new BotMessage
        {
            MessageId = original.MessageId,
            UserId = original.From?.Id,
            UserNickname = original.From?.Username,
            UserFirstname = original.From?.FirstName,
            UserLastname = original.From?.LastName,
            ChatId = original.Chat.Id,
            ChatTitle = original.Chat.Title,
            Date = original.Date,
            Text = original.Text,

            // ToDo: get necessary data, update types if required.
            Photo = original.Photo?.ToString(),
            Sticker = original.Sticker?.FileId
        };
    }
}
