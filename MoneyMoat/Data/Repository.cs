using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyModels;
using Foundatio.Caching;

namespace MoneyMoat
{
    public class Repository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : Entity
        where TContext : DbContext
    {
        public DbSet<TEntity> Datas { get; set; }

        protected readonly ICacheClient m_cacheClient;
        protected readonly TContext m_context;

        public Repository(TContext context, DbSet<TEntity> datas, ICacheClient cacheClient)
        {
            Datas = datas;
            m_cacheClient = cacheClient;
            m_context = context;
        }
        private string GetKey(int id)
        {
            return string.Concat(typeof(TEntity).Name, id);
        }
        private string GetKey(TEntity data)
        {
            return string.Concat(typeof(TEntity).Name, data.GetId());
        }

        public TEntity GetDefault()
        {
            return Datas.Find(1);
        }
        public async Task<TEntity> Find(int id)
        {
            var key = GetKey(id);
            var ret = await m_cacheClient.GetAsync<TEntity>(key);
            if (ret.HasValue)
                return ret.Value;
            else
            {
                var data = Datas.Find(id);
                if (data != null)
                    await m_cacheClient.SetAsync(key, data);
                return data;
            }
        }
        public async Task<TEntity> Find(string key)
        {
            var ret = await m_cacheClient.GetAsync<TEntity>(key);
            if (ret.HasValue)
                return ret.Value;
            else
            {
                var data = Datas.Find(key);
                if (data != null)
                    await m_cacheClient.SetAsync(key, data);
                return data;
            }
        }
        public async Task<bool> Any(int id)
        {
            var key = GetKey(id);
            var ret = await m_cacheClient.ExistsAsync(key);
            if (!ret)
            {
                var data = await Find(id);
                ret = data != null;
            }
            return ret;
        }
        public async Task<bool> Any(string key)
        {
            var ret = await m_cacheClient.ExistsAsync(key);
            if (!ret)
            {
                var data = await Find(key);
                ret = data != null;
            }
            return ret;
        }
        public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
        {
            var ret = await Datas.AnyAsync(predicate);
            return ret;
        }
        public List<TEntity> GetAll()
        {
            return Datas.ToList();
        }
        public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken))
        {
            return await Datas.FirstOrDefaultAsync(predicate, token);
        }
        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken))
        {
            return Datas.Where(predicate);
        }
        public async Task<TEntity[]> WhereToArray(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken))
        {
            return await Datas.Where(predicate).ToArrayAsync();
        }
        public async Task<List<TEntity>> WhereToList(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken))
        {
            return await Datas.Where(predicate).ToListAsync();
        }
        public async Task<bool> Add(TEntity data)
        {
            var key = GetKey(data);
            data.TryUpdateTime();
            var ret = await m_cacheClient.SetAsync(key, data);
            Datas.Add(data);
            await m_context.SaveChangesAsync();
            return ret;
        }
        public bool AddNoCacheNotSave(TEntity data)
        {
            data.TryUpdateTime();
            Datas.Add(data);
            return true;
        }
        public async Task<bool> SaveChangesAsync()
        {
            await m_context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddNoCache(TEntity data)
        {
            data.TryUpdateTime();
            Datas.Add(data);
            await m_context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Update(TEntity data)
        {
            var key = GetKey(data);
            var ret = await m_cacheClient.SetAsync(key, data);
            data.TryUpdateTime();
            Datas.Update(data);
            await m_context.SaveChangesAsync();
            return ret;
        }
        public async Task<bool> Remove(TEntity data)
        {
            bool ret = true;
            var key = GetKey(data);
            await m_cacheClient.RemoveAsync(key);
            Datas.Remove(data);
            await m_context.SaveChangesAsync();
            return ret;
        }
        public async Task<bool> Remove(int id)
        {
            bool ret = false;
            var data = await Find(id);
            if (data != null)
            {
                ret = true;
                var key = GetKey(id);
                await m_cacheClient.RemoveAsync(key);
                Datas.Remove(data);
                await m_context.SaveChangesAsync();
            }
            return ret;
        }
        public async Task<bool> RemoveRange(params TEntity[] datas)
        {
            bool ret = true;
            List<string> keys = new List<string>();
            for (int i = 0; i < datas.Length; i++)
            {
                var key = GetKey(datas[i].GetId());
                keys.Add(key);
            }
            await m_cacheClient.RemoveAllAsync(keys);

            Datas.RemoveRange(datas);
            await m_context.SaveChangesAsync();
            return ret;
        }
    }
}
