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

    [TestCase("# Заголовок", "<h1>Заголовок</h1>", TestName = "Parse_HeaderMarkerAtBeginning_RendersHeader")]
    [TestCase("# Заголовок __с _разной_ разметкой__", "<h1>Заголовок <strong>с <em>разной</em> разметкой</strong></h1>", TestName = "Parse_HeaderWithInlineMarkup_RendersInlineNodesInsideHeader")]
    public void Parse_HeaderInput_RendersHeader(string input, string expected)
    {
        Assert.That(parser.Parse(input), Is.EqualTo(expected));
    }

    [TestCase("#Без пробела", "<p>#Без пробела</p>", TestName = "Parse_HashWithoutFollowingSpace_RendersParagraph")]
    [TestCase("## Заголовок", "<p>## Заголовок</p>", TestName = "Parse_TwoHashMarkers_RendersParagraph")]
    [TestCase(" # Заголовок", "<p> # Заголовок</p>", TestName = "Parse_IndentedHashMarker_RendersParagraph")]
    [TestCase(@"\# Заголовок", "<p># Заголовок</p>", TestName = "Parse_EscapedHeaderMarker_RendersParagraphWithLiteralHash")]
    public void Parse_NonMatchingHeaderPrefix_RendersParagraph(string input, string expected)
    {
        Assert.That(parser.Parse(input), Is.EqualTo(expected));
    }

    [Test]
    public void Parse_BlocksSeparatedByBlankLines_RendersEachBlockInOrder()
    {
        const string input = "Первый абзац\n\n# Заголовок\n\nВторой абзац";

        Assert.That(
            parser.Parse(input),
            Is.EqualTo("<p>Первый абзац</p><h1>Заголовок</h1><p>Второй абзац</p>"));
    }

    [TestCase("- Первый пункт", "<ul><li>Первый пункт</li></ul>", TestName = "Parse_SingleMarkedListItem_RendersUnorderedList")]
    [TestCase("- ", "<ul><li></li></ul>", TestName = "Parse_EmptyMarkedListItem_RendersUnorderedList")]
    [TestCase("- Первый пункт\n- Второй _курсивный_ пункт\n- Третий __жирный__ пункт", "<ul><li>Первый пункт</li><li>Второй <em>курсивный</em> пункт</li><li>Третий <strong>жирный</strong> пункт</li></ul>", TestName = "Parse_ConsecutiveMarkedListItemsWithInlineMarkup_RendersOneList")]
    [TestCase("- Пункт списка\n\nОбычный абзац", "<ul><li>Пункт списка</li></ul><p>Обычный абзац</p>", TestName = "Parse_BlankLineAfterMarkedList_RendersFollowingParagraphSeparately")]
    [TestCase("- Пункт списка\nОбычный абзац", "<ul><li>Пункт списка</li></ul><p>Обычный абзац</p>", TestName = "Parse_NonListLineAfterMarkedList_RendersFollowingParagraphSeparately")]
    [TestCase("- Первый список\nОбычный абзац\n- Второй список", "<ul><li>Первый список</li></ul><p>Обычный абзац</p><ul><li>Второй список</li></ul>", TestName = "Parse_NonListLineBetweenMarkedLists_RendersThreeSeparateBlocks")]
    [TestCase("Обычный абзац\n- Пункт списка", "<p>Обычный абзац</p><ul><li>Пункт списка</li></ul>", TestName = "Parse_ParagraphBeforeMarkedListWithoutBlankLine_RendersSeparateBlocks")]
    [TestCase("- Первый список\n\n- Второй список", "<ul><li>Первый список</li></ul><ul><li>Второй список</li></ul>", TestName = "Parse_BlankLineBetweenMarkedLists_RendersSeparateLists")]
    [TestCase("- Пункт списка\n# Заголовок", "<ul><li>Пункт списка</li></ul><h1>Заголовок</h1>", TestName = "Parse_MarkedListFollowedByHeaderWithoutBlankLine_RendersSeparateBlocks")]
    [TestCase("-Пункт списка", "<p>-Пункт списка</p>", TestName = "Parse_DashWithoutFollowingSpace_RendersParagraph")]
    [TestCase(" - Пункт списка", "<p> - Пункт списка</p>", TestName = "Parse_IndentedDash_RendersParagraph")]
    [TestCase(@"\- Пункт списка", "<p>- Пункт списка</p>", TestName = "Parse_EscapedDash_RendersLiteralDash")]
    public void Parse_MarkedListInput_RendersExpectedHtml(string input, string expected)
    {
        Assert.That(parser.Parse(input), Is.EqualTo(expected));
    }

    [Test]
    public void Parse_EmptyText_ReturnsEmptyString()
    {
        Assert.IsEmpty(parser.Parse(string.Empty));
    }
}
