using FluentAssertions;
using Markdown.Node;
using Markdown.Node.Type;
using Markdown.Renderer;

namespace Markdown.Tests;

[TestFixture]
public class HtmlRendererTests
{
    private HtmlRenderer _renderer;

    [SetUp]
    public void SetUp()
    {
        _renderer = new HtmlRenderer();
    }

    [Test]
    public void Render_ShouldReturnEmptyString_WhenBlocksEmpty()
    {
        _renderer.Render([]).Should().BeEmpty();
    }

    [Test]
    public void Render_ShouldRenderParagraph_WithoutHtmlWrapper()
    {
        var block = new BlockNode(BlockType.Paragraph, [new TextNode("Привет мир")]);

        var result = _renderer.Render([block]);

        result.Should().Be("Привет мир");
    }

    [Test]
    public void Render_ShouldRenderHeader_WithH1Tag()
    {
        var block = new BlockNode(BlockType.Header1, [new TextNode("Заголовок")]);

        var result = _renderer.Render([block]);

        result.Should().Be("<h1>Заголовок</h1>");
    }

    [Test]
    public void Render_ShouldRenderItalicEmphasis_WithEmTag()
    {
        var block = new BlockNode(BlockType.Paragraph, [
            new EmphasisNode(EmphasisType.Italic, [new TextNode("курсив")])
        ]);

        var result = _renderer.Render([block]);

        result.Should().Be("<em>курсив</em>");
    }

    [Test]
    public void Render_ShouldRenderBoldEmphasis_WithStrongTag()
    {
        var block = new BlockNode(BlockType.Paragraph, [
            new EmphasisNode(EmphasisType.Bold, [new TextNode("жирный")])
        ]);

        var result = _renderer.Render([block]);

        result.Should().Be("<strong>жирный</strong>");
    }

    [Test]
    public void Render_ShouldRenderLinkNode_WithAnchorTag()
    {
        var block = new BlockNode(BlockType.Paragraph, [
            new LinkNode("https://example.com", [new TextNode("текст ссылки")])
        ]);

        var result = _renderer.Render([block]);

        result.Should().Be("<a href=\"https://example.com\">текст ссылки</a>");
    }

    [Test]
    public void Render_ShouldSeparateBlocks_WithNewlines()
    {
        var blocks = new[]
        {
            new BlockNode(BlockType.Header1, [new TextNode("Заголовок")]),
            new BlockNode(BlockType.Paragraph, [new TextNode("Абзац 1")]),
            new BlockNode(BlockType.Paragraph, [new TextNode("Абзац 2")])
        };

        var result = _renderer.Render(blocks);

        result.Should().Be("<h1>Заголовок</h1>\nАбзац 1\nАбзац 2");
    }

    [Test]
    public void Render_ShouldRenderNestedNodes_Correctly()
    {
        var block = new BlockNode(BlockType.Header1, [
            new TextNode("Заголовок "),
            new EmphasisNode(EmphasisType.Bold, [
                new TextNode("с "),
                new EmphasisNode(EmphasisType.Italic, [new TextNode("курсивом")]),
                new TextNode(" внутри")
            ])
        ]);

        var result = _renderer.Render([block]);

        result.Should().Be("<h1>Заголовок <strong>с <em>курсивом</em> внутри</strong></h1>");
    }
}
