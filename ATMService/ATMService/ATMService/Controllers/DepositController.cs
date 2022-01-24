using ATMService.Models;
using ATMService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
        /// Money deposit 
        /// </summary>
        /// <param name="data">Money deposit per denomination</param>
        /// <returns>Money balance</returns>
        [HttpPost]
        public async Task<IActionResult> DepositAsync(Dictionary<string, int> data)
        {
            IActionResult retVal;
            OperationResult<int> result = await _service.DepositAsync(data);
            string logMessage;

            switch (result?.ResultCode)
            {
                case Enums.ResultCode.Success:
                    _logger.LogInformation($"ATM Balance: {result.Result}");
                    retVal = Ok(result.Result);
                    break;
                case Enums.ResultCode.DepositError:
                    logMessage = $"ERROR-400.1 - Invalid input data: {Environment.NewLine}{result.ErrorMessage}";
                    _logger.LogError(logMessage);
                    retVal = BadRequest(logMessage);
                    break;
                default:
                    logMessage = "ERROR-400.2 - Invalid dposit process result.";
                    _logger.LogError(logMessage);
                    retVal = BadRequest(logMessage); //Invalid process result
                    break;
            }

            return retVal;
        }
    }
}
