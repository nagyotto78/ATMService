using ATMService.DAL.Entities;
using ATMService.DAL.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ATMService.DAL.Repositories
{
    /// <summary>
    /// Money storage datasource handling
    /// </summary>
    public class MoneyStorageRepository : CRUDRepository<MoneyStorage>, IMoneyStorageRepository
    {

        public MoneyStorageRepository(ATMDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Calculate balance of ATM storage
        /// </summary>
        /// <returns>Balance</returns>
        public async Task<int> GetBalanceAsync()
        {
            return await _entities.SumAsync(i => i.MoneyDenomination.Value * i.Count);
        }
    }
}
