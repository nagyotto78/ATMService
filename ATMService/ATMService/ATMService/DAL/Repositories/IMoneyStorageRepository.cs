using ATMService.DAL.Entities;
using ATMService.DAL.Repositories.Base;

namespace ATMService.DAL.Repositories
{

    /// <summary>
    /// Interface for money storage data handling
    /// </summary>
    public interface IMoneyStorageRepository : ICRUDRepository<MoneyStorage>
    {
    }
}
