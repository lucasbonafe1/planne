using Microsoft.Extensions.Caching.Memory;
using Planne.Client.Domain.Services.Interfaces;
using Planne.Core.DTOs;
using System.Text.Json;

namespace Planne.Client.Domain.Services;

public class HttpClientService : IHttpClientService
{
    private readonly IMemoryCache _memoryCache;
    private readonly double _cacheSlidingExpirationSeconds = 600;
    private readonly double _cacheAbsoluteExpirationRelativeToNowMinutes = 120;

    public HttpClientService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<IEnumerable<TItem>> GetFromJsonAsync<TItem, TResponse>(string endpoint, string baseUrl, string subscriptionKey)
        where TResponse : ExternalJsonDTO<TItem>
        where TItem : class
    {
        try
        {
            string cacheKey = $"{baseUrl}/{endpoint}";

            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<TItem>? cachedValue))
            {
                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(baseUrl)
                };

                HttpResponseMessage response = await httpClient.GetAsync(string.Empty);

                if (!response.IsSuccessStatusCode)
                    return null!;

                string content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<TResponse>(content);

                cachedValue = result?.items;

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(_cacheSlidingExpirationSeconds),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheAbsoluteExpirationRelativeToNowMinutes),
                    Size = 1
                };

                _memoryCache.Set(cacheKey, cachedValue, cacheOptions);
            }

            return cachedValue!;
        }
        catch (Exception ex) { throw new Exception(ex.Message); }
    }
    public async Task<IEnumerable<TItem>> GetFromJsonAsync<TItem>(string endpoint, string baseUrl, string subscriptionKey)
        where TItem : class
    {
        try
        {
            string cacheKey = $"{baseUrl}/{endpoint}";

            if (!_memoryCache.TryGetValue(cacheKey, out IEnumerable<TItem>? cachedValue))
            {
                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(baseUrl)
                };

                HttpResponseMessage response = await httpClient.GetAsync(string.Empty);

                if (!response.IsSuccessStatusCode)
                    return null!;

                string content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ExternalJsonDTO<TItem>>(content);

                cachedValue = result!.items;

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(_cacheSlidingExpirationSeconds),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheAbsoluteExpirationRelativeToNowMinutes),
                    Size = 1
                };

                _memoryCache.Set(cacheKey, cachedValue, cacheOptions);
            }

            return cachedValue!;
        }
        catch (Exception ex) { throw new Exception(ex.Message); }
    }
}
