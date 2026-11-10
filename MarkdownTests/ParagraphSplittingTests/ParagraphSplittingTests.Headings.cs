using FluentAssertions;
using Markdown.Extensions;
using Markdown.Paragraphs;

namespace MarkdownTests.ParagraphSplittingTests;

public partial class ParagraphSplittingTests
{
    [Test]
    public void GetParagraphs_WithHashOnSecondLine_DoesNotMarkAsHeading()
    {
        var source =
            """
            Обычный текст
            # Не заголовок
            """;

        var paragraphs = source.GetParagraphs().ToArray();

        paragraphs.Should().Equal(
            new Paragraph(source, 0, source.Length, false));
    }

    [Test]
    public void GetParagraphs_WithMultilineHeading_ShouldMarkAsHeading()
    {
        var source =
            """
            # Заголовок
            Продолжение заголовка
            """;

        var paragraphs = source.GetParagraphs().ToArray();

        paragraphs.Should().Equal(
            new Paragraph(source, 0, source.Length, true));
    }

    [Test]
    public void GetParagraphs_WithHeadingAndRegularParagraph_ShouldDetectHeading()
    {
        var heading =
            """
            # Заголовок
            

            """;
        var source = heading + "Обычный текст";

        var paragraphs = source.GetParagraphs().ToArray();

        paragraphs.Should().Equal(
            new Paragraph(source, 0, heading.Length, true),
            new Paragraph(source, heading.Length, source.Length, false));
    }
}
