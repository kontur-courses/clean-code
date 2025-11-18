using FluentAssertions;
using Markdown.Parsing;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class HtmlRenderTests
{
    private HtmlRender render;

    [SetUp]
    public void Setup()
    {
        render = new HtmlRender();
    }

    [Test]
    public void TextNode_RenderVerbatim()
    {
        var document = new DocumentNode([new TextNode("plain")]);
        render.Render(document).Should().Be("plain");
    }

    [Test]
    public void EmphasisNode_WrapWithEmTags()
    {
        var document = new DocumentNode([new EmphasisNode([new TextNode("italic")])]);
        render.Render(document).Should().Be("<em>italic</em>");
    }

    [Test]
    public void StrongNode_WrapWithStrongTags()
    {
        var document = new DocumentNode([new StrongNode([new TextNode("bold")])]);
        render.Render(document).Should().Be("<strong>bold</strong>");
    }

    [Test]
    public void LinkNode_RenderAnchor()
    {
        var document = new DocumentNode([new LinkNode("https://example.com", [new TextNode("Example")])]);
        render.Render(document).Should().Be("<a href=\"https://example.com\">Example</a>");
    }

    [Test]
    public void HeadingNode_ClampLevelToRange()
    {
        var document = new DocumentNode([
            new HeadingNode([new TextNode("zero")]),
            new HeadingNode([new TextNode("seven")])
        ]);

        render.Render(document).Should().Be("<h1>zero</h1><h1>seven</h1>");
    }

    [Test]
    public void DocumentNode_RenderChildrenSequentially()
    {
        var document = new DocumentNode([
            new TextNode("Hello "),
            new StrongNode([new TextNode("world")]),
            new TextNode("!")
        ]);

        render.Render(document).Should().Be("Hello <strong>world</strong>!");
    }
}