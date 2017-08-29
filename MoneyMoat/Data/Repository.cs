﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MoneyModels;
using Z.EntityFramework.Plus;

namespace MoneyMoat
{
    public class Repository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : Entity
        where TContext : DbContext
    {
        public DbSet<TEntity> Datas { get; set; }

        protected readonly TContext m_context;

        public Repository(TContext context)
        {
            m_context = context;
            Datas = m_context.Set<TEntity>();
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
            return Find(1);
        }
        public TEntity Find(int id)
        {
            var data = Datas.Find(id);
            return data;
        }
        public TEntity Find(string key)
        {
            var data = Datas.Find(key);
            return data;
        }

        public bool Any(int id)
        {
            return Datas.Any(t=>t.GetId() == id);
        }
        public bool Any(string key)
        {
            return Datas.Any(t => t.GetKey() == key);
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var ret = await Datas.AnyAsync(predicate);
            return ret;
        }
        public List<TEntity> GetAll()
        {
            return Datas.ToList();
        }
        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken))
        {
            return await Datas.FirstOrDefaultAsync(predicate, token);
        }
        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken))
        {
            return Datas.Where(predicate);
        }
        public async Task<TEntity[]> WhereToArrayAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken))
        {
            return await Datas.Where(predicate).ToArrayAsync();
        }
        public async Task<List<TEntity>> WhereToListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken))
        {
            return await Datas.Where(predicate).ToListAsync();
        }
        public void Add(TEntity data)
        {
            var key = GetKey(data);
            data.TryUpdateTime();
            Datas.Add(data);
        }
        public void Update(TEntity data)
        {
            var key = GetKey(data);
            data.TryUpdateTime();
            Datas.Update(data);
        }
        public void Remove(TEntity data)
        {
            var key = GetKey(data);
            Datas.Remove(data);
        }
        public bool Remove(int id)
        {
            bool ret = false;
            var data =  Find(id);
            if (data != null)
            {
                ret = true;
                Remove(data);
            }
            return ret;
        }
        public bool RemoveRange(params TEntity[] datas)
        {
            bool ret = true;
            Datas.RemoveRange(datas);
            return ret;
        }
        public async Task<int> RemoveRangeAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Datas.Where(predicate).DeleteAsync();
        }
        public async Task SaveChangesAsync()
        {
            await m_context.SaveChangesAsync();
        }
    }
}
