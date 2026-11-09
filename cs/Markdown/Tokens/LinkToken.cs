using System.Text;

namespace Markdown.Tokens;

internal sealed class LinkToken(string raw, int start, string url)
    : TextToken(raw, start)
{
    public string Url { get; } = url;
    
    public TokenRole Role { get; set; } = TokenRole.Literal;

    public override StringBuilder WriteHtml()
    {
        var result = new StringBuilder();
        if (Role == TokenRole.Opening)
            result.Append("<a href=\"").Append(Url).Append("\">");
        else if (Role == TokenRole.Closing) 
            result.Append("</a>");
        else 
            result.Append("[");

        return result;
    }
}
