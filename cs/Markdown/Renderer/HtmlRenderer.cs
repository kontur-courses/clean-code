using System.Text;
using System.Web;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Renderer;

/// <summary>
/// HTML-рендерер. Преобразует токены в HTML-текст, экранируя спецсимволы в тексте
/// </summary>
public class HtmlRenderer : IRenderer
{
    public string Render(IEnumerable<IToken> tokens)
    {
        var sb = new StringBuilder();
        foreach (var token in tokens)
        {
            sb.Append(RenderToken(token));
        }

        return sb.ToString();
    }

    private string? RenderToken(IToken token)
    {
        return token switch
        {
            TextToken textToken => HttpUtility.HtmlEncode(textToken.TextContent),
            TagToken tagToken => RenderTagToken(tagToken),
            _ => null
        };
    }

    private string RenderTagToken(TagToken tagToken)
    {
        var sb = new StringBuilder();
        sb.Append($"<{tagToken.Tag.HtmlTag}");
        foreach (var (key, value) in tagToken.Attributes)
        {
            sb.Append($" {key}=\"{value}\"");
        }
        
        if (tagToken.Tag.SelfClosing)
        {
            sb.Append(" />");
            return sb.ToString();
        }

        sb.Append('>');
        foreach (var child in tagToken.Children)
        {
            sb.Append(RenderToken(child));
        }

        sb.Append($"</{tagToken.Tag.HtmlTag}>");
        return sb.ToString();
    }
}