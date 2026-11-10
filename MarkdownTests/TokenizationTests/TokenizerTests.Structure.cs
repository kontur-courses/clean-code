using FluentAssertions;
using Markdown.Extensions;
using Markdown.Paragraphs;
using Markdown.Tokenization.Tokens.Abstractions;
using Markdown.Tokenization.Tokens.Implementations;

namespace MarkdownTests.TokenizationTests;

public partial class TokenizationTests
{
    [Test]
    public void Tokenize_WithLineBreaks_ShouldCreateLineBreakTokens()
    {
        var source =
            """
        a
        b
        
        """;

        var tokens = Tokenize(source);

        tokens.Should().Equal(
            new TextToken(source, 0, 1),
            new LineBreakToken(source, 1),
            new TextToken(source, 3, 1),
            new LineBreakToken(source, 4));
    }

    [Test]
    public void Tokenize_WithRegularParagraph_ShouldNotCreateHeadingTokens()
    {
        var source = "# Обычный текст";

        var tokens = Tokenize(source);

        tokens.Should().Equal(
            new TextToken(source, 0, source.Length));
    }

    [Test]
    public void Tokenize_WithHeading_ShouldAddHeadingTokens()
    {
        var source = "# Заголовок";

        var tokens = Tokenize(source, isHeading: true);

        tokens.Should().Equal(
            new HeadingToken(false),
            new TextToken(source, 2, source.Length - 2),
            new HeadingToken(true));
    }

    [Test]
    public void Tokenize_WithMultilineHeading_ShouldKeepLineBreaksInsideHeading()
    {
        var source =
            """
        # Заголовок
        Продолжение
        """;

        var lineBreakStart = source.IndexOf("\r\n");

        var tokens = Tokenize(source, isHeading: true);

        tokens.Should().Equal(
            new HeadingToken(false),
            new TextToken(source, 2, lineBreakStart - 2),
            new LineBreakToken(source, lineBreakStart),
            new TextToken(source, lineBreakStart + 2,
                source.Length - lineBreakStart - 2),
            new HeadingToken(true));
    }

    [Test]
    public void Tokenize_WithHeadingAndTrailingLineBreaks_ShouldCloseHeadingLast()
    {
        var source = "# Заголовок\r\n\r\n";
        var lineBreakStart = source.IndexOf("\r\n");

        var tokens = Tokenize(source, isHeading: true);

        tokens.Should().Equal(
            new HeadingToken(false),
            new TextToken(source, 2, lineBreakStart - 2),
            new LineBreakToken(source, lineBreakStart),
            new LineBreakToken(source, lineBreakStart + 2),
            new HeadingToken(true));
    }

    [Test]
    public void Tokenize_WithLimitedParagraphRange_ShouldNotReadOutsideRange()
    {
        var source = "prefix _x_ suffix";
        var start = source.IndexOf('_');
        var paragraph = new Paragraph(source, start, start + 3, false);

        var tokens = paragraph.Tokenize().ToArray();

        tokens.Should().Equal(
            new EmToken(source, start),
            new TextToken(source, start + 1, 1),
            new EmToken(source, start + 2));
    }

    [Test]
    public void Tokenize_WithMultipleTokens_ShouldReferenceOriginalSource()
    {
        var source = "_текст_ [ссылка](url)";

        var tokens = Tokenize(source);

        foreach (var token in tokens.OfType<SourceToken>())
            token.Source.Should().BeSameAs(source);
    }
}
