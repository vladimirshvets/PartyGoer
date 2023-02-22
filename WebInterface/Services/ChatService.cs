using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using WebInterface.Data;
using WebInterface.Models;

namespace WebInterface.Services;

public class ChatService
{
    private readonly ApplicationDbContext _context;

    private readonly CacheService _cacheService;

    public ChatService(
		ApplicationDbContext context, IDistributedCache cache)
	{
        _context = context;
        _cacheService = new CacheService(cache);
	}

    public async Task<Chat?> GetChatAsync(string appId, long chatId)
    {
        // ToDo:
        // Store chat key as primary key?
        string chatKey = $"{appId}_{chatId}";
        Chat? chat = null;

        // Try loading chat info from cache.
        string? chatStringValue = await _cacheService.GetStringAsync(chatKey);

        if (chatStringValue != null)
        {
            chat = JsonSerializer.Deserialize<Chat>(chatStringValue);
        }
        else
        {
            // Try loading chat info from database.
            chat = _context.Chats.FirstOrDefault(x => x.ChatId == chatId);
            if (chat != null)
            {
                // Save chat info to cache.
                chatStringValue = JsonSerializer.Serialize(chat);
                await _cacheService.SetStringAsync(
                    chatKey,
                    chatStringValue,
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    });
            }
        }

        return chat;
    }
}
