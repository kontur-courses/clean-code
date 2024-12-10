using System.Text;
using Markdown.Tokens;

namespace Markdown.TokenConverters;

public class StrongConverter : TokenConverterBase
{
    public override void Render(BaseToken baseToken, StringBuilder result)
    {
        result.Append("<strong>");
        RenderChildren(baseToken, result);
        result.Append("</strong>");
    }
}