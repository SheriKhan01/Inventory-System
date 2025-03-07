using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.IServices
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
        );

        Task<T> GetByIdAsync(Guid id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        Task AddAsync(T entity);
        void Update(T entity);
        Task Delete(T entity);
    }
}
