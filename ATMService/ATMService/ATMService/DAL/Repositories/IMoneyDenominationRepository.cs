using ATMService.DAL.Entities;
using ATMService.DAL.Repositories.Base;

namespace ATMService.DAL.Repositories
{
    /// <summary>
    /// Interface for money denomination data handling
    /// </summary>
    public interface IMoneyDenominationRepository : IReadRepository<MoneyDenomination>
    {
    }
}
