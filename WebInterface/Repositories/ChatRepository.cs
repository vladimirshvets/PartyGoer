using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using WebInterface.Data;
using WebInterface.Models;

namespace WebInterface.Repositories;

public class ChatRepository
{
    /// <summary>
    /// Database context.
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Cache context.
    /// </summary>
    private readonly AppCache _cache;

    /// <summary>
    /// Instance of logger.
    /// </summary>
    private readonly ILogger<ChatRepository> _logger;

    public ChatRepository(
        ApplicationDbContext context,
        AppCache cache,
        ILogger<ChatRepository> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Chat?> GetChatAsync(string appId, long chatId)
    {
        // ToDo:
        // Store chat key as primary key?
        string chatKey = $"{appId}_{chatId}";

        Chat? chat = await GetChatInfoFromCacheAsync(chatKey);
        if (chat == null)
        {
            // Try loading chat info from database.
            // ToDo:
            // Try FindAsync instead.
            chat = _context.Chats.FirstOrDefault(x => x.ChatId == chatId);
            if (chat != null)
            {
                // Save chat info to cache.
                await SaveChatInfoToCacheAsync(chat);
            }
        }

        return chat;
    }

    public async Task<Chat?> SaveChatAsync(Chat chat)
    {
        _context.Chats.Add(chat);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return null;
        }
        await SaveChatInfoToCacheAsync(chat);
        return chat;
    }

    private async Task<Chat?> GetChatInfoFromCacheAsync(string chatKey)
    {
        string? chatStringValue = await _cache.GetStringAsync(chatKey);

        if (chatStringValue != null)
        {
            return JsonSerializer.Deserialize<Chat>(chatStringValue);
        }
        return null;
    }

    private async Task SaveChatInfoToCacheAsync(Chat chat)
    {
        // ToDo:
        // Store chat key as primary key?
        string chatKey = $"{chat.AppId}_{chat.ChatId}";
        string chatStringValue = JsonSerializer.Serialize(chat);

        await _cache.SetStringAsync(
            chatKey,
            chatStringValue,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
    }
}
