//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;
//using CommonLibs;
//using DotNetCore.CAP;

//namespace CommonServices.Caching
//{
//    //public interface IHybridCacheClient : ICacheClient { }

//    public class HybridCacheClient<T> : ICacheClient<T> where T:Entity
//    {
//        private readonly string _cacheId = Guid.NewGuid().ToString("N");
//        protected readonly ICacheClient<T> _distributedCache;
//        private readonly InMemoryCacheClient<T> _localCache;
//        //protected readonly IMessageBus _messageBus;
//        private readonly ICapPublisher _publisher;
//        private readonly ILogger _logger;
//        private long _localCacheHits;
//        private long _invalidateCacheCalls;
//        //private readonly string m_capName;

//        public HybridCacheClient(ICacheClient<T> distributedCacheClient, ICapPublisher publisher, ILoggerFactory loggerFactory = null)
//        {
//            _logger = loggerFactory.CreateLogger<HybridCacheClient<T>>();
//            _distributedCache = distributedCacheClient;
//            _publisher = publisher;
//            //_messageBus.SubscribeAsync<InvalidateCache>(OnRemoteCacheItemExpiredAsync).GetAwaiter().GetResult();
//            _localCache = new InMemoryCacheClient<T>(new InMemoryCacheClientOptions { LoggerFactory = loggerFactory }) { MaxItems = 100 };
//            _localCache.ItemExpired.AddHandler(OnLocalCacheItemExpiredAsync);

//            //m_capName = "cache.data.update";
//        }

//        public InMemoryCacheClient<T> LocalCache => _localCache;
//        public long LocalCacheHits => _localCacheHits;
//        public long InvalidateCacheCalls => _invalidateCacheCalls;

//        public int LocalCacheSize
//        {
//            get { return _localCache.MaxItems ?? -1; }
//            set { _localCache.MaxItems = value; }
//        }

//        private async Task OnLocalCacheItemExpiredAsync(object sender, EntityExpiredEventArgs<T> args)
//        {
//            if (!args.SendNotification)
//                return;

//            _logger.LogTrace("Local cache expired event: key={0}", args.Key);
//            await _publisher.PublishAsync("cache.data.remove", new InvalidateCache { CacheId = _cacheId, Keys = new[] { args.Key }, Expired = true });
//            //await _messageBus.PublishAsync(new InvalidateCache { CacheId = _cacheId, Keys = new[] { args.Key }, Expired = true }).AnyContext();
//        }

//        public async Task OnRemoteCacheItemExpiredAsync(InvalidateCache message)
//        {
//            if (!String.IsNullOrEmpty(message.CacheId) && String.Equals(_cacheId, message.CacheId))
//                return;

//            _logger.LogTrace(String.Format("Invalidating local cache from remote: id={0} expired={1} keys={2}", message.CacheId, message.Expired, String.Join(",", message.Keys ?? new string[] { })));
//            Interlocked.Increment(ref _invalidateCacheCalls);
//            if (message.FlushAll)
//            {
//                await _localCache.RemoveAllAsync().AnyContext();
//                _logger.LogTrace("Fushed local cache");
//            }
//            else if (message.Keys != null && message.Keys.Length > 0)
//            {
//                var keysToRemove = new List<string>(message.Keys.Length);
//                foreach (string key in message.Keys)
//                {
//                    if (message.Expired)
//                        await _localCache.RemoveExpiredKeyAsync(key, false).AnyContext();
//                    else if (key.EndsWith("*"))
//                        await _localCache.RemoveByPrefixAsync(key.Substring(0, key.Length - 1)).AnyContext();
//                    else
//                        keysToRemove.Add(key);
//                }

//                int results = await _localCache.RemoveAllAsync(keysToRemove).AnyContext();
//                _logger.LogTrace("Removed {0} keys from local cache", results);
//            }
//            else
//            {
//                _logger.LogWarning("Unknown invalidate cache message");
//            }
//        }

