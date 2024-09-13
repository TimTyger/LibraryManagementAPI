using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi_Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T? GetById(int id);
        IQueryable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> expression);
        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        Task Remove(T entity);
        Task RemoveRange(IEnumerable<T> entities);

        Task<T> Update(T entity);

        Task<IEnumerable<T>> UpdateRange(IEnumerable<T> entities);

    }

    public interface ITransactionHandler
    {
        Task<IDbContextTransaction> BeginTransaction();

        Task CommitTransaction(IDbContextTransaction transaction);

        Task RollbackTransaction(IDbContextTransaction transaction);
    }
}
