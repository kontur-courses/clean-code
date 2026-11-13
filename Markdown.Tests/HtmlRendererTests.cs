using System.Text;
using NUnit.Framework;
using Markdown.Rendering;
using Markdown.Syntax;

namespace Markdown.Tests;

public class HtmlRendererTests
{

    private HtmlRenderer renderer = null!;

    [SetUp]
    public void SetUp()
    {
        renderer = new HtmlRenderer();
    }

    [Test]
    public void RenderBlock_Paragraph_RendersNestedInlineNodes()
    {
        var output = new StringBuilder();

        renderer.RenderBlock(
            output,
            [
                Text("обычный "),
                Emphasis(Text("курсив")),
                Text(" и "),
                Strong(Text("полужирный "), Emphasis(Text("курсивный")))
            ],
            HtmlBlockKind.Paragraph);

        Assert.That(
            output.ToString(),
            Is.EqualTo("<p>обычный <em>курсив</em> и <strong>полужирный <em>курсивный</em></strong></p>"));
    }

    [Test]
    public void RenderBlock_Header_RendersRequestedLevelAndInlineNodes()
    {
        var output = new StringBuilder();

        renderer.RenderBlock(
            output,
            [Text("Заголовок "), Strong(Text("жирный"))],
            HtmlBlockKind.Header,
            headerLevel: 1);

        Assert.That(output.ToString(), Is.EqualTo("<h1>Заголовок <strong>жирный</strong></h1>"));
    }

    [Test]
    public void RenderBlock_Paragraph_EscapesHtmlCharactersInTextNodes()
    {
        var output = new StringBuilder();

        renderer.RenderBlock(output, [Text("a & b < c > d")], HtmlBlockKind.Paragraph);

        Assert.That(output.ToString(), Is.EqualTo("<p>a &amp; b &lt; c &gt; d</p>"));
    }

    [Test]
    public void RenderBlock_MarkedList_RendersEachItemAndItsInlineNodes()
    {
        var output = new StringBuilder();
        IReadOnlyList<IReadOnlyList<InlineNode>> items =
        [
            [Text("Первый пункт")],
            [Text("Второй "), Emphasis(Text("курсивный")), Text(" пункт")]
        ];

        renderer.RenderBlock(
            output,
            content: null,
            blockKind: HtmlBlockKind.MarkedList,
            listItems: items);

        Assert.That(
            output.ToString(),
            Is.EqualTo("<ul><li>Первый пункт</li><li>Второй <em>курсивный</em> пункт</li></ul>"));
    }

    [TestCase(0)]
    [TestCase(7)]
    public void RenderBlock_Header_InvalidLevel_ThrowsArgumentOutOfRangeException(int level)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            renderer.RenderBlock(
                new StringBuilder(),
                [Text("Заголовок")],
                HtmlBlockKind.Header,
                level));
    }

    private static TextNode Text(string text) => new(text);

    private static EmphasisNode Emphasis(params InlineNode[] children) => new(children);

    private static StrongNode Strong(params InlineNode[] children) => new(children);
}
