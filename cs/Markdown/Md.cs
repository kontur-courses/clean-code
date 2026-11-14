using Markdown.Parser;
using Markdown.Parser.Interfaces;
using Markdown.Renderer;
using Markdown.Renderer.Interfaces;
using Markdown.Tokenizer;

namespace Markdown;

public class Md
{
    private readonly IBlockParser _blockParser;
    private readonly IRenderer _renderer;

    public Md() : this(
        new BlockParser(
            new MarkdownTokenizer(),
            new InlineParser()),
        new HtmlRenderer())
    {
    }

    public Md(IBlockParser blockParser, IRenderer renderer)
    {
        _blockParser = blockParser;
        _renderer = renderer;
    }

    public string Render(string markdown)
    {
        if (string.IsNullOrEmpty(markdown))
            return string.Empty;

        var blocks = _blockParser.Parse(markdown);
        return _renderer.Render(blocks);
    }
}
