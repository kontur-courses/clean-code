using Markdown.Markdown;
using Markdown.Renderers;

namespace Markdown;

public class Md(ITokenizer tokenizer, IRenderer renderer)
{
    public string Render(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        return renderer.Render(tokens);
    }
}