using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetCore.CAP;
using CommonLibs;

namespace CommonServices.Caching
{
    public interface ISubscriberService
    {
        Task RemoveCache(InvalidateCache message);
        //Task RemoveCacheByPrefix(string prefix);
        //Task RemoveCacheAll(string[] keys);
    }

    public class CacheSubscriberService<T> : ICapSubscribe ,ISubscriberService where T:Entity
    {
        private readonly ICacheClient<T> m_cache_client;
        public CacheSubscriberService(ICacheClient<T> cacheClient)
        {
            m_cache_client = cacheClient;
        }

        [CapSubscribe("cache.data.remove")]
        public async Task RemoveCache(InvalidateCache message)
        {
            await m_cache_client.OnRemoteCacheItemExpiredAsync(message);
        }

        //[CapSubscribe("cache.data.removeall")]
        //public async Task RemoveCacheByPrefix(string prefix)
        //{
        //    await m_cache_client.RemoveByPrefixAsync(prefix);
        //}
        //[CapSubscribe("cache.data.removeall")]
        //public async Task RemoveCacheAll(string[] keys)
        //{
        //    await m_cache_client.RemoveAllAsync(keys);
        //}
    }
}
