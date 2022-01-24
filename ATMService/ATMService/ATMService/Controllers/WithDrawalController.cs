using ATMService.Models;
using ATMService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
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
        public async Task<IActionResult> WithDrawalAsync([FromBody] int data)
        {
            IActionResult retVal;
            OperationResult<Dictionary<string, int>> result = await _service.WithDrawalAsync(data);
            string logMessage;

            switch (result?.ResultCode)
            {
                case Enums.ResultCode.Success:
                    _logger.LogInformation($"Withdrawed money: {JsonSerializer.Serialize(result.Result)}");
                    retVal = Ok(result.Result);
                    break;
                case Enums.ResultCode.WithDrawalInvalidNumber:
                    logMessage = $"ERROR-400.3 - The value must be divisible by 1000 and greater than zero. ({data})";
                    _logger.LogError(logMessage);
                    retVal = BadRequest(logMessage);
                    break;
                case Enums.ResultCode.WithDrawalNotPossible:
                    logMessage = $"ERROR-503.4 - Withdrawal is not possible ({data})";
                    _logger.LogError(logMessage);
                    retVal = StatusCode(503, logMessage);
                    break;
                default:
                    logMessage = "ERROR-400.5 - Invalid withdrawal process result.";
                    _logger.LogError(logMessage);
                    retVal = BadRequest(logMessage); //Invalid process result
                    break;
            }

            return retVal;
        }
    }
}
