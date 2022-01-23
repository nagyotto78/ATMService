using ATMService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATMService.Controllers
{
    /// <summary>
    /// Money withdrawal handling
    /// </summary>
    [ApiController]
    [Route("api/withdrawal")]
    public class WithDrawalController : ControllerBase
    {

        private readonly ILogger<WithDrawalController> _logger;

        private readonly IATMService _service;

        public WithDrawalController(ILogger<WithDrawalController> logger,
                                    IATMService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Money withdrawal 
        /// </summary>
        /// <param name="data">Required money</param>
        /// <returns>Withdrawn money per denomination</returns>
        [HttpPost]
        public async Task<Dictionary<string, int>> WithDrawalAsync([FromBody] long data)
        {
            return await _service.WithDrawalAsync(data);
        }
    }
}
