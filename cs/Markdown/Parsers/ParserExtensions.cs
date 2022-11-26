using System.Text;
using Markdown.Contracts;
using Markdown.Tokens;

namespace Markdown.Parsers;

public static class ParserExtensions
{
    public static string ConcatChildren(this Token token, IReadOnlyDictionary<TokenType, ITokenParser> parsers,
        string? prefix = null, string? postfix = null)
    {
        var stringBuilder = new StringBuilder();
        if (!string.IsNullOrEmpty(prefix))
            stringBuilder.Append(prefix);

        foreach (var element in token.Children)
            stringBuilder.Append(
                parsers[element.Type].Parse(element));

        if (!string.IsNullOrEmpty(postfix))
            stringBuilder.Append(postfix);
        var result = stringBuilder.ToString();
        var isEmpty = result == prefix + postfix;
        return isEmpty ? string.Empty : result;
    }
}