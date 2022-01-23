using ATMService.DAL.Entities;
using ATMService.DAL.Repositories;
using ATMService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ATMService.Services
{
    /// <summary>
    /// ATM service business logic (deposit, withdrawal)
    /// </summary>
    public class ATMService : IATMService
    {

        private readonly IMoneyDenominationRepository _moneyDenominationRepository;

        private readonly IMoneyStorageRepository _moneyStorageRepository;

        private readonly ILogger<ATMService> _logger;

        public ATMService(ILogger<ATMService> logger,
                          IMoneyDenominationRepository moneyDenominationRepository,
                          IMoneyStorageRepository moneyStorageRepository)
        {
            _logger = logger;
            _moneyDenominationRepository = moneyDenominationRepository;
            _moneyStorageRepository = moneyStorageRepository;
        }

        #region "Deposit"

        /// <summary>
        /// Put money into ATM storage
        /// </summary>
        /// <param name="data">Denominations count</param>
        /// <returns>ATM balance</returns>
        public async Task<OperationResult<long>> DepositAsync(Dictionary<string, int> data)
        {
            OperationResult<long> retVal = null;

            try
            {

                // Read available denominations
                Dictionary<string, MoneyDenomination> availableDenominations = await _moneyDenominationRepository.Read()
                                                                                                                 .ToDictionaryAsync(k => k.Key);
                if (data != null)
                {
                    // Logging input data
                    _logger.LogInformation($"Input data : {JsonSerializer.Serialize(data)}");

                    StringBuilder errorMessages = new StringBuilder();

                    // Load storage
                    Dictionary<string, MoneyStorage> storage = await _moneyStorageRepository.Read()
                                                                                            .ToDictionaryAsync(k => k.MoneyDenomination.Key);

                    foreach (KeyValuePair<string, int> item in data)
                    {
                        // Denomination checking
                        if (availableDenominations.ContainsKey(item.Key))
                        {
                            // Denomination count checking
                            if (item.Value >= 0 && errorMessages.Length == 0)
                            {
                                // Valid number
                                await ChangeDenominationStorage(availableDenominations, storage, item);
                            }
                            else
                            {
                                // Invalid number
                                errorMessages.Append($"{item.Key} - Invalid denomination count: ({item.Value}); ");
                            }
                        }
                        else
                        {
                            errorMessages.Append($"{item.Key} - Invalid denomination; ");
                        }
                    }
                    if (errorMessages.Length == 0)
                    {
                        // Save changes
                        await _moneyStorageRepository.SaveChangesAsync();

                        // Calculate balance
                        retVal = new OperationResult<long>(Enums.ResultCode.Success)
                        {
                            Result = await _moneyStorageRepository.GetBalanceAsync()
                        };
                    } 
                    else
                    {
                        // An error has occured
                        retVal = new OperationResult<long>(Enums.ResultCode.DepositError)
                        {
                            ErrorMessage = errorMessages.ToString()
                        };
                    }
                }
            }
            catch (System.Exception exc)
            {
                _logger.LogError(exc.Message);
                retVal = new OperationResult<long>(Enums.ResultCode.Error)
                {
                    ErrorMessage = exc.Message
                };
            }
            return retVal;
        }
    
        #endregion

        #region WithDrawal

        public async Task<OperationResult<Dictionary<string, int>>> WithDrawalAsync(long data)
        {
            OperationResult<Dictionary<string, int>> retVal = new OperationResult<Dictionary<string, int>>();

            //TODO: Business logic

            return retVal;
        }

        #endregion

        #region Common methods

        /// <summary>
        /// Apply changes by denomination item
        /// </summary>
        /// <param name="availableDenominations">Available denominations</param>
        /// <param name="item">Source item</param>
        /// <returns></returns>
        private async Task ChangeDenominationStorage(Dictionary<string, MoneyDenomination> availableDenominations,
                                               Dictionary<string, MoneyStorage> storage,
                                               KeyValuePair<string, int> item)
        {

            if (storage.ContainsKey(item.Key))
            {
                // Update
                MoneyStorage money = storage[item.Key];

                if (money != null)
                {
                    money.Count += item.Value;
                    await _moneyStorageRepository.UpdateAsync(money);
                }
            }
            else
            {
                // Insert
                MoneyStorage money = new MoneyStorage()
                {
                    MoneyDenomination = availableDenominations[item.Key],
                    Count = item.Value
                };
                await _moneyStorageRepository.CreateAsync(money);
            }
        }

        #endregion

    }
}
