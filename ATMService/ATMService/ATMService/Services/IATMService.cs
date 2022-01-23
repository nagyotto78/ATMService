using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATMService.Services
{
    public interface IATMService
    {
        Task<Dictionary<string, int>> WithDrawalAsync(long data);

        Task<long> DepositAsync(Dictionary<string, int> data);

    }
}
