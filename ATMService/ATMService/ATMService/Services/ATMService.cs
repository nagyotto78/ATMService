using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATMService.Services
{
    public class ATMService : IATMService
    {
        public async Task<long> DepositAsync(Dictionary<string, int> data)
        {
            long retVal = 0;

            //TODO: Business logic

            return retVal;
        }

        public async Task<Dictionary<string, int>> WithDrawalAsync(long data)
        {
            Dictionary<string, int> retVal = new Dictionary<string, int>();

            //TODO: Business logic

            return retVal;
        }
    }
}
