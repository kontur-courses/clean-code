namespace Markdown;

public static class ArgumentExceptionHelpers
{
    public static void ThrowIfFalse(bool flag, string message)
    {
        if (!flag)
            throw new ArgumentException(message);
    }

    public static void ThrowIfNull(object? obj, string message)
    {
        if (obj == null)
            throw new ArgumentException(message);
    }
}