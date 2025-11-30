using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class MdTests
{
    private Md markdown;

    [SetUp]
    public void SetUp()
    {
        markdown = new Md();
    }

    [TestCase("wordA wordB")]
    public void Render_ReturnsString_WhenTextWithoutTags(string input)
    {
        var result = markdown.Render(input);

        result.Should().Be("wordA wordB");
    }

    [TestCaseSource(nameof(CasesWhenTextWithOnePairedTag))]
    [Description("Checks each paired tag")]
    public void Render_ReturnsString_WhenTextWithOnePairedTag(string input, string expectedResult)
    {
        var result = markdown.Render(input);

        result.Should().Be(expectedResult);
    }

    [TestCaseSource(nameof(CasesWhenTextWithMultipleNonNestedPairedTags))]
    public void Render_ReturnsString_WhenTextWithMultipleNonNestedPairedTags(string input, string expectedResult)
    {
        var result = markdown.Render(input);

        result.Should().Be(expectedResult);
    }

    [TestCaseSource(nameof(CasesWhenTextWithMultipleNestedPairedTags))]
    public void Render_ReturnsString_WhenTextWithMultipleNestedPairedTags(string input, string expectedResult)
    {
        var result = markdown.Render(input);

        result.Should().Be(expectedResult);
    }

    [TestCaseSource(nameof(CasesWhenTextWithPairedTagWithoutPair))]
    [Description("Checks each paired tag")]
    public void Render_ReturnsString_WhenTextWithPairedTagWithoutPair(string input, string expectedResult)
    {
        var result = markdown.Render(input);

        result.Should().Be(expectedResult);
    }

    [TestCaseSource(nameof(CasesWhenEmptyTextInsideTags))]
    public void Render_ReturnsString_WhenEmptyTextInsideTags(string input, string expectedResult)
    {
        var result = markdown.Render(input);

        result.Should().Be(expectedResult);
    }

    [TestCaseSource(nameof(CasesTextContainsHeaderTag))]
    public void Render_ReturnsString_WhenTextContainsHeaderTag(string input, string expectedResult)
    {
        var result = markdown.Render(input);

        result.Should().Be(expectedResult);
    }

    [TestCaseSource(nameof(CasesWhenTextContainsEscapingTag))]
    public void Render_ReturnsString_WhenTextContainsEscapingTag(string input, string expectedResult)
    {
        var result = markdown.Render(input);

        result.Should().Be(expectedResult);
    }

    [TestCaseSource(nameof(CasesWhenTextContainsOverlappingTags))]
    public void Render_ReturnsString_WhenTextContainsOverlappingTags(string input, string expectedResult)
    {
        var result = markdown.Render(input);

        result.Should().Be(expectedResult);
    }

    [TestCaseSource(nameof(CasesWhenBoldTagInsideItalicTag))]
    public void Render_ReturnsString_WhenBoldTagInsideItalicTag(string input, string expectedResult)
    {
        var result = markdown.Render(input);

        result.Should().Be(expectedResult);
    }

    [TestCaseSource(nameof(CasesWhenItalicTagInsideBoldTag))]
    public void Render_ReturnsString_WhenItalicTagInsideBoldTag(string input, string expectedResult)
    {
        var result = markdown.Render(input);

        result.Should().Be(expectedResult);
    }

    [TestCaseSource(nameof(CasesWhenTextWithNumbersAndContainsBoldItalicTags))]
    public void Render_ReturnsString_WhenTextWithNumbersAndContainsBoldItalicTags(string input, string expectedResult)
    {
        var result = markdown.Render(input);

        result.Should().Be(expectedResult);
    }

    [TestCaseSource(nameof(CasesWhenTextWithWhiteSpaceAndContainsBoldItalicTags))]
    public void Render_ReturnsString_WhenTextWithWhiteSpaceAndContainsBoldItalicTags(string input, string expectedResult)
    {
        var result = markdown.Render(input);

        result.Should().Be(expectedResult);
    }

    [TestCaseSource(nameof(CasesWhenTextContainsBoldItalicTagsInMiddleWords))]
    public void Render_ReturnsString_WhenTextContainsBoldItalicTagsInMiddleWords(string input, string expectedResult)
    {
        var result = markdown.Render(input);

        result.Should().Be(expectedResult);
    }
    
    [TestCaseSource(nameof(CasesWhenTextContainsLinkTag))]
    public void Parse_ReturnsIEnumerableTokens_WhenTextContainsLinkTag(string input, string expectedResult)
    {
        var result = markdown.Render(input);

        result.Should().Be(expectedResult);
    }

    public static IEnumerable<TestCaseData> CasesWhenTextWithOnePairedTag()
    {
        yield return new TestCaseData("_wordA wordB_", "<em>wordA wordB</em>");
        yield return new TestCaseData("__wordA wordB__", "<strong>wordA wordB</strong>");
        yield return new TestCaseData("wordA _wordB wordC_ wordD", "wordA <em>wordB wordC</em> wordD");
        yield return new TestCaseData("wordA __wordB wordC__ wordD", "wordA <strong>wordB wordC</strong> wordD");
    }

    public static IEnumerable<TestCaseData> CasesWhenTextWithMultipleNonNestedPairedTags()
    {
        yield return new TestCaseData("_wordA_ __wordB__ wordC", "<em>wordA</em> <strong>wordB</strong> wordC");
    }

    public static IEnumerable<TestCaseData> CasesWhenTextWithMultipleNestedPairedTags()
    {
        yield return new TestCaseData("__wordA _wordB_ wordC__", "<strong>wordA <em>wordB</em> wordC</strong>");
    }

    public static IEnumerable<TestCaseData> CasesWhenTextWithPairedTagWithoutPair()
    {
        yield return new TestCaseData("_wordA", "_wordA");
        yield return new TestCaseData("__wordA", "__wordA");
    }

    public static IEnumerable<TestCaseData> CasesWhenEmptyTextInsideTags()
    {
        yield return new TestCaseData("__", "__");
        yield return new TestCaseData("____", "____");
    }

    public static IEnumerable<TestCaseData> CasesTextContainsHeaderTag()
    {
        yield return new TestCaseData("# wordA", "<h1>wordA</h1>");
        yield return new TestCaseData("# wordA # ", "<h1>wordA # </h1>");
        yield return new TestCaseData(" # wordA", " # wordA");
        yield return new TestCaseData($"# wordA{Environment.NewLine} # ", $"<h1>wordA</h1>{Environment.NewLine} # ");
        yield return new TestCaseData($" wordA{Environment.NewLine}{Environment.NewLine}# wordB", $" wordA{Environment.NewLine}{Environment.NewLine}<h1>wordB</h1>");
    }

    public static IEnumerable<TestCaseData> CasesWhenTextContainsEscapingTag()
    {
        yield return new TestCaseData(@"\\", @"\\");
        yield return new TestCaseData(@"\_wordA_", @"\_wordA_");
    }

    public static IEnumerable<TestCaseData> CasesWhenTextContainsOverlappingTags()
    {
        yield return new TestCaseData("__wordA_wordB__wordC_", "__wordA_wordB__wordC_");
        yield return new TestCaseData("_wordA__wordB_wordC__ wordD", "_wordA__wordB_wordC__ wordD");
    }

    public static IEnumerable<TestCaseData> CasesWhenBoldTagInsideItalicTag()
    {
        yield return new TestCaseData("_wordA__wordB__wordC_", "<em>wordA__wordB__wordC</em>");
    }

    public static IEnumerable<TestCaseData> CasesWhenItalicTagInsideBoldTag()
    {
        yield return new TestCaseData("__wordA_wordB_wordC__", "<strong>wordA<em>wordB</em>wordC</strong>");
    }

    public static IEnumerable<TestCaseData> CasesWhenTextWithNumbersAndContainsBoldItalicTags()
    {
        yield return new TestCaseData("wo__rd1__", "wo__rd1__");
        yield return new TestCaseData("wo_rd1_", "wo_rd1_");
    }

    public static IEnumerable<TestCaseData> CasesWhenTextWithWhiteSpaceAndContainsBoldItalicTags()
    {
        yield return new TestCaseData("_wordA _", "_wordA _");
        yield return new TestCaseData("__wordA __", "__wordA __");
        yield return new TestCaseData("_ wordA_", "_ wordA_");
        yield return new TestCaseData("__ wordA__", "__ wordA__");
    }

    public static IEnumerable<TestCaseData> CasesWhenTextContainsBoldItalicTagsInMiddleWords()
    {
        yield return new TestCaseData("_wor_dA", "<em>wor</em>dA");
        yield return new TestCaseData("__wor__dA", "<strong>wor</strong>dA");
        yield return new TestCaseData("_wordA wor_dB", "_wordA wor_dB");
        yield return new TestCaseData("__wordA wor__dB", "__wordA wor__dB");
    }
    public static IEnumerable<TestCaseData> CasesWhenTextContainsLinkTag()
    {
        yield return new TestCaseData("[Name Link](https://www.example.com \"Tooltip\")",
            "<a href=\"https://www.example.com\" title=\"Tooltip\">Name Link</a>");
        yield return new TestCaseData("[Name Link](https://www.example.com Tooltip\")",
            "<a href=\"https://www.example.com Tooltip\"\">Name Link</a>");
        yield return new TestCaseData("Name Link](https://www.example.com Tooltip\")",
            "Name Link](https://www.example.com Tooltip\")");
        yield return new TestCaseData("[Name Link(https://www.example.com Tooltip\")",
            "[Name Link(https://www.example.com Tooltip\")");
        yield return new TestCaseData("[Name Link]https://www.example.com Tooltip\")",
            "[Name Link]https://www.example.com Tooltip\")");
        yield return new TestCaseData("[Name Link](https://www.example.com Tooltip\"",
            "[Name Link](https://www.example.com Tooltip\"");
    }
    
}