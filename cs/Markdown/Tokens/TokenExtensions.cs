namespace Markdown.Tokens;

internal static class TokenExtensions
{
    public static string ConvertToHtml(this IEnumerable<IToken> tokens)
    {
        return string.Concat(tokens.Select(token => token.Value));
    }

    public static bool HasWhiteSpaceBetween(this LinkedListNode<IToken> token, TagToken pairTag)
    {
        for (var current = token.Next; current.Value != pairTag; current = current.Next)
            if (current.Value.Value?.Contains(' ') == true)
                return true;

        return false;
    }
}