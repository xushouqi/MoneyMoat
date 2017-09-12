using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonLibs;

namespace CommonServices.Caching
{
    public interface ICacheClient<T> : IDisposable where T: Entity
    {
        Task<int> RemoveAllAsync(IEnumerable<string> keys = null);
        Task<int> RemoveByPrefixAsync(string prefix);
        Task<T> GetAsync(string key);
        Task<IDictionary<string, T>> GetAllAsync(IEnumerable<string> keys);
        Task<bool> AddAsync(string key, T value, TimeSpan? expiresIn = null);
        Task<bool> SetAsync(string key, T value, TimeSpan? expiresIn = null);
        Task<int> SetAllAsync(IDictionary<string, T> values, TimeSpan? expiresIn = null);
        Task<bool> ReplaceAsync(string key, T value, TimeSpan? expiresIn = null);
        Task<bool> ExistsAsync(string key); 
        Task<TimeSpan?> GetExpirationAsync(string key);
        Task SetExpirationAsync(string key, TimeSpan expiresIn);
        //Task<double> SetIfHigherAsync(string key, double value, TimeSpan? expiresIn = null);
        //Task<double> SetIfLowerAsync(string key, double value, TimeSpan? expiresIn = null);
        Task<long> SetAddAsync(string key, IEnumerable<T> value, TimeSpan? expiresIn = null);
        Task<long> SetRemoveAsync(string key, IEnumerable<T> value, TimeSpan? expiresIn = null);
        Task<ICollection<T>> GetSetAsync(string key);

        Task OnRemoteCacheItemExpiredAsync(InvalidateCache message);
    }


    public class InvalidateCache
    {
        public string CacheId { get; set; }
        public string[] Keys { get; set; }
        public bool FlushAll { get; set; }
        public bool Expired { get; set; }
    }
}
