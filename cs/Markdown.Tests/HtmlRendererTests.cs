using Markdown.Domain;
using FluentAssertions;
using static Markdown.Rendering.HtmlRenderer;

namespace Markdown.Tests;

[TestFixture]
public class HtmlRendererTests
{
    [TestCaseSource(nameof(NodeCases))]
    public void Render_ValidNode_ReturnsExpectedHtml(
        Node node,
        string expected)
    {
        var actual = RenderNodes([node]);

        actual.Should().Be(expected);
    }

    [Test]
    public void Render_NestedNodes_ReturnsExpectedHtml()
    {
        Node node = new ParagraphNode
        {
            Children =
            [
                new TextNode("Hello "),
                new StrongNode
                {
                    Children =
                    [
                        new TextNode("bold "),
                        new EmphasisNode
                        {
                            Children = [new TextNode("italic")]
                        }
                    ]
                },
                new TextNode("!")
            ]
        };

        var actual = RenderNodes([node]);

        actual.Should().Be(
            "<p>Hello <strong>bold <em>italic</em></strong>!</p>");
    }

    [TestCase("<", "&lt;")]
    [TestCase(">", "&gt;")]
    [TestCase("&", "&amp;")]
    [TestCase("\"", "&quot;")]
    [TestCase(
        "<script>alert(1)</script>",
        "&lt;script&gt;alert(1)&lt;/script&gt;")]
    public void Render_SpecialCharacters_EncodesHtml(
        string text,
        string expected)
    {
        var actual = RenderNodes([new TextNode(text)]);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Render_MultipleNodes_ConcatenatesHtml()
    {
        Node[] nodes =
        [
            new HeaderNode
            {
                Children = [new TextNode("Title")]
            },
            new ParagraphNode
            {
                Children = [new TextNode("Content")]
            }
        ];

        var actual = RenderNodes(nodes);


        actual.Should().Be("<h1>Title</h1><p>Content</p>");
    }

    [Test]
    public void Render_EmptyCollection_ReturnsEmptyString()
    {
        var actual = RenderNodes([]);

        actual.Should().BeEmpty();
    }

    [Test]
    public void Render_EmptyContainer_ReturnsEmptyHtmlElement()
    {
        Node node = new ParagraphNode
        {
            Children = []
        };

        var actual = RenderNodes([node]);

        actual.Should().Be("<p></p>");
    }

    private static IEnumerable<TestCaseData> NodeCases()
    {
        yield return new TestCaseData(
                new TextNode("Hello"),
                "Hello")
            .SetName("Render_TextNode");

        yield return new TestCaseData(
                new HeaderNode
                {
                    Children = [new TextNode("Hello")]
                },
                "<h1>Hello</h1>")
            .SetName("Render_HeaderNode");

        yield return new TestCaseData(
                new ParagraphNode
                {
                    Children = [new TextNode("Hello")]
                },
                "<p>Hello</p>")
            .SetName("Render_ParagraphNode");

        yield return new TestCaseData(
                new EmphasisNode
                {
                    Children = [new TextNode("Hello")]
                },
                "<em>Hello</em>")
            .SetName("Render_EmphasisNode");

        yield return new TestCaseData(
                new StrongNode
                {
                    Children = [new TextNode("Hello")]
                },
                "<strong>Hello</strong>")
            .SetName("Render_StrongNode");
    }
}