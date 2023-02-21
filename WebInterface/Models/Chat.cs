using System;
using System.ComponentModel.DataAnnotations;

namespace WebInterface.Models;

public class Chat
{
	/// <summary>
	/// Increment ID.
	/// </summary>
	[Key]
	public long Id { get; set; }

    /// <summary>
    /// Messaging app identifier.
    /// </summary>
    [Display(Name = "App")]
    public string AppId { get; init; }

    /// <summary>
    /// Chat ID.
    /// </summary>
    [Display(Name = "Chat ID")]
    public long ChatId { get; init; }

	/// <summary>
	/// Chat Type.
	/// </summary>
	public string Type { get; init; }

    /// <summary>
    /// Chat Title (for group chats).
    /// </summary>
    [Display(Name = "Title (for group chats)")]
    public string? Title { get; init; }

    /// <summary>
    /// Chat Full Name (for private chats).
    /// </summary>
    [Display(Name = "Full Name (for private chats)")]
    public string? FullName { get; init; }

    /// <summary>
    /// Flag shows whether a chat is being listened or not.
    /// </summary>
    [Display(Name = "Being listened")]
    public bool IsBeingListened { get; set; }

    /// <summary>
    /// Flag shows whether a chat is authorized by bot to process messages.
    /// </summary>
    [Display(Name = "Authorized")]
    public bool IsAuthorized { get; set; }
}
