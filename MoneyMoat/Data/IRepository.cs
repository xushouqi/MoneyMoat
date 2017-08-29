﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyMoat
{
    public interface IRepository<TEntity>
    {
        List<TEntity> GetAll();
        TEntity GetDefault();
        TEntity Find(int id);
        TEntity Find(string key);
        bool Any(int id);
        bool Any(string key);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken));
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken));
        Task<TEntity[]> WhereToArrayAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken));
        Task<List<TEntity>> WhereToListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken));
        void Add(TEntity data);
        //bool AddNoCacheNotSave(TEntity data);
        //bool AddNoCache(TEntity data);
        //Task<bool> SaveChangesAsync();
        void Update(TEntity data);
        void Remove(TEntity data);
        bool Remove(int id);
        bool RemoveRange(params TEntity[] datas);
        Task<int> RemoveRangeAsync(Expression<Func<TEntity, bool>> predicate);
        Task SaveChangesAsync();
    }
}
