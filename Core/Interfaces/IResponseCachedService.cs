using System;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICachedResponseService
    {
        Task CachedResponseAsync(string cachedKey, object response, TimeSpan timeToLive);
        Task<string> GetCachedResponseAsync(string cachedKey);
    }
}