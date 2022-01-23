using ATMService.DAL.Entities;
using ATMService.DAL.Repositories.Base;

namespace ATMService.DAL.Repositories
{
    /// <summary>
    /// Available denomination datasource handling
    /// </summary>
    public class MoneyDenominationRepository : ReadRepository<MoneyDenomination>, IMoneyDenominationRepository
    {

        public MoneyDenominationRepository(ATMDbContext context) : base(context) { }

    }
}
