using System.Text;
using Markdown.Tokens;

namespace Markdown;

public class MarkdownToHtmlConverter
{
    private static readonly Dictionary<(TokenType type, bool isOpening), string> HtmlTags = new()
    {
        [(TokenType.Header, true)] = "<h1>",
        [(TokenType.Header, false)] = "</h1>",
        [(TokenType.Bold, true)] = "<strong>",
        [(TokenType.Bold, false)] = "</strong>",
        [(TokenType.Italic, true)] = "<em>",
        [(TokenType.Italic, false)] = "</em>"
    };

    public string ToHtml(string text, Token root)
    {
        var stringBuilder = new StringBuilder();
        AddTag(stringBuilder, root, true);

        if (root.NestedTokens.Count > 0)
            stringBuilder.Append(NestedTagsToHtml(text, root));
        else if (root is TextToken asTextToken)
        {
            stringBuilder.Append(text.AsSpan(asTextToken.FirstPosition, asTextToken.Length));
        }

        AddTag(stringBuilder, root, false);
        return stringBuilder.ToString();
    }

    private string NestedTagsToHtml(string text, Token root)
    {
        var stringBuilder = new StringBuilder();

        foreach (var token in root.NestedTokens)
        {
            if (token is TextToken asTextToken)
                stringBuilder.Append(text.AsSpan(asTextToken.FirstPosition, asTextToken.Length));
            else
                stringBuilder.Append(ToHtml(text, token));
        }
        return stringBuilder.ToString();
    }

    private static void AddTag(StringBuilder stringBuilder, Token token, bool isOpening)
    {
        var tagInfo = (token.TokenType, isOpening);
        if (HtmlTags.ContainsKey(tagInfo))
            stringBuilder.Append(HtmlTags[tagInfo]);
    }
}