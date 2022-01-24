using ATMService.DAL.Entities;
using ATMService.DAL.Repositories;
using ATMService.Enums;
using ATMService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<OperationResult<int>> DepositAsync(Dictionary<string, int> data)
        {
            OperationResult<int> retVal = null;

            try
            {

                // Read available denominations
                Dictionary<string, MoneyDenomination> availableDenominations = await _moneyDenominationRepository.Read()
                                                                                                                 .ToDictionaryAsync(k => k.Key);
                if (data != null)
                {
                    // Logging input data
                    _logger.LogInformation($"Input data : {JsonSerializer.Serialize(data)}");

                    StringBuilder errorMessages = new ();

                    Dictionary<string, MoneyStorage> storage = await _moneyStorageRepository.Read(null, null, "MoneyDenomination")
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
                        retVal = new OperationResult<int>(Enums.ResultCode.Success)
                        {
                            Result = await _moneyStorageRepository.GetBalanceAsync()
                        };

                        LogStorageState("ATM Storage after process:");

                    }
                    else
                    {
                        // An error has occured
                        retVal = new OperationResult<int>(Enums.ResultCode.DepositError)
                        {
                            ErrorMessage = errorMessages.ToString()
                        };
                    }
                }
            }
            catch (System.Exception exc)
            {
                _logger.LogError(exc.Message);
                retVal = new OperationResult<int>(Enums.ResultCode.Error)
                {
                    ErrorMessage = exc.Message
                };
            }
            return retVal;
        }

        #endregion

        #region WithDrawal

        public async Task<OperationResult<Dictionary<string, int>>> WithDrawalAsync(int data)
        {
            OperationResult<Dictionary<string, int>> retVal;

            ResultCode validationResult = await ValidateWithDrawal(data);
            if (validationResult == ResultCode.Valid)
            {
                retVal = new OperationResult<Dictionary<string, int>>()
                {
                    Result = new Dictionary<string, int>()
                };

                // Load ATM storage
                Dictionary<string, MoneyStorage> storage = await _moneyStorageRepository.Read(null, null, "MoneyDenomination")
                                                                                        .ToDictionaryAsync(k => k.MoneyDenomination.Key);
                LogStorageState("ATM Storage before process:");

                // Evaluate value
                Dictionary<string, int> denominations = EvaluateWithDrawalValue(storage, data);

                if (denominations == null)
                {
                    // The money withdrawal is not possible
                    retVal = new OperationResult<Dictionary<string, int>>(ResultCode.WithDrawalNotPossible);
                }
                else
                {
                    // Update ATM storage
                    foreach (KeyValuePair<string, int> item in denominations)
                    {
                        // Decrease denomination count
                        await ChangeDenominationStorage(null, storage, new KeyValuePair<string, int>(item.Key, -item.Value));
                    }
                    await _moneyStorageRepository.SaveChangesAsync();

                    // Set response value
                    retVal = new OperationResult<Dictionary<string, int>>(ResultCode.Success)
                    {
                        Result = denominations
                    };

                    LogStorageState("ATM Storage after process:");

                }
            }
            else
            {
                // Invalid source data
                retVal = new OperationResult<Dictionary<string, int>>(validationResult);
            }

            return retVal;
        }

        /// <summary>
        /// Collecting the required denomintaions from the ATM storage
        /// </summary>
        /// <param name="storage">ATM storage</param>
        /// <param name="data">Amount</param>
        /// <returns>Denomination collecton</returns>
        private Dictionary<string, int> EvaluateWithDrawalValue(Dictionary<string, MoneyStorage> storage, int data)
        {
            Dictionary<string, int> retVal = null;
            List<MoneyStorage> filteredStorage = storage.Values.Where(i => i.MoneyDenomination.Value <= data).ToList();
            int[,] changingMatrix = CreateChangingMatrix(filteredStorage, data);    // Create changing matrix (Fill with available denominations)

            if (changingMatrix != null)
            {
                int remaind = MoneyChange(ref changingMatrix, data, 0);         // Money changing

                if (remaind == 0)
                {
                    // Fill result dictionary
                    retVal = new Dictionary<string, int>();
                    for (int i = 0; i < changingMatrix.GetLength(0); i++)
                    {
                        if (changingMatrix[i, 2] > 0)
                        {
                            retVal.Add(changingMatrix[i, 0].ToString(), changingMatrix[i, 2]);
                        }
                    }
                }
            }
            return retVal;
        }

        /// <summary>
        /// Create money changing matrix
        /// </summary>
        /// <param name="filteredStorage">Filtered ATM storage content</param>
        /// <param name="data">Required amount</param>
        /// <returns>Base matrix for evaluation</returns>
        private static int[,] CreateChangingMatrix(List<MoneyStorage> filteredStorage, int data)
        {
            int i = 0;
            int[,] retVal = null;
            if (filteredStorage?.Count > 0)
            {
                retVal = new int[filteredStorage.Count, 3];
                foreach (MoneyStorage item in filteredStorage.OrderByDescending(i => i.MoneyDenomination.Value))
                {
                    retVal[i, 0] = item.MoneyDenomination.Value;                                    // Value
                    retVal[i, 1] = Math.Min(data / item.MoneyDenomination.Value, item.Count);       // Max Useable count
                    retVal[i, 2] = 0;                                                               // Used count
                    i++;
                }
            }
            return retVal;
        }

        /// <summary>
        /// Money changing algoritm
        /// </summary>
        /// <param name="changingMatrix">Changing matrix</param>
        /// <param name="data">Amount </param>
        /// <param name="denominationIndex">Denomination index in array</param>
        /// <returns>Reamind amount</returns>
        private int MoneyChange(ref int[,] changingMatrix, int data, int denominationIndex)
        {
            int retVal = data;
            int maxCount = Math.Min(data / changingMatrix[denominationIndex, 0], changingMatrix[denominationIndex, 1]);
            for (int j = maxCount; j >= 0 && retVal != 0; j--)
            {
                retVal = data - changingMatrix[denominationIndex, 0] * j;     // Set remainded amount
                changingMatrix[denominationIndex, 2] = j;               // Current solution the given denomination
                if (retVal != 0 && denominationIndex < changingMatrix.GetLength(0) - 1)
                {
                    retVal = MoneyChange(ref changingMatrix, retVal, denominationIndex + 1);      // Change reaminded amount
                }

            }
            return retVal;
        }

        /// <summary>
        /// Validate input data
        /// </summary>
        /// <param name="data">Money value</param>
        /// <returns>Validation result</returns>
        private async Task<ResultCode> ValidateWithDrawal(int data)
        {
            ResultCode retVal = ResultCode.Valid;

            if (data % 1000 > 0 || data <= 0)
            {
                // Amount is not divisible by 1000 or less than zero
                retVal = ResultCode.WithDrawalInvalidNumber;
            }

            // Balance checking
            int balance = await _moneyStorageRepository.GetBalanceAsync();
            if (balance < data)
            {
                // Amount is greater than the ATM balance
                retVal = ResultCode.WithDrawalNotPossible;
            }
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
                    await _moneyStorageRepository.UpdateAsync(money, false);
                }
            }
            else if (availableDenominations != null && item.Value > 0)
            {

                // Insert
                MoneyStorage money = new ()
                {
                    MoneyDenomination = availableDenominations[item.Key],
                    Count = item.Value
                };
                await _moneyStorageRepository.CreateAsync(money, false);

                storage.Add(item.Key, money);
            }
        }

        /// <summary>
        /// Write log about the ATM storage current state
        /// </summary>
        /// <param name="message">Logger message</param>
        /// <returns></returns>
        private async void LogStorageState(string message)
        {
            var storage = await _moneyStorageRepository.Read(null,
                                                             i => i.OrderBy(d => d.MoneyDenomination.Value),
                                                             "MoneyDenomination")
                                                       .AsNoTracking()
                                                       .Select(i => new { i.MoneyDenomination.Key, i.Count })
                                                       .ToListAsync();

            StringBuilder sb = new ();

            // Log the denominations count 
            sb.AppendLine(message);
            foreach (var item in storage)
            {
                sb.AppendLine($"{item.Key} => {item.Count}");
            }
            _logger.LogInformation(sb.ToString());
        }

        #endregion

    }
}
