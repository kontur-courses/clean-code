using Markdown.Parsing;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class BlockParserTests
{
    private BlockParser parser = null!;

    [SetUp]
    public void SetUp()
    {
        parser = new BlockParser();
    }

    [Test]
    public void Parse_Paragraph_RendersParagraph()
    {
        Assert.That(parser.Parse("Обычный текст"), Is.EqualTo("<p>Обычный текст</p>"));
    }

    [Test]
    public void Parse_HeaderMarkerAtBeginning_RendersHeader()
    {
        Assert.That(parser.Parse("# Заголовок"), Is.EqualTo("<h1>Заголовок</h1>"));
    }

    [Test]
    public void Parse_HeaderWithInlineMarkup_RendersInlineNodesInsideHeader()
    {
        Assert.That(
            parser.Parse("# Заголовок __с _разной_ разметкой__"),
            Is.EqualTo("<h1>Заголовок <strong>с <em>разной</em> разметкой</strong></h1>"));
    }

    [TestCase("#Без пробела", "<p>#Без пробела</p>", TestName = "Parse_HashWithoutFollowingSpace_RendersParagraph")]
    [TestCase("## Заголовок", "<p>## Заголовок</p>", TestName = "Parse_TwoHashMarkers_RendersParagraph")]
    [TestCase(" # Заголовок", "<p> # Заголовок</p>", TestName = "Parse_IndentedHashMarker_RendersParagraph")]
    public void Parse_NonMatchingHeaderPrefix_RendersParagraph(string input, string expected)
    {
        Assert.That(parser.Parse(input), Is.EqualTo(expected));
    }

    [Test]
    public void Parse_EscapedHeaderMarker_RendersParagraphWithLiteralHash()
    {
        Assert.That(parser.Parse(@"\# Заголовок"), Is.EqualTo("<p># Заголовок</p>"));
    }

    [Test]
    public void Parse_BlocksSeparatedByBlankLines_RendersEachBlockInOrder()
    {
        const string input = "Первый абзац\n\n# Заголовок\n\nВторой абзац";

        Assert.That(
            parser.Parse(input),
            Is.EqualTo("<p>Первый абзац</p><h1>Заголовок</h1><p>Второй абзац</p>"));
    }

    [Test]
    public void Parse_EmptyText_ReturnsEmptyString()
    {
        Assert.IsEmpty(parser.Parse(string.Empty));
    }
}
