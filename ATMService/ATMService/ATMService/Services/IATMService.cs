using ATMService.Models;
using System.Threading.Tasks;

namespace ATMService.Services
{

    /// <summary>
    /// ATM service business logic
    /// </summary>
    public interface IATMService
    {

        /// <summary>
        /// Money withdrawal
        /// </summary>
        /// <param name="data">Required money</param>
        /// <returns>Money denominations and counts</returns>
        Task<OperationResult<Denominations>> WithDrawalAsync(int data);

        /// <summary>
        /// Money deposit
        /// </summary>
        /// <param name="data">Money denominations and counts</param>
        /// <returns>ATM balance</returns>
        Task<OperationResult<int>> DepositAsync(Denominations data);

    }
}
