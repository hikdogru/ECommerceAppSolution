namespace ECommerceApp.Core.Extensions;

public static class StringExtensions
{
    public static bool ContainsCaseInsensitive(this string source, string substring)
    {
        return source?.IndexOf(substring, StringComparison.OrdinalIgnoreCase) > -1;
    }
}