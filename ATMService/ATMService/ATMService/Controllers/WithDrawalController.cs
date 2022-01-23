using ATMService.Models;
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
        public async Task<IActionResult> WithDrawalAsync([FromBody] long data)
        {
            IActionResult retVal = null;
            OperationResult<Dictionary<string, int>> result = await _service.WithDrawalAsync(data);
            if (result != null)
            {
                switch (result.ResultCode)
                {
                    case Enums.ResultCode.Success:
                        retVal = Ok(result.Result);
                        break;
                    case Enums.ResultCode.WithDrawalInvalidNumber:
                        retVal = BadRequest($"The value must be divisible by 1000. ({data})");
                        break;
                    case Enums.ResultCode.WithDrawalCanNotServed:
                        retVal = StatusCode(503, $"Invalid denominations counts: {result.ErrorMessage}");
                        break;
                }
            }
            retVal ??= BadRequest("Invalid operation result."); //Invalid process result

            return retVal;
        }
    }
}
