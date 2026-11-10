using FluentAssertions;
using Markdown.Extensions;
using Markdown.Paragraphs;

namespace MarkdownTests.ParagraphSplittingTests;

public partial class ParagraphSplittingTests
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
        var source =
            """
            Первая строка
            Вторая строка
            """;

        var paragraphs = source.GetParagraphs().ToArray();

        paragraphs.Should().Equal(
            new Paragraph(source, 0, source.Length, false));
    }
}
