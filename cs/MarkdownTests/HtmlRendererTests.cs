using FluentAssertions;
using Markdown.Entities;
using Markdown.Parsers;
using Markdown.Renderers;
using Markdown.Tags;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class HtmlRendererTests
{
    private readonly HtmlRenderer renderer = new(new InlineParser([new StrongTag(), new EmphasisTag()]));

    [Test]
    public void Render_HeadingBlock_DelegatesToTag()
    {
        var blocks = new[]
        {
            new MarkdownBlock { Tag = new HeadingTag(), Content = "# Title _em_" }
        };

        renderer.Render(blocks).Should().Be("<h1>Title <em>em</em></h1>");
    }

    [Test]
    public void Render_SeveralBlocks_SeparatesThemWithLineBreak()
    {
        var blocks = new[]
        {
            new MarkdownBlock { Tag = new ParagraphTag(), Content = "первый" },
            new MarkdownBlock { Tag = new UnorderedListTag(), Content = "- второй" }
        };

        renderer.Render(blocks).Should().Be("первый\n<ul><li>второй</li></ul>");
    }
}