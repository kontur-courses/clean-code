using System.Runtime.InteropServices;

namespace Markdown.Extensions;

public static class ReadOnlyMemoryExtensions
{
    public static bool Contains(this ReadOnlyMemory<char> memory, char value)
    {
        ArgumentExceptionHelpers.ThrowIfFalse(
            MemoryMarshal.TryGetString(memory, out var str, out var start, out var length),
            "Underlying object in the input argument is not a string");
        for (var i = start; i < start + length; i++)
        {
            if (str![i] == value)
                return true;
        }

        return false;
    }
    
    public static bool ContainsNumber(this ReadOnlyMemory<char> memory)
    {
        ArgumentExceptionHelpers.ThrowIfFalse(
            MemoryMarshal.TryGetString(memory, out var str, out var start, out var length),
            "Underlying object in the input argument is not a string");
        for (var i = start; i < start + length; i++)
        {
            if (int.TryParse(str![i].ToString(), out _))
                return true;
        }

        return false;
    }
}