using Markdown.Abstractions;

namespace Markdown;

public class Md
{
    private readonly ITokenizer tokenizer;
    private readonly ITokenCollectionParser parser;
    private readonly IRenderer renderer;

    public Md(ITokenizer tokenizer, ITokenCollectionParser parser, IRenderer renderer)
    {
        this.tokenizer = tokenizer;
        this.parser = parser;
        this.renderer = renderer;
    }
    public string Render(string markdownText)
    {
        var tokens = tokenizer.Tokenize(markdownText);
        var tagNodes = parser.Parse(tokens);
        var html = renderer.Render(tagNodes);
        return html;
    }
}