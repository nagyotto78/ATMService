namespace ATMService.Enums
{
    /// <summary>
    /// Processing result codes
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// Unknown result
        /// </summary>
        None,

        /// <summary>
        /// The input data is valid
        /// </summary>
        Valid,

        /// <summary>
        /// The operation was succeeded
        /// </summary>
        Success,

        /// <summary>
        /// Business logic error
        /// </summary>
        Error,

        /// <summary>
        /// The withdrawal input number is divisible by 1000
        /// </summary>
        WithDrawalInvalidNumber,

        /// <summary>
        /// There is not enought money in the ATM storage
        /// </summary>
        WithDrawalCanNotServed,

        /// <summary>
        /// An error has occured during the deposit process
        /// </summary>
        DepositError

    }
}
