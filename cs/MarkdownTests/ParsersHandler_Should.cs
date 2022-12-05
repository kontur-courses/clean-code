using FluentAssertions;
using Markdown;

namespace MarkdownTests;

public class ParsersHandler_Should
{
    private readonly Parser _handler = new();

    [Test]
    public void WhenNoSpecialSymbols_ReturnsInitialString()
    {
        _handler.Parse("ABC").Should().Be("ABC");
    }

    [TestCase("_AB_C", "<em>AB</em>C", TestName = "ForPaired_ReturnsConverted1")]
    [TestCase("_ABC", "_ABC", TestName = "ForUnpaired_ReturnsNotConverted1")]
    [TestCase("_A B_C", "_A B_C", TestName = "WhenHighlightsDifferentWords_ReturnsNotConverted1")]
    [TestCase("_ ABC_", "_ ABC_", TestName = "WhenSpaceAfterStartOfHighlighting_ReturnsNotConverted1")]
    [TestCase("_ABC _K_", "_ABC <em>K</em>", TestName = "WhenSpaceBeforeEndOfHighlighting_ReturnsNotConverted1")]
    [TestCase("__", "__", TestName = "WhenHighlightsEmptyString_ReturnsNotConverted1")]
    public void WhenHasUnderscores(string input, string expected)
    {
        _handler.Parse(input).Should().Be(expected);
    }

    [TestCase("__AB__C", "<strong>AB</strong>C", TestName = "ForPaired_ReturnsConverted")]
    [TestCase("__ABC", "__ABC", TestName = "ForUnpaired_ReturnsNotConverted")]
    [TestCase("__A B__C", "__A B__C", TestName = "WhenHighlightsDifferentWords_ReturnsNotConverted")]
    [TestCase("__ ABC__", "__ ABC__", TestName = "WhenSpaceAfterStartOfHighlighting_ReturnsNotConverted")]
    [TestCase("__ABC __K__", "__ABC <strong>K</strong>",
        TestName = "WhenSpaceBeforeEndOfHighlighting_ReturnsNotConverted")]
    [TestCase("____", "____", TestName = "WhenHighlightsEmptyString_ReturnsNotConverted")]
    public void WhenHasDoubleUnderscores(string input, string expected)
    {
        _handler.Parse(input).Should().Be(expected);
    }

    [TestCase("__A_B_C__", "<strong>A<em>B</em>C</strong>")]
    public void WhenHasNesting(string input, string expected)
    {
        _handler.Parse(input).Should().Be(expected);
    }

    [TestCase("__A_B__C_", "__A_B__C_", TestName = "ForDoubleUnderscoresAndUnderscores")]
    [TestCase("_A__B_C__", "_A__B_C__", TestName = "ForUnderscoresAndDoubleUnderscores")]
    public void WhenHasIntersections_ReturnsNotConverted(string input, string expected)
    {
        _handler.Parse(input).Should().Be(expected);
    }

    [TestCase("/_ABC_", "_ABC_", TestName = "WhenEscapingUnderscore_ReturnsNotConverted")]
    [TestCase("//_ABC_", "/<em>ABC</em>", TestName = "WhenEscapingSlash_ReturnsConverted")]
    public void WhenHasEscaping(string input, string expected)
    {
        _handler.Parse(input).Should().Be(expected);
    }

    [TestCase("# ABC DEF ", "<h1> ABC DEF </h1>")]
    public void WhenHasHash(string input, string expected)
    {
        _handler.Parse(input).Should().Be(expected);
    }
}