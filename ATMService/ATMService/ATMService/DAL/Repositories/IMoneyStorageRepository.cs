using ATMService.DAL.Entities;
using ATMService.DAL.Repositories.Base;
using System.Threading.Tasks;

namespace ATMService.DAL.Repositories
{

    /// <summary>
    /// Interface for money storage data handling
    /// </summary>
    public interface IMoneyStorageRepository : ICRUDRepository<MoneyStorage>
    {

        /// <summary>
        /// Calculate balance of ATM storage
        /// </summary>
        /// <returns>Balance</returns>
        Task<int> GetBalanceAsync();
    }
}
