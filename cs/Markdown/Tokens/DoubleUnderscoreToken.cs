using System.Text;

namespace Markdown.Tokens;
internal class DoubleUnderscoreToken(string value, int start) : TextToken(value, start)
{
    public override StringBuilder WriteHtml()
    {
        return Role switch
        {
            TokenRole.Opening => new StringBuilder("<strong>"),
            TokenRole.Closing => new StringBuilder("</strong>"),
            _ => new StringBuilder(Value)
        };
    }
}

