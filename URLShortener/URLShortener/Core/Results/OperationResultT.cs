namespace URLShortener.Core.Results
{
    public class OperationResult<T> : OperationResult
    {
        public T? Data { get; init; }

        public static OperationResult<T> Ok(T data)
        {
            return new OperationResult<T>
            {
                Success = true,
                Data = data
            };
        }

        public static new OperationResult<T> Fail(string errorMessage, string? errorCode = null)
        {
            return new OperationResult<T>
            {
                Success = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode
            };
        }
        public static OperationResult<T> FromOperationResult(OperationResult result)
        {
            return new OperationResult<T>
            {
                Success = result.Success,
                ErrorMessage = result.ErrorMessage,
                ErrorCode = result.ErrorCode,
                Data = default!
            };
        }
    }
}
