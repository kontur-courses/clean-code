using System.Text;

namespace Markdown.Tokens;

internal class HeaderToken(string value, int start, string text, List<TextToken> content) : TextToken(value, start)
{
    public List<TextToken> Content { get; } = content;

    public string Text { get; } = text;

    public override StringBuilder WriteHtml()
    {
        var result = new StringBuilder();
        result.Append("<h1>");

        foreach (var token in Content)
        {
            result.Append(token.WriteHtml());
        }

        result.Append("</h1>");

        return result;
    }
}
