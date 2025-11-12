using FluentAssertions;
using NUnit.Framework;
using Markdown.Rendering;
using Markdown.Parsing.Nodes;

namespace Markdown.Tests;

public class HtmlRendererTests
{
    private static string R(RootNode node)
        => HtmlRenderer.Render(node);

    [Test]
    public void Html_ShouldRenderEmptyDocument_Test()
    {
        var root = new RootNode([]);
        R(root).Should().Be("");
    }

    [Test]
    public void Html_ShouldRenderParagraph_WithText_Test()
    {
        var p = new ParagraphNode([new TextNode("hello")]);

        R(new RootNode([p]))
            .Should().Be("<p>hello</p>");
    }

    [Test]
    public void Html_ShouldRenderParagraph_WithMultipleInlines_Test()
    {
        var p = new ParagraphNode([
            new TextNode("hello "),
            new EmNode([new TextNode("world")]),
            new TextNode(" again")
        ]);

        R(new RootNode([p]))
            .Should().Be("<p>hello <em>world</em> again</p>");
    }

    [Test]
    public void Html_ShouldRenderHeaderWithPlainText_Test()
    {
        var h = new HeaderNode([new TextNode("Title")]);

        R(new RootNode([h]))
            .Should().Be("<h1>Title</h1>");
    }

    [Test]
    public void Html_ShouldRenderHeaderWithInlineFormatting_Test()
    {
        var h = new HeaderNode([
            new TextNode("Hello "),
            new StrongNode([new TextNode("strong ")]),

            new EmNode([new TextNode("em")])
        ]);

        R(new RootNode([h]))
            .Should().Be("<h1>Hello <strong>strong </strong><em>em</em></h1>");
    }

    [Test]
    public void Html_ShouldRenderEm_Test()
    {
        var p = new ParagraphNode([
            new TextNode("a "),
            new EmNode([new TextNode("b")]),
            new TextNode(" c")
        ]);

        R(new RootNode([p]))
            .Should().Be("<p>a <em>b</em> c</p>");
    }

    [Test]
    public void Html_ShouldRenderStrong_Test()
    {
        var p = new ParagraphNode([
            new TextNode("a "),
            new StrongNode([new TextNode("b")]),
            new TextNode(" c")
        ]);

        R(new RootNode([p]))
            .Should().Be("<p>a <strong>b</strong> c</p>");
    }

    [Test]
    public void Html_ShouldRenderNestedEmInsideStrong_Test()
    {
        var strong = new StrongNode([
            new TextNode("a "),
            new EmNode([new TextNode("b")]),
            new TextNode(" c")
        ]);

        var p = new ParagraphNode([strong]);

        R(new RootNode([p]))
            .Should().Be("<p><strong>a <em>b</em> c</strong></p>");
    }

    [Test]
    public void Html_ShouldRenderEscapedChar_AsLiteral_Test()
    {
        var p = new ParagraphNode([
            new TextNode("x"),
            new EscapedNode('_'),
            new TextNode("y")
        ]);

        R(new RootNode([p]))
            .Should().Be("<p>x_y</p>");
    }

    [Test]
    public void Html_ShouldEscapeHtmlInsideTextNodes_Test()
    {
        var p = new ParagraphNode([new TextNode("<hello> & \"world\"")]);

        R(new RootNode(new() { p }))
            .Should().Be("<p>&lt;hello&gt; &amp; &quot;world&quot;</p>");
    }

    [Test]
    public void Html_ShouldRenderLink_WithTextOnly_Test()
    {
        var link = new LinkNode(
            [new TextNode("google")],
            "https://google.com"
        );

        var p = new ParagraphNode([link]);

        R(new RootNode([p]))
            .Should().Be("<p><a href=\"https://google.com\">google</a></p>");
    }

    [Test]
    public void Html_ShouldRenderLink_WithInlineFormatting_Test()
    {
        var link = new LinkNode(
            [
                new TextNode("a "),
                new EmNode([new TextNode("b")]),
                new TextNode(" c")
            ],
            "/x"
        );

        var p = new ParagraphNode([link]);

        R(new RootNode([p]))
            .Should().Be("<p><a href=\"/x\">a <em>b</em> c</a></p>");
    }

    [Test]
    public void Html_ShouldRenderComplexNestedFormatting_Test()
    {
        var p = new ParagraphNode([
            new TextNode("start "),
            new StrongNode([
                new TextNode("A "),
                new EmNode([new TextNode("B")]),
                new TextNode(" C")
            ]),

            new TextNode(" end")
        ]);

        R(new RootNode([p]))
            .Should().Be("<p>start <strong>A <em>B</em> C</strong> end</p>");
    }

    [Test]
    public void Html_ShouldEscapeUrlInHref_Test()
    {
        var link = new LinkNode(
            [new TextNode("x")],
            "https://example.com?a=1&b=<>&\""
        );

        var p = new ParagraphNode([link]);

        R(new RootNode([p]))
            .Should().Be("<p><a href=\"https://example.com?a=1&amp;b=&lt;&gt;&amp;&quot;\">x</a></p>");
    }
}