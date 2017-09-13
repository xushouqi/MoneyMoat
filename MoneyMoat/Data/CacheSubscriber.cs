using System;
using System.Threading.Tasks;
using DotNetCore.CAP;
using StockModels;

namespace CommonServices.Caching
{
    public interface ISubscriberService
    {
        Task RemoveCache(InvalidateCache message);
    }

    public class CacheSubscriber : ICapSubscribe ,ISubscriberService
    {
        private readonly IServiceProvider _services;

        public CacheSubscriber(IServiceProvider services)
        {
            _services = services;
        }

        [CapSubscribe("cache.data.remove")]
        public async Task RemoveCache(InvalidateCache message)
        {
            switch (message.CacheId)
            {
                case "":
                    var cache_client = (ICacheClient<Stock>)_services.GetService(typeof(ICacheClient<Stock>));
                    await cache_client.OnRemoteCacheItemExpiredAsync(message);
                    break;
                default:
                    break;
            }
        }
    }
}
