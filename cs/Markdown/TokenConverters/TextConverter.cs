using System.Text;
using Markdown.Interfaces;
using Markdown.Tokens;

namespace Markdown.TokenConverters;

public class TextConverter : ITokenConverter
{
    public void Render(BaseToken baseToken, StringBuilder result)
    {
        result.Append(baseToken.Content);
    }
}