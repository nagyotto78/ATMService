using ATMService.DAL.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ATMService.DAL.Repositories.Base
{
    /// <summary>
    /// Interface for database reading operations
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    public interface IReadRepository<T> where T : class, IModelId
    {
        /// <summary>
        /// Datasource query
        /// </summary>
        /// <param name="filter">Filter expression</param>
        /// <param name="orderBy">Order by expression</param>
        /// <param name="includeProperties">Other navigation properties</param>
        /// <returns>Query</returns>
        IQueryable<T> Read(Expression<Func<T, bool>> filter = null,
                           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                           string includeProperties = "");

        /// <summary>
        /// Find data by id
        /// </summary>
        /// <param name="id">Data primary key</param>
        /// <returns>Founded entity or null</returns>
        Task<T> FindByIdAsync(int id, 
                              string includeProperties = "");

    }
}
