using FluentAssertions;
using Markdown;

namespace MarkdownTests;

public class ParagraphSplittingTests
{
    [Test]
    public void GetParagraphs_WithSingleLine()
    {
        var source = "Обычный текст";

        var paragraphs = source.GetParagraphs().ToArray();

        paragraphs.Should().Equal(
            new Paragraph(source, 0, source.Length, false));
    }

    [Test]
    public void GetParagraphs_WithEmptyLine_ShouldBeEmpty()
    {
        var source = string.Empty;
        var paragraphs = source.GetParagraphs().ToArray();

        paragraphs.Should().BeEmpty();
    }

    [Test]
    public void GetParagraphs_WithSingleLineBreak_DoesNotSplitParagraph()
    {
        var source = """
            Первая строка
            Вторая строка
            """;

        var paragraphs = source.GetParagraphs().ToArray();

        paragraphs.Should().Equal(
            new Paragraph(source, 0, source.Length, false));
    }

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
}
