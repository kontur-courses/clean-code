using System.Text;

namespace Markdown.Tokens;

internal class UnderscoreToken(string value, int start) : TextToken(value, start)
{
    public override StringBuilder WriteHtml()
    {
        return Role switch
        {
            TokenRole.Opening => new StringBuilder("<em>"),
            TokenRole.Closing => new StringBuilder("</em>"),
            _ => new StringBuilder(Value)
        };
    }
}
