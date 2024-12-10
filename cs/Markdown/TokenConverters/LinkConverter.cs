using System.Text;
using Markdown.Tokens;

namespace Markdown.TokenConverters;

public class LinkConverter : TokenConverterBase
{
    public override void Render(BaseToken token, StringBuilder result)
    {
        var linkToken = (LinkToken)token;

        result.Append($"<a href=\"{linkToken.Url}\">");
        RenderChildren(linkToken, result);
        result.Append("</a>");
    }
}