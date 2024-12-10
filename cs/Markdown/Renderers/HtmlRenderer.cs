using System.Text;
using Markdown.Interfaces;
using Markdown.TokenConverters;
using Markdown.Tokens;

namespace Markdown.Renderers;

public class HtmlRenderer : IRenderer
{
    public string Render(IEnumerable<BaseToken> tokens)
    {
        var result = new StringBuilder();
        foreach (var token in tokens)
        {
            var converter = TokenConverterFactory.GetConverter(token.Type);
            converter.Render(token, result);
        }
        var text = result.ToString();
        text = TagReplacer.SimilarTagsNester(text);
        return text;
    }
}