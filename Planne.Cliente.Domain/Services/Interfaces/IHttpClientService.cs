

using Planne.Core.DTOs;

namespace Planne.Client.Domain.Services.Interfaces;

public interface IHttpClientService
{
    Task<IEnumerable<TItem>> GetFromJsonAsync<TItem, TResponse>(string endpoint, string baseUrl, string subscriptionKey)
        where TResponse : ExternalJsonDTO<TItem>
        where TItem : class;

    Task<IEnumerable<TItem>> GetFromJsonAsync<TItem>(string endpoint, string baseUrl, string subscriptionKey)
        where TItem : class;
}