//        public async Task<int> RemoveAllAsync(IEnumerable<string> keys = null)
//        {
//            bool flushAll = keys == null || !keys.Any();
//            await _publisher.PublishAsync("cache.data.remove", new InvalidateCache { CacheId = _cacheId, FlushAll = flushAll, Keys = keys?.ToArray() });
//            //await _messageBus.PublishAsync(new InvalidateCache { CacheId = _cacheId, FlushAll = flushAll, Keys = keys?.ToArray() }).AnyContext();
//            await _localCache.RemoveAllAsync(keys).AnyContext();
//            return await _distributedCache.RemoveAllAsync(keys).AnyContext();
//        }

//        public async Task<int> RemoveByPrefixAsync(string prefix)
//        {
//            await _publisher.PublishAsync("cache.data.remove", new InvalidateCache { CacheId = _cacheId, Keys = new[] { prefix + "*" } });
//            //await _messageBus.PublishAsync(new InvalidateCache { CacheId = _cacheId, Keys = new[] { prefix + "*" } }).AnyContext();
//            await _localCache.RemoveByPrefixAsync(prefix).AnyContext();
//            return await _distributedCache.RemoveByPrefixAsync(prefix).AnyContext();
//        }

//        public async Task<CacheValue<T>> GetAsync(string key)
//        {
//            var cacheValue = await _localCache.GetAsync<T>(key).AnyContext();
//            if (cacheValue.HasValue)
//            {
//                _logger.LogTrace("Local cache hit: {0}", key);
//                Interlocked.Increment(ref _localCacheHits);
//                return cacheValue;
//            }

//            _logger.LogTrace("Local cache miss: {0}", key);
//            cacheValue = await _distributedCache.GetAsync(key).AnyContext();
//            if (cacheValue.HasValue)
//            {
//                var expiration = await _distributedCache.GetExpirationAsync(key).AnyContext();
//                _logger.LogTrace("Setting Local cache key: {0} with expiration: {1}", key, expiration);

//                await _localCache.SetAsync(key, cacheValue.Value, expiration).AnyContext();
//                return cacheValue;
//            }

//            return cacheValue.HasValue ? cacheValue : CacheValue<T>.NoValue;
//        }

//        public Task<IDictionary<string, T>> GetAllAsync(IEnumerable<string> keys)
//        {
//            return _distributedCache.GetAllAsync(keys);
//        }

//        public async Task<bool> AddAsync(string key, T value, TimeSpan? expiresIn = null)
//        {
//            _logger.LogTrace("Adding key \"{0}\" to local cache with expiration: {1}", key, expiresIn);
//            bool added = await _distributedCache.AddAsync(key, value, expiresIn).AnyContext();
//            if (added)
//                await _localCache.SetAsync(key, value, expiresIn).AnyContext();

//            return added;
//        }

//        public async Task<bool> SetAsync(string key, T value, TimeSpan? expiresIn = null)
//        {
//            _logger.LogTrace("Setting key \"{0}\" to local cache with expiration: {1}", key, expiresIn);
//            await _publisher.PublishAsync("cache.data.remove", new InvalidateCache { CacheId = _cacheId, Keys = new[] { key } });
//            //await _messageBus.PublishAsync(new InvalidateCache { CacheId = _cacheId, Keys = new[] { key } }).AnyContext();
//            await _localCache.SetAsync(key, value, expiresIn).AnyContext();

//            return await _distributedCache.SetAsync(key, value, expiresIn).AnyContext();
//        }

//        public async Task<int> SetAllAsync(IDictionary<string, T> values, TimeSpan? expiresIn = null)
//        {
//            if (values == null || values.Count == 0)
//                return 0;

//            _logger.LogTrace("Adding keys \"{0}\" to local cache with expiration: {1}", values.Keys, expiresIn);
//            await _publisher.PublishAsync("cache.data.removeall", new InvalidateCache { CacheId = _cacheId, Keys = values.Keys.ToArray() });
//            //await _messageBus.PublishAsync(new InvalidateCache { CacheId = _cacheId, Keys = values.Keys.ToArray() }).AnyContext();
//            await _localCache.SetAllAsync(values, expiresIn).AnyContext();
//            return await _distributedCache.SetAllAsync(values, expiresIn).AnyContext();
//        }

