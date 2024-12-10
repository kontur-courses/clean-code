using System.Text;
using Markdown.Tokens;

namespace Markdown.TokenConverters;

public class HeaderConverter : TokenConverterBase
{
    public override void Render(BaseToken baseToken, StringBuilder result)
    {
        var level = baseToken.HeaderLevel;
        result.Append($"<h{level}>");
        RenderChildren(baseToken, result);
        result.Append($"</h{level}>");
    }
}