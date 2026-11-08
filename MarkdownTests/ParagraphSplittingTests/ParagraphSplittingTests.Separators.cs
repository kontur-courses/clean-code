using FluentAssertions;
using Markdown.Extensions;
using Markdown.Paragraphs;

namespace MarkdownTests.ParagraphSplittingTests;

public partial class ParagraphSplittingTests
{
    [TestCase(
    """
    Первый абзац

    Второй абзац
    """, 2)]
    [TestCase(
    """
    Первый абзац



    Второй абзац
    """, 2)]
    [TestCase(
    """
    Первый абзац

    Второй абзац


    Третий абзац
    """, 3)]
    public void GetParagraphs_WithEmptyLineBetweenText_SplitsParagraphs(string source, int paragraphsCount)
    {
        var paragraphs = source.GetParagraphs().ToArray();

        paragraphs.Length.Should().Be(paragraphsCount);
    }

    [Test]
    public void GetParagraphs_WithMultipleEmptyLines_ShouldBelongToPreviousParagraph()
    {
        var first =
            """
        Первый


        """;
        var second = """Второй""";
        var source = first + second;

        var paragraphs = source.GetParagraphs().ToArray();

        paragraphs.Should().Equal(
            new Paragraph(source, 0, first.Length, false),
            new Paragraph(source, first.Length, source.Length, false));
    }

    [Test]
    public void GetParagraphs_WithWhitespaceOnlyLine_DoesNotSplitParagraph()
    {
        var source =
            """
        Первая строка
                   
        Вторая строка
        """; //теперь между абзацами пустая строка, но с пробелами

        var paragraphs = source.GetParagraphs().ToArray();

        paragraphs.Should().Equal(
            new Paragraph(source, 0, source.Length, false));
    }

    [Test]
    public void GetParagraphs_WithLeadingEmptyLines_ShouldSkipThem()
    {
        var source =
            """


        Обычный текст
        """;
        var start = 4; // \r\n\r\n - 4 символа пропускаются

        var paragraphs = source.GetParagraphs().ToArray();

        paragraphs.Should().Equal(
            new Paragraph(source, start, source.Length, false));
    }

    [TestCase("""

    """)]
    [TestCase("""


    """)]
    public void GetParagraphs_WithOnlyLineBreaks_ShouldBeEmpty(string source)
    {
        var paragraphs = source.GetParagraphs().ToArray();

        paragraphs.Should().BeEmpty();
    }
}
