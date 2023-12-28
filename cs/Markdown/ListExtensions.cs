using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown;

public static class ListExtensions
{
    public static bool TryAddToken(this List<Token> tokens, Tag tag, string markdownText, int idx)
    {
        var instance = tag.TryFindToken(markdownText, idx);

        if (instance != null && !TokenHighlighter.Excluded.Any(token => Token.TokenEquals(token, instance)))
        {
            tokens.Add(instance);
            return true;
        }

        return false;
    }
}