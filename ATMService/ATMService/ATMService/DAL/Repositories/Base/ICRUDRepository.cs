using ATMService.DAL.Entities;
using System.Threading.Tasks;

namespace ATMService.DAL.Repositories.Base
{
    /// <summary>
    /// Interface for database CRUD operations 
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    public interface ICRUDRepository<T> : IReadRepository<T> where T : class, IModelId
    {

        /// <summary>
        /// Create entity
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="saveChanges">Save changes if required (default=true)</param>
        /// <returns></returns>
        Task CreateAsync(T model, bool saveChanges = true);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="saveChanges">Save changes if required (default=true)</param>
        /// <returns></returns>
        Task<T> UpdateAsync(T model, bool saveChanges = true);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="saveChanges">Save changes if required (default=true)</param>
        /// <returns></returns>
        Task DeleteAsync(T model, bool saveChanges = true);

        /// <summary>
        /// Delete all entity from datasource
        /// </summary>
        /// <param name="saveChanges">Save changes if required (default=true)</param>
        /// <returns></returns>
        Task DeleteAllAsync(bool saveChanges = true);
    }
}