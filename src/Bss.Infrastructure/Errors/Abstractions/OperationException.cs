namespace Bss.Infrastructure.Errors.Abstractions;
public class OperationException : Exception
{
    public string ErrorCode { get; protected set; }
    public IEnumerable<string> ErrorDetails { get; protected set; } = [];

    // Constructor accepting message and optional error code
    public OperationException(string message, string? errorCode = null)
        : base(message)
    {
        ErrorCode = errorCode ?? OperationErrorCodes.InternalError;
    }

    // Constructor accepting message, details, and optional error code
    public OperationException(string message, IEnumerable<string> details, string? errorCode = null)
        : base(message)
    {
        ErrorCode = errorCode ?? OperationErrorCodes.InternalError;
        ErrorDetails = details ?? [];
    }

    // Constructor accepting message, details, error code, and inner exception
    public OperationException(string message, Exception innerException, IEnumerable<string> details, string? errorCode = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode ?? OperationErrorCodes.InternalError;
        ErrorDetails = details ?? [];
    }

    // Constructor accepting details and optional error code
    public OperationException(IEnumerable<string> details, string? errorCode = null)
        : this("Several problems occurred.", details, errorCode)
    {
    }

    // Constructor accepting message, error code, and inner exception
    public OperationException(string message, Exception innerException, string? errorCode = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode ?? OperationErrorCodes.InternalError;
    }
}
