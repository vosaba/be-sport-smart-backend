namespace Bss.Infrastructure.Commands.Abstractions;

public class CommandConstants
{
    public const string RoutePrefix = "api/v1";

    public const string CommandMethodName = "Handle";

    public const string CommandTypeSuffix = "Handler";

    public const string HandlerSuffixPattern = @"Handler$";

    public const string CommandNamespacePart = "Commands";
}
