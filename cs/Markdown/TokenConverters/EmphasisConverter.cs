using System.Text;
using Markdown.Tokens;

namespace Markdown.TokenConverters;

public class EmphasisConverter : TokenConverterBase
{
    public override void Render(BaseToken baseToken, StringBuilder result)
    {
        result.Append("<em>");
        RenderChildren(baseToken, result);
        result.Append("</em>");
    }
}