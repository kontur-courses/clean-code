using System.Text;

namespace Markdown.Tokens;

internal class HeaderToken(string value, int start) : TextToken(value, start)
{
    public TokenRole Role { get; set; } = TokenRole.Literal;

    public override StringBuilder WriteHtml()
    {
        
        var result = new StringBuilder();
        if (Role == TokenRole.Opening)
            result.Append("<h1>");
        else if (Role == TokenRole.Closing)
            result.Append("</h1>");
        else
            result.Append("#");

        return result;
    }
}
