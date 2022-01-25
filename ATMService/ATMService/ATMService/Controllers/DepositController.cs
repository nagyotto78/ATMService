using ATMService.Models;
using ATMService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// Money deposit. Available denominations: 1000, 2000, 5000, 10000, 20000
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/deposit
        ///             
        ///     {
        ///         "1000" : 5,
        ///         "2000" : 2,
        ///         "5000" : 1
        ///         "10000" : 2
        ///     }
        ///     
        /// </remarks>
        /// <param name="data">Money deposit per denomination</param>
        /// <response code="200">ATM balance</response>
        /// <response code="400">If the request data contains not available denominations</response> 
        /// <returns example="34000">Money balance</returns>
        [HttpPost]
        [Produces("application/json")]
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> DepositAsync([Required] Denominations data)
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
                    logMessage = "ERROR-400.2 - Invalid deposit process result.";
                    _logger.LogError(logMessage);
                    retVal = BadRequest(logMessage); //Invalid process result
                    break;
            }

            return retVal;
        }
    }
}
