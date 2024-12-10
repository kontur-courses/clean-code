using System.Text;
using Markdown.Interfaces;
using Markdown.Tokens;

namespace Markdown.TokenConverters;

public abstract class TokenConverterBase : ITokenConverter
{
    public abstract void Render(BaseToken baseToken, StringBuilder result);

    protected static void RenderChildren(BaseToken baseToken, StringBuilder result)
    {
        foreach (var child in baseToken.Children)
        {
            var converter = TokenConverterFactory.GetConverter(child.Type);
            converter.Render(child, result);
        }
    }
}