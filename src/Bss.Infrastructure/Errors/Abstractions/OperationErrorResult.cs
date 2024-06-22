namespace Bss.Infrastructure.Errors.Abstractions;

public class OperationErrorResult
{
    public required string ErrorCode { get; init; } = string.Empty;
    public required string ErrorMessage { get; init; } = string.Empty;

    public required string[] ErrorDetails { get; init; } = [];

    public static OperationErrorResult FromException(Exception exception)
    {
        if (exception is OperationException operationException)
        {
            return new OperationErrorResult
            {
                ErrorCode = operationException.ErrorCode,
                ErrorMessage = operationException.Message,
                ErrorDetails = operationException.ErrorDetails.ToArray()
            };
        }

        return new OperationErrorResult
        {
            ErrorCode = OperationErrorCodes.InternalError,
            ErrorMessage = exception.Message,
            ErrorDetails = []
        };
    }
}