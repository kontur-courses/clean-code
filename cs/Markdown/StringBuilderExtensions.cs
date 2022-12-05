using System.Text;

namespace Markdown;

public static class StringBuilderExtensions
{
    public static StringBuilder ReplaceAt(this StringBuilder str, int startIndex, int length, string replace)
    {
        return str.Remove(startIndex, Math.Min(length, str.Length - startIndex))
            .Insert(startIndex, replace);
    }
}