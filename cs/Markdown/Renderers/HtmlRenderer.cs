using System.Text;
using Markdown.Html;
using Markdown.Markdown.Tokens;

namespace Markdown.Renderers;

public class HtmlRenderer : IRenderer
{
    private readonly HtmlTagConverter tagConverter = new();

    public string Render(IList<IToken> tokens)
    {
        var tokenBuilder = new StringBuilder();

        foreach (var token in tokens)
        {
            tokenBuilder.Append(token.Type is TokenType.Tag ? tagConverter.Convert(token) : token.Content);
        }

        return tokenBuilder.ToString();
    }
}