using ATMService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ATMService.Controllers
{

    /// <summary>
    /// Money deposit handling
    /// </summary>
    [ApiController]
    [Route("api/deposit")]
    public class DepositController : ControllerBase
    {

        private readonly ILogger<DepositController> _logger;

        private readonly IATMService _service;

        public DepositController(ILogger<DepositController> logger,
                                 IATMService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Money withdrawal 
        /// </summary>
        /// <param name="data">Money deposit per denomination</param>
        /// <returns>Money balance</returns>
        [HttpPost]
        public async Task<long> DepositAsync(Dictionary<string, int> data)
        {
            return await _service.DepositAsync(data);
        }
    }
}
