namespace Bss.Infrastructure.Errors.Abstractions;

public class OperationErrorResult
{
    public required string ErrorCode { get; init; } = string.Empty;
    public required string ErrorMessage { get; init; } = string.Empty;
    public required string[] ErrorDetails { get; init; } = [];
    public required string Detail { get; init; } = string.Empty;

    public static OperationErrorResult FromException(Exception exception)
    {
        if (exception is OperationException operationException)
        {
            return new OperationErrorResult
            {
                ErrorCode = operationException.ErrorCode,
                ErrorMessage = operationException.Message,
                ErrorDetails = operationException.ErrorDetails.ToArray(),
                Detail = operationException.ErrorDetails.Any() ? operationException.ErrorDetails.First() : operationException.Message
            };
        }

        return new OperationErrorResult
        {
            ErrorCode = OperationErrorCodes.InternalError,
            ErrorMessage = exception.Message,
            ErrorDetails = [],
            Detail = exception.Message
        };
    }
}