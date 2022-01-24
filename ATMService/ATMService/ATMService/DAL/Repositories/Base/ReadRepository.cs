using ATMService.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ATMService.DAL.Repositories.Base
{
    public class ReadRepository<T> : IReadRepository<T> where T : class, IModelId
    {
        /// <summary>
        /// Database context
        /// </summary>
        internal readonly DbContext _context;

        /// <summary>
        /// Entities
        /// </summary>
        internal DbSet<T> _entities;

        public ReadRepository(ATMDbContext context)
        {
            _context = context;
            _entities = _context?.Set<T>();
        }

        public IQueryable<T> Read(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _entities as IQueryable<T>;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            includeProperties ??= "";
            foreach (string includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query;
        }

        public async Task<T> FindByIdAsync(int id, string includeProperties = "")
        {
            return await Read(m => m.Id == id, null, includeProperties).FirstOrDefaultAsync();
        }

    }
}
