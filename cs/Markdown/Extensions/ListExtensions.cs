using Markdown.Markdown.Tokens;

namespace Markdown.Extensions;

public static class ListExtensions
{
    public static int IndexOf(this IReadOnlyList<IToken> list, IToken item)
    {
        for (var i = 0; i < list.Count; i++)
            if (list[i] == item)
                return i;
        return -1;
    }
}