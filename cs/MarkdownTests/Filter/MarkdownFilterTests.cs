using Markdown.Filter;
using MarkdownTests.Filter.TestCases;

namespace MarkdownTests.Filter;

[TestFixture]
public class MarkdownFilterTests
{
    private MarkdownFilter filter = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        filter = new MarkdownFilter();
    }

    [TestCaseSource(typeof(FilterTestCases), nameof(FilterTestCases.EmptyInputTests))]
    public void FilterTokens_ReturnsEmptyList_IfInputIsEmpty(FilterTestData filterTestData)
    {
        AssertFilterTokensReturnsCorrectResult(filterTestData);
    }
    
    [TestCaseSource(typeof(FilterTestCases), nameof(FilterTestCases.EmptyLinesTests))]
    public void FilterTokens_ReturnsCorrectResult_OnEmptyLines(FilterTestData filterTestData)
    {
        AssertFilterTokensReturnsCorrectResult(filterTestData);
    }

    [TestCaseSource(typeof(FilterTestCases), nameof(FilterTestCases.PartOfWordTests))]
    public void FilterTokens_ReturnsCorrectResult_OnTokensSurroundingPartsOfAWord(FilterTestData filterTestData)
    {
        AssertFilterTokensReturnsCorrectResult(filterTestData);
    }

    [TestCaseSource(typeof(FilterTestCases), nameof(FilterTestCases.SpaceInterruptionTests))]
    public void FilterTokens_ReturnsCorrectResult_OnTokensInterruptedBySpace(FilterTestData filterTestData)
    {
        AssertFilterTokensReturnsCorrectResult(filterTestData);
    }

    [TestCaseSource(typeof(FilterTestCases), nameof(FilterTestCases.BreakingNumbersTests))]
    public void FilterTokens_ReturnsCorrectResult_OnTokenBreakingANumber(FilterTestData filterTestData)
    {
        AssertFilterTokensReturnsCorrectResult(filterTestData);
    }

    [TestCaseSource(typeof(FilterTestCases), nameof(FilterTestCases.DifferentWordsTests))]
    public void FilterTokens_ReturnsCorrectResult_OnTokensInDifferentWords(FilterTestData filterTestData)
    {
        AssertFilterTokensReturnsCorrectResult(filterTestData);
    }

    [TestCaseSource(typeof(FilterTestCases), nameof(FilterTestCases.NestedTagsTests))]
    public void FilterTokens_ReturnsCorrectResult_WithNestedTags(FilterTestData filterTestData)
    {
        AssertFilterTokensReturnsCorrectResult(filterTestData);
    }

    [TestCaseSource(typeof(FilterTestCases), nameof(FilterTestCases.IntersectingTagsTests))]
    public void FilterTokens_RemovesIncorrectTags_WhenTagsIntersect(FilterTestData filterTestData)
    {
        AssertFilterTokensReturnsCorrectResult(filterTestData);
    }
    
    [TestCaseSource(typeof(FilterTestCases), nameof(FilterTestCases.UnpairedTagsTests))]
    public void FilterTokens_RemovesIncorrectTangs_WhenTagsAreUnpaired(FilterTestData filterTestData)
    {
        AssertFilterTokensReturnsCorrectResult(filterTestData);
    }
    
    private void AssertFilterTokensReturnsCorrectResult(FilterTestData data)
    {
        var result = filter.FilterTokens(data.Tokens, data.Line);
        CollectionAssert.AreEqual(data.Expected, result);
    }
}