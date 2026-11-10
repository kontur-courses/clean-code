using FluentAssertions;
using Markdown.Tokenization.Tokens.Abstractions;
using Markdown.Tokenization.Tokens.Implementations;

namespace MarkdownTests.HtmlBuilderTests;

public partial class HtmlBuilderTests
{
    [Test]
    public void Append_WithMultilineHeading_ShouldCreateH1()
    {
        var source = "Title\r\nMore";

        Render(
            new HeadingToken(false),
            new TextToken(source, 0, 5),
            new LineBreakToken(source, 5),
            new TextToken(source, 7, 4),
            new HeadingToken(true))
            .Should().Be("<h1>Title<br>More</h1>");
    }

    [TestCase(0, "<h1>Head</h1>")]
    [TestCase(1, "<h1>Head</h1>")]
    [TestCase(2, "<h1>Head</h1><br>")]
    [TestCase(3, "<h1>Head</h1><br><br>")]
    public void Append_WithTrailingHeadingBreaks_ShouldMoveThemOutside(
        int breakCount, string expected)
    {
        var source = "Head" + string.Concat(
            Enumerable.Repeat("\r\n", breakCount));

        var tokens = new List<Token>
        {
            new HeadingToken(false),
            new TextToken(source, 0, 4)
        };

        for (var i = 0; i < breakCount; i++)
            tokens.Add(new LineBreakToken(source, 4 + i * 2));

        tokens.Add(new HeadingToken(true));

        Render(tokens.ToArray()).Should().Be(expected);
    }

    [Test]
    public void Append_WithLineBreaks_ShouldPreserveAllBreaks()
    {
        var source = "one\r\ntwo\r\n\r\n";

        Render(
            new TextToken(source, 0, 3),
            new LineBreakToken(source, 3),
            new TextToken(source, 5, 3),
            new LineBreakToken(source, 8),
            new LineBreakToken(source, 10))
            .Should().Be("one<br>two<br><br>");
    }

    [Test]
    public void Append_WithBreaksBeforeHeading_ShouldPreserveThem()
    {
        var source = "\r\n\r\n\r\nHeading";

        Render(
            new LineBreakToken(source, 0),
            new LineBreakToken(source, 2),
            new LineBreakToken(source, 4),
            new HeadingToken(false),
            new TextToken(source, 6, 7),
            new HeadingToken(true))
            .Should().Be("<br><br><br><h1>Heading</h1>");
    }
}
