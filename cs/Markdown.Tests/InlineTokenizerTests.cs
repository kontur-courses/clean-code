using FluentAssertions;
using static Markdown.InlineTokenizer;


namespace Markdown.Tests;

public class InlineTokenizerTests
{
    [TestCase("")]
    [TestCase("plain text")]
    [TestCase("___")]
    [TestCase("plain _ text")]
    public void Tokenize_NoValidTokens_ReturnsEmpty(string text)
    {
        Tokenize(text).Should().BeEmpty();
    }

    [TestCase(@"\_", 0)]
    [TestCase(@"\\", 0)]
    [TestCase(@"text\_", 4)]
    public void Tokenize_BackslashBeforeSpecialChar_ReturnsEscape(string text, int position)
    {
        Tokenize(text).Should().Equal(new EscapeToken(position));
    }

    [TestCase(@"p\lain")]
    [TestCase(@"plain\")]
    public void Tokenize_BackslashBeforeOrdinaryCharOrAtEnd_ReturnsEmpty(string text)
    {
        Tokenize(text).Should().BeEmpty();
    }


    [Test]
    public void Tokenize_EscapedUnderscore_IsNotDelimiter()
    {
        Tokenize(@"\_a").Should().Equal(new EscapeToken(0));
    }

    [Test]
    public void Tokenize_UnderscoreAfterEscape_IsDelimiter()
    {
        Tokenize(@"\__").Should().Equal(
            new EscapeToken(0),
            new DelimiterRunToken(2, 1, false, true));
    }

    [Test]
    public void Tokenize_EscapedBackslashBeforeUnderscore_UnderscoreIsDelimiter()
    {
        Tokenize(@"\\_a").Should().Equal(
            new EscapeToken(0),
            new DelimiterRunToken(2, 1, true, true));
    }

    [Test]
    public void Tokenize_ConsecutiveEscapes_ReturnsEachEscape()
    {
        Tokenize(@"\\\_").Should().Equal(
            new EscapeToken(0),
            new EscapeToken(2));
    }

    [Test]
    public void Tokenize_OddBackslashesAtEnd_LastOneIsIgnored()
    {
        Tokenize(@"\\\").Should().Equal(new EscapeToken(0));
    }

    [TestCase("a__b", 1, 2)]
    [TestCase("a___b", 1, 3)]
    public void Tokenize_UnderscoreRun_ReturnsSingleTokenWithRunLength(
        string text, int position, int length)
    {
        Tokenize(text).Should().Equal(
            new DelimiterRunToken(position, length, true, true));
    }

    [TestCase("_a", true, false)]
    [TestCase("a_", false, true)]
    [TestCase("a_b", true, true)]
    [TestCase("a _b", true, false)]
    [TestCase("a_ b", false, true)]
    public void Tokenize_SingleUnderscore_SetsFlagsByNeighbors(
        string text, bool canOpen, bool canClose)
    {
        var tokens = Tokenize(text);

        tokens.Should().ContainSingle()
            .Which.Should().BeOfType<DelimiterRunToken>()
            .Which.Should().Match<DelimiterRunToken>(
                t => t.CanOpen == canOpen && t.CanClose == canClose);
    }

    [Test]
    public void Tokenize_WithStartIndex_IgnoresTextBeforeIt()
    {
        Tokenize("_a_ b_", startIndex: 4).Should().Equal(
            new DelimiterRunToken(5, 1, false, true));
    }

    [Test]
    public void Tokenize_MixedText_ReturnsTokensInTextOrder()
    {
        Tokenize(@"_a\_b_").Should().Equal(
            new DelimiterRunToken(0, 1, true, false),
            new EscapeToken(2),
            new DelimiterRunToken(5, 1, false, true));
    }
}