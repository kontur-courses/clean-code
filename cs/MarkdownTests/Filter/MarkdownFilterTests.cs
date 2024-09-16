using Markdown.Filter.MarkdownFilters;
using Markdown.Tokens;
using MarkdownTests.Filter.TestCases;

namespace MarkdownTests.Filter;

[TestFixture]
public class MarkdownFilterTests
{
    private readonly MarkdownFilter filter = new ();

    [TestCaseSource(typeof(FilterTestCases), nameof(FilterTestCases.EmptyInputTests))]
    public List<Token> FilterTokens_ReturnsEmptyList_IfInputIsEmpty(string line, List<Token> tokens)
    {
        return filter.FilterTokens(tokens, line);
    }

    [TestCaseSource(typeof(FilterTestCases), nameof(FilterTestCases.EmptyLinesTests))]
    public List<Token> FilterTokens_ReturnsCorrectResult_OnEmptyLines(string line, List<Token> tokens)
    {
        return filter.FilterTokens(tokens, line);
    }

    [TestCaseSource(typeof(FilterTestCases), nameof(FilterTestCases.PartOfWordTests))]
    public List<Token> FilterTokens_ReturnsCorrectResult_OnTokensSurroundingPartsOfAWord(string line, List<Token> tokens)
    {
        return filter.FilterTokens(tokens, line);
    }

    [TestCaseSource(typeof(FilterTestCases), nameof(FilterTestCases.SpaceInterruptionTests))]
    public List<Token> FilterTokens_ReturnsCorrectResult_OnTokensInterruptedBySpace(string line, List<Token> tokens)
    {
        return filter.FilterTokens(tokens, line);
    }

    [TestCaseSource(typeof(FilterTestCases), nameof(FilterTestCases.BreakingNumbersTests))]
    public List<Token> FilterTokens_ReturnsCorrectResult_OnTokenBreakingANumber(string line, List<Token> tokens)
    {
        return filter.FilterTokens(tokens, line);
    }

    [TestCaseSource(typeof(FilterTestCases), nameof(FilterTestCases.DifferentWordsTests))]
    public List<Token> FilterTokens_ReturnsCorrectResult_OnTokensInDifferentWords(string line, List<Token> tokens)
    {
        return filter.FilterTokens(tokens, line);
    }

    [TestCaseSource(typeof(FilterTestCases), nameof(FilterTestCases.NestedTagsTests))]
    public List<Token> FilterTokens_ReturnsCorrectResult_WithNestedTags(string line, List<Token> tokens)
    {
        return filter.FilterTokens(tokens, line);
    }

    [TestCaseSource(typeof(FilterTestCases), nameof(FilterTestCases.IntersectingTagsTests))]
    public List<Token> FilterTokens_RemovesIncorrectTags_WhenTagsIntersect(string line, List<Token> tokens)
    {
        return filter.FilterTokens(tokens, line);
    }

    [TestCaseSource(typeof(FilterTestCases), nameof(FilterTestCases.UnpairedTagsTests))]
    public List<Token> FilterTokens_RemovesIncorrectTangs_WhenTagsAreUnpaired(string line, List<Token> tokens)
    {
        return filter.FilterTokens(tokens, line);
    }
}