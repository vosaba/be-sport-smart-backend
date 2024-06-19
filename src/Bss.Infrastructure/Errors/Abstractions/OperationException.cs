namespace Bss.Infrastructure.Errors.Abstractions;

public class OperationErrorCodes
{
    public const string InvalidRequest = "InvalidRequest";
    public const string InvalidOperation = "InvalidOperation";
    public const string InternalError = "InternalError";
    public const string Conflict = "Conflict";
    public const string Forbidden = "Forbidden";

}
public class OperationException(string message, string? errorCode = null) : Exception(message)
{
    public string ErrorCode { get; protected set; } = errorCode ?? OperationErrorCodes.InternalError;
}
