using ATMService.DAL.Entities;
using System.Threading.Tasks;

namespace ATMService.DAL.Repositories.Base
{
    public class CRUDRepository<T> : ReadRepository<T>, ICRUDRepository<T> where T : class, IModelId
    {

        public CRUDRepository(ATMDbContext context) : base(context)
        {
        }

        public async Task CreateAsync(T model, bool saveChanges = true)
        {
            _entities?.Add(model);
            if (saveChanges)
            {
                await SaveChangesAsync();
            }
        }

        public async Task<T> UpdateAsync(T model, bool saveChanges = true)
        {
            _entities?.Update(model);
            if (saveChanges)
            {
                await SaveChangesAsync();
            }
            return model;
        }

        public async Task DeleteAsync(T model, bool saveChanges = true)
        {
            _entities?.Remove(model);
            if (saveChanges)
            {
                await SaveChangesAsync();
            }
        }

        public async Task  DeleteAllAsync(bool saveChanges = true)
        {
            _entities?.RemoveRange(_entities);
            if (saveChanges)
            {
                await SaveChangesAsync();
            }
        }

        /// <summary>
        /// Save database changes
        /// This method can execute any required operation before or after saving
        /// </summary>
        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    }
}
