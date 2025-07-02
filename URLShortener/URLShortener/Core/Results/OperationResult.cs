namespace URLShortener.Core.Results
{
    public class OperationResult
    {
        public bool Success { get; init; }
        public string? ErrorMessage { get; init; }
        public string? ErrorCode { get; init; }

        public static OperationResult Ok() => new() { Success = true };
        public static OperationResult Fail(string message, string? code = null) =>
            new() { Success = false, ErrorMessage = message, ErrorCode = code };
    }
}
