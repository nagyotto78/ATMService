using ATMService.Enums;

namespace ATMService.Models
{

    /// <summary>
    /// Operation result
    /// </summary>
    public class OperationResult<T> 
    {

        public OperationResult()
        {
        }

        public OperationResult(ResultCode resultCode)
        {
            ResultCode = resultCode;
        }

        /// <summary>
        /// The process result code
        /// </summary>
        public ResultCode ResultCode { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The operation result
        /// </summary>
        public T Result { get; set; }

    }
}
