using System;
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
        Task<TEntity> Find(int id);
        Task<TEntity> Find(string key);
        Task<bool> Any(int id);
        Task<bool> Any(string key);
        Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken));
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken));
        Task<TEntity[]> WhereToArray(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken));
        Task<List<TEntity>> WhereToList(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken));
        Task<bool> Add(TEntity data);
        //bool AddNoCacheNotSave(TEntity data);
        Task<bool> AddNoCache(TEntity data);
        //Task<bool> SaveChangesAsync();
        Task<bool> Update(TEntity data);
        Task<bool> Remove(TEntity data);
        Task<bool> Remove(int id);
        Task<bool> RemoveRange(params TEntity[] datas);
        Task<bool> RemoveRange(Expression<Func<TEntity, bool>> predicate);
    }
}
