using Markdown.Interfaces;
using Markdown.Parsers;
using Markdown.Renderers;
using Markdown.Tags;

namespace Markdown;

public sealed class Md
{
    private readonly MarkdownParser _parser;
    private readonly HtmlRenderer _renderer;

    public Md()
    {
        var inlineParser = new InlineParser([new StrongTag(), new EmphasisTag()]);
        var blockTags = new IBlockTag[]
        {
            new HeadingTag(),
            new UnorderedListTag(),
            new ParagraphTag()
        };

        _parser = new MarkdownParser(blockTags);
        _renderer = new HtmlRenderer(inlineParser);
    }

    public string Render(string markdownText)
    {
        var markdownBlocks = _parser.Parse(markdownText);
        return _renderer.Render(markdownBlocks);
    }
}