using Markdown.Tokens;
using Markdown.Tokens.HtmlToken;

namespace Markdown;

public static class Extantions
{
    public static bool IsOnlyDight(this IEnumerable<BaseHtmlToken> tokens)
    {
        if (tokens.Count() < 3)
            throw new ArgumentException("Method takes at least three node");
        return tokens
            .Skip(1)
            .Take(tokens.Count() - 2)
            .SelectMany(node => node.Value)
            .All(char.IsDigit);
    }
    public static IEnumerable<BaseHtmlToken>? TextifyTags(this IEnumerable<BaseHtmlToken> nodes)
    {
        foreach (var node in nodes)
        {
            if (node is TextToken or HtmlTokenWithTag)
                yield return node;
            else
                yield return new TextToken(node.Value);
        }
    }
    public static IEnumerable<T> InnerElements<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable.Count() < 2)
            throw new ArgumentException("Should have at least 2 elements");
        if (enumerable.Count() == 2)
            return Enumerable.Empty<T>();
        return enumerable.Skip(1).Take(enumerable.Count() - 2);
    }
}