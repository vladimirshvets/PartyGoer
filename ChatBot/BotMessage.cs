namespace PartyGoer.ChatBot;

/// <summary>
/// Representation of a message.
/// </summary>
public class BotMessage
{
    public long MessageId { get; set; }

    public long? UserId { get; set; }

    public string? UserNickname { get; set; }

    public string? UserFirstname { get; set; }

    public string? UserLastname { get; set; }

    public long ChatId { get; set; }

    public string? ChatTitle { get; set; }

    public DateTime Date { get; set; }

    public string? Text { get; set; }

    public string? Photo { get; set; }

    public string? Sticker { get; set; }

    public string? UserFullname
    {
        get
        {
            if (UserFirstname == null)
            {
                return UserLastname;
            }
            if (UserLastname == null)
            {
                return UserFirstname;
            }
            return $"{UserFirstname} {UserLastname}";
        }
    }

    public BotMessageType Type
    {
        get
        {
            if (this != null)
            {
                if (Text == null)
                {
                    if (Photo == null)
                    {
                        if (Sticker == null)
                        {
                            return BotMessageType.Unknown;
                        }
                        return BotMessageType.Sticker;
                    }
                    return BotMessageType.Photo;
                }
                return BotMessageType.Text;
            }
            return BotMessageType.Unknown;
        }
    }
}
