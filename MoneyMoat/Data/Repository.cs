using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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

        private ConcurrentDictionary<string, TEntity> m_updateDb = new ConcurrentDictionary<string, TEntity>();
        private ConcurrentDictionary<string, TEntity> m_addDb = new ConcurrentDictionary<string, TEntity>();

        public Repository(TContext context, DbSet<TEntity> datas, ICacheClient cacheClient)
        {
            Datas = datas;
            m_cacheClient = cacheClient;
            m_context = context;

            new Task(Work).Start();
        }

        private void Work()
        {
            var typeName = typeof(TEntity).Name;

            while (true)
            {
                bool ret = false;
                lock (m_lock_me)
                {
                    if (m_addDb.Count > 0)
                    {
                        lock (m_addDb)
                        {
                            foreach (var item in m_addDb)
                            {
                                Datas.Add(item.Value);
                                Console.WriteLine("Lazy Add Db: {0}.{1}", typeName, item.Key);
                            }
                            m_addDb.Clear();
                        }
                        ret = true;
                    }
                    if (m_updateDb.Count > 0)
                    {
                        lock (m_updateDb)
                        {
                            foreach (var item in m_updateDb)
                            {
                                Datas.Update(item.Value);
                                Console.WriteLine("Lazy Update Db: {0}.{1}", typeName, item.Key);
                            }
                            m_updateDb.Clear();
                        }
                        ret = true;
                    }
                    if (ret)
                        m_context.SaveChanges();
                }
                Task.Delay(100);
            }
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

        object m_lock_me = new object();
        private ConcurrentDictionary<string, TEntity> m_findDb = new ConcurrentDictionary<string, TEntity>();
        public async Task<TEntity> Find(string key)
        {
            //var ret = await m_cacheClient.GetAsync<TEntity>(key);
            //if (ret.HasValue)
            //    return ret.Value;
            //else
            {
                var data = default(TEntity);
                if (!m_findDb.TryGetValue(key, out data))
                {
                    lock (m_lock_me)
                    {
                        data = Datas.Find(key);
                    }
                    m_findDb.AddOrUpdate(key, data, (tkey, oldValue) => data);
                }
                //if (data != null)
                //    await m_cacheClient.SetAsync(key, data);
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
            m_addDb.AddOrUpdate(key, data, (tkey, oldValue) => data);
            //Datas.Add(data);
            //await m_context.SaveChangesAsync();
            return ret;
        }
        public async Task<bool> AddNoCache(TEntity data)
        {
            data.TryUpdateTime();
            //Datas.Add(data);
            //await m_context.SaveChangesAsync();
            var key = GetKey(data);
            m_addDb.AddOrUpdate(key, data, (tkey, oldValue) => data);
            return true;
        }
        //public bool AddNoCacheNotSave(TEntity data)
        //{
        //    data.TryUpdateTime();
        //    Datas.Add(data);
        //    return true;
        //}
        //public async Task<bool> SaveChangesAsync()
        //{
        //    await m_context.SaveChangesAsync();
        //    return true;
        //}
        public async Task<bool> Update(TEntity data)
        {
            var key = GetKey(data);
            var ret = await m_cacheClient.SetAsync(key, data);
            data.TryUpdateTime();
            //Datas.Update(data);
            //await m_context.SaveChangesAsync();
            m_updateDb.AddOrUpdate(key, data, (tkey, oldValue) => data);
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
                ret = await Remove(data);
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
        public async Task<bool> RemoveRange(Expression<Func<TEntity, bool>> predicate)
        {
            var datas = Where(predicate).ToArray();
            return await RemoveRange(datas);
        }
    }
}
