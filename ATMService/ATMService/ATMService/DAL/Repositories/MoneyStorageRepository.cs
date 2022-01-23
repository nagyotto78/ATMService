using ATMService.DAL.Entities;
using ATMService.DAL.Repositories.Base;

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
    }
}
