using ATMService.Models;
using ATMService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/withdrawal
        ///             
        ///     34000
        ///     
        /// </remarks>
        /// <param name="data" example="34000">Amount</param>
        /// <returns>Withdrawn amount per denomination</returns>
        /// <response code="200">Withdrawn amount per denomination</response>
        /// <response code="400">If the amount is not divisible by 1000 or not greater than zero</response> 
        /// <response code="503">If the amount withdrawal is not possible</response> 
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Denominations), 200)]
        public async Task<IActionResult> WithDrawalAsync([FromBody][Required] int data)
        {
            IActionResult retVal;
            OperationResult<Denominations> result = await _service.WithDrawalAsync(data);
            string logMessage;

            switch (result?.ResultCode)
            {
                case Enums.ResultCode.Success:
                    _logger.LogInformation($"Withdrawn amount: {JsonSerializer.Serialize(result.Result)}");
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