//        public async Task<bool> ReplaceAsync(string key, T value, TimeSpan? expiresIn = null)
//        {
//            await _publisher.PublishAsync("cache.data.remove", new InvalidateCache { CacheId = _cacheId, Keys = new[] { key } });
//            //await _messageBus.PublishAsync(new InvalidateCache { CacheId = _cacheId, Keys = new[] { key } }).AnyContext();
//            await _localCache.ReplaceAsync(key, value, expiresIn).AnyContext();
//            return await _distributedCache.ReplaceAsync(key, value, expiresIn).AnyContext();
//        }
        
//        public Task<bool> ExistsAsync(string key)
//        {
//            return _distributedCache.ExistsAsync(key);
//        }

//        public Task<TimeSpan?> GetExpirationAsync(string key)
//        {
//            return _distributedCache.GetExpirationAsync(key);
//        }

//        public async Task SetExpirationAsync(string key, TimeSpan expiresIn)
//        {
//            await _localCache.SetExpirationAsync(key, expiresIn).AnyContext();
//            await _distributedCache.SetExpirationAsync(key, expiresIn).AnyContext();
//            await _publisher.PublishAsync("cache.data.remove", new InvalidateCache { CacheId = _cacheId, Keys = new[] { key } });
//            //await _messageBus.PublishAsync(new InvalidateCache { CacheId = _cacheId, Keys = new[] { key } }).AnyContext();
//        }
        
//        public async Task<long> SetAddAsync(string key, IEnumerable<T> values, TimeSpan? expiresIn = null)
//        {
//            await _publisher.PublishAsync("cache.data.remove", new InvalidateCache { CacheId = _cacheId, Keys = new[] { key } });
//            //await _messageBus.PublishAsync(new InvalidateCache { CacheId = _cacheId, Keys = new[] { key } }).AnyContext();
//            await _localCache.SetAddAsync(key, values, expiresIn).AnyContext();
//            return await _distributedCache.SetAddAsync(key, values, expiresIn).AnyContext();
//        }

//        public async Task<long> SetRemoveAsync(string key, IEnumerable<T> values, TimeSpan? expiresIn = null)
//        {
//            await _publisher.PublishAsync("cache.data.remove", new InvalidateCache { CacheId = _cacheId, Keys = new[] { key } });
//            //await _messageBus.PublishAsync(new InvalidateCache { CacheId = _cacheId, Keys = new[] { key } }).AnyContext();
//            await _localCache.SetRemoveAsync(key, values, expiresIn).AnyContext();
//            return await _distributedCache.SetRemoveAsync(key, values, expiresIn).AnyContext();
//        }

//        public async Task<ICollection<T>> GetSetAsync(string key)
//        {
//            var cacheValue = await _localCache.GetSetAsync(key).AnyContext();
//            if (cacheValue.HasValue)
//            {
//                _logger.LogTrace("Local cache hit: {0}", key);
//                Interlocked.Increment(ref _localCacheHits);
//                return cacheValue;
//            }

//            _logger.LogTrace("Local cache miss: {0}", key);
//            cacheValue = await _distributedCache.GetSetAsync(key).AnyContext();
//            if (cacheValue.HasValue)
//            {
//                var expiration = await _distributedCache.GetExpirationAsync(key).AnyContext();
//                _logger.LogTrace("Setting Local cache key: {0} with expiration: {1}", key, expiration);

//                await _localCache.SetAddAsync(key, cacheValue.Value, expiration).AnyContext();
//                return cacheValue;
//            }

//            return cacheValue.HasValue ? cacheValue : null;
//        }

//        public virtual void Dispose()
//        {
//            _localCache.ItemExpired.RemoveHandler(OnLocalCacheItemExpiredAsync);
//            _localCache.Dispose();

//            // TODO: unsubscribe handler from messagebus.
//        }
//    }
//}
