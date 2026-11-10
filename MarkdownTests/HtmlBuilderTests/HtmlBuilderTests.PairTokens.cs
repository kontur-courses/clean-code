using FluentAssertions;
using Markdown.Tokenization.Tokens;
using Markdown.Tokenization.Tokens.Implementations;

namespace MarkdownTests.HtmlBuilderTests;

public partial class HtmlBuilderTests
{
    [TestCase(false, "<em>text</em>")]
    [TestCase(true, "<strong>text</strong>")]
    public void Append_WithPair_ShouldCreateHtmlTags(bool isStrong, string expected)
    {
        var source = isStrong ? "__text__" : "_text_";
        var markerLength = isStrong ? 2 : 1;

        PairToken opening = isStrong
            ? new StrongToken(source, 0)
            : new EmToken(source, 0);

        PairToken closing = isStrong
            ? new StrongToken(source, source.Length - markerLength)
            : new EmToken(source, source.Length - markerLength);

        opening.Context = new ResolvingContext(PairRole.Open, 0);
        closing.Context = new ResolvingContext(PairRole.Close, 0);

        Render(
            opening,
            new TextToken(source, markerLength, 4),
            closing)
            .Should().Be(expected);
    }

    [TestCase(false, "_")]
    [TestCase(true, "__")]
    public void Append_WithUnpairedToken_ShouldPreserveUnderscores(bool strong, string expected)
    {
        PairToken token = strong
            ? new StrongToken(expected, 0)
            : new EmToken(expected, 0);

        token.Context = new ResolvingContext(PairRole.Text, 0);

        Render(token).Should().Be(expected);
    }
}
