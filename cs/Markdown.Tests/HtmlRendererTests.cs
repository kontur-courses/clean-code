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
        Node node = NodeFactory.Paragraph(NodeFactory.Text("Hello "),
            NodeFactory.Strong(NodeFactory.Text("bold "),
                NodeFactory.Emphasis(NodeFactory.Text("italic"))),
            NodeFactory.Text("!"));

        var actual = RenderNodes([node]);

        actual.Should().Be(
            "<p>Hello <strong>bold <em>italic</em></strong>!</p>");
    }

    [Test]
    public void Render_MultipleNodes_ConcatenatesHtml()
    {
        Node[] nodes =
        [
            NodeFactory.Header(NodeFactory.Text("Title")),
            NodeFactory.Paragraph(NodeFactory.Text("Content"))
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
        Node node = NodeFactory.Paragraph();

        var actual = RenderNodes([node]);

        actual.Should().Be("<p></p>");
    }

    private static IEnumerable<TestCaseData> NodeCases()
    {
        yield return new TestCaseData(
                NodeFactory.Text("Hello"),
                "Hello")
            .SetName("Render_TextNode");

        yield return new TestCaseData(
                NodeFactory.Header(NodeFactory.Text("Hello")),
                "<h1>Hello</h1>")
            .SetName("Render_HeaderNode");

        yield return new TestCaseData(
                NodeFactory.Paragraph(NodeFactory.Text("Hello")),
                "<p>Hello</p>")
            .SetName("Render_ParagraphNode");

        yield return new TestCaseData(
                NodeFactory.Emphasis(NodeFactory.Text("Hello")),
                "<em>Hello</em>")
            .SetName("Render_EmphasisNode");

        yield return new TestCaseData(
                NodeFactory.Strong(NodeFactory.Text("Hello")),
                "<strong>Hello</strong>")
            .SetName("Render_StrongNode");

        yield return new TestCaseData(
                NodeFactory.UnorderedList(NodeFactory.ListItem(NodeFactory.Text("Hello"))),
                "<ul><li>Hello</li></ul>")
            .SetName("Render_UnorderedListNode");
    }
}