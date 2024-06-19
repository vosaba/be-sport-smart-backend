namespace Bss.Infrastructure.Shared;

public static class StringExtensions
{
    public static string ToLowerCamelCase(this string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            return char.ToLowerInvariant(value[0]) + value[1..];
        }

        return value;
    }
}
