using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown;

public static class ListExtensions
{
    public static bool TryAddToken(this List<IToken> tokens, ITag tag, string markdownText, int idx)
    {
        var instance = tag.TryFindToken(markdownText, idx);

        if (instance != null && !TokenHighlighter.Excluded.Any(token => IToken.TokenEquals(token, instance)))
        {
            tokens.Add(instance);
            return true;
        }

        return false;
    }
}