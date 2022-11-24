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
        else
        {
            var textInsideTag = text.Substring(root.FirstPosition + root.Opening.Length,
                                root.Length - root.Opening.Length - root.Ending.Length);
            stringBuilder.Append(EscapeRules.RemoveEscapes(textInsideTag));
        }

        AddTag(stringBuilder, root, false);
        return stringBuilder.ToString();
    }

    private string NestedTagsToHtml(string text, Token root)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(GetTextBeforeNestedTag(text, root, root.NestedTokens[0]));

        for (var i = 0; i < root.NestedTokens.Count; i++)
        {
            var token = root.NestedTokens[i];
            stringBuilder.Append(ToHtml(text, token));
            if (i < root.NestedTokens.Count - 1)
                stringBuilder.Append(GetTextBetweenNestedTags(text, token, root.NestedTokens[i + 1]));
        }
        stringBuilder.Append(GetTextAfterNestedTag(text, root, root.NestedTokens[^1]));
        return stringBuilder.ToString();
    }

    private static void AddTag(StringBuilder stringBuilder, Token token, bool isOpening)
    {
        var tagInfo = (token.TokenType, isOpening);
        if (HtmlTags.ContainsKey(tagInfo))
            stringBuilder.Append(HtmlTags[tagInfo]);
    }

    private static ReadOnlySpan<char> GetTextBeforeNestedTag(string text, Token parent, Token child)
    {
        var start = parent.FirstPosition + parent.Opening.Length;
        var length = child.FirstPosition - start;
        return text.IsRangeInBound(start, length) ? EscapeRules.RemoveEscapes(text.Substring(start, length)) : new ReadOnlySpan<char>();
    }
    
    private static ReadOnlySpan<char> GetTextAfterNestedTag(string text, Token parent, Token child)
    {
        var start = child.LastPosition + 1;
        var length = parent.LastPosition - parent.Ending.Length - start + 1;
        return text.IsRangeInBound(start, length) ? EscapeRules.RemoveEscapes(text.Substring(start, length)) : new ReadOnlySpan<char>();
    }
    
    private static ReadOnlySpan<char> GetTextBetweenNestedTags(string text, Token firstToken, Token secondToken)
    {
        var start = firstToken.LastPosition + 1;
        var length = secondToken.FirstPosition - start;
        return text.IsRangeInBound(start, length) ? EscapeRules.RemoveEscapes(text.Substring(start, length)) : new ReadOnlySpan<char>();
    }
}