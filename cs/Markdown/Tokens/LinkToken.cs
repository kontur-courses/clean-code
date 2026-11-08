using System.Text;

namespace Markdown.Tokens;

internal sealed class LinkToken(string raw, int start, string text, string url, List<TextToken> content)
    : TextToken(raw, start)
{
    public string Text { get; } = text;

    public string Url { get; } = url;

    public List<TextToken> Content { get; } = content;

    public override StringBuilder WriteHtml()
    {
        var result = new StringBuilder();
        result.Append("<a href=\"").Append(Url).Append("\">");

        foreach (var token in Content)
        {
            result.Append(token.WriteHtml());
        }

        result.Append("</a>");

        return result;
    }
}
