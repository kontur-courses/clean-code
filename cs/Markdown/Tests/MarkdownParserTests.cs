using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class MarkdownParserTests
{
    private static readonly Dictionary<TagType, string> pairedTags = new()
    {
        { TagType.Italic, "_" },
        { TagType.Bold, "__" }
    };

    private IParser parser;

    [SetUp]
    public void SetUp()
    {
        parser = new MarkdownParser();
    }

    [TestCase("wordA wordB")]
    public void Parse_ReturnsIEnumerableTokens_WhenTextWithoutTags(string input)
    {
        var result = parser.Parse(input);

        result.Should().BeEquivalentTo([new Token(TagType.None, "wordA wordB")]);
    }

    [TestCaseSource(nameof(CasesWhenTextWithOnePairedTag))]
    [Description("Checks each paired tag")]
    public void Parse_ReturnsIEnumerableTokens_WhenTextWithOnePairedTag(string input, IEnumerable<Token> expectedResult)
    {
        var result = parser.Parse(input);

        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrdering());
    }

    [TestCaseSource(nameof(CasesWhenTextWithMultipleNonNestedPairedTags))]
    public void Parse_ReturnsIEnumerableTokens_WhenTextWithMultipleNonNestedPairedTags(string input,
        IEnumerable<Token> expectedResult)
    {
        var result = parser.Parse(input);

        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrdering());
    }

    [TestCaseSource(nameof(CasesWhenTextWithMultipleNestedPairedTags))]
    public void Parse_ReturnsIEnumerableTokens_WhenTextWithMultipleNestedPairedTags(string input,
        IEnumerable<Token> expectedResult)
    {
        var result = parser.Parse(input);

        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrdering());
    }

    [TestCaseSource(nameof(CasesWhenTextWithPairedTagWithoutPair))]
    [Description("Checks each paired tag")]
    public void Parse_ReturnsIEnumerableTokens_WhenTextWithPairedTagWithoutPair(string input,
        IEnumerable<Token> expectedResult)
    {
        var result = parser.Parse(input);

        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrdering());
    }

    [TestCaseSource(nameof(CasesWhenEmptyTextInsideTags))]
    public void Parse_ReturnsIEnumerableTokens_WhenEmptyTextInsideTags(string input, IEnumerable<Token> expectedResult)
    {
        var result = parser.Parse(input);

        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrdering());
    }

    [TestCaseSource(nameof(CasesTextContainsHeaderTag))]
    public void Parse_ReturnsIEnumerableTokens_WhenTextContainsHeaderTag(string input,
        IEnumerable<Token> expectedResult)
    {
        var result = parser.Parse(input);

        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrdering());
    }

    [TestCaseSource(nameof(CasesWhenTextContainsEscapingTag))]
    public void Parse_ReturnsIEnumerableTokens_WhenTextContainsEscapingTag(string input,
        IEnumerable<Token> expectedResult)
    {
        var result = parser.Parse(input);

        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrdering());
    }

    [TestCaseSource(nameof(CasesWhenTextContainsOverlappingTags))]
    public void Parse_ReturnsIEnumerableTokens_WhenTextContainsOverlappingTags(string input,
        IEnumerable<Token> expectedResult)
    {
        var result = parser.Parse(input);

        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrdering());
    }

    [TestCaseSource(nameof(CasesWhenBoldTagInsideItalicTag))]
    public void Parse_ReturnsIEnumerableTokens_WhenBoldTagInsideItalicTag(string input,
        IEnumerable<Token> expectedResult)
    {
        var result = parser.Parse(input);

        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrdering());
    }

    [TestCaseSource(nameof(CasesWhenItalicTagInsideBoldTag))]
    public void Parse_ReturnsIEnumerableTokens_WhenItalicTagInsideBoldTag(string input,
        IEnumerable<Token> expectedResult)
    {
        var result = parser.Parse(input);

        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrdering());
    }

    [TestCaseSource(nameof(CasesWhenTextWithNumbersAndContainsBoldItalicTags))]
    public void Parse_ReturnsIEnumerableTokens_WhenTextWithNumbersAndContainsBoldItalicTags(string input,
        IEnumerable<Token> expectedResult)
    {
        var result = parser.Parse(input);

        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrdering());
    }

    [TestCaseSource(nameof(CasesWhenTextWithWhiteSpaceAndContainsBoldItalicTags))]
    public void Parse_ReturnsIEnumerableTokens_WhenTextWithWhiteSpaceAndContainsBoldItalicTags(string input,
        IEnumerable<Token> expectedResult)
    {
        var result = parser.Parse(input);

        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrdering());
    }

    [TestCaseSource(nameof(CasesWhenTextContainsBoldItalicTagsInMiddleWords))]
    public void Parse_ReturnsIEnumerableTokens_WhenTextContainsBoldItalicTagsInMiddleWords(string input,
        IEnumerable<Token> expectedResult)
    {
        var result = parser.Parse(input);

        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrdering());
    }
    
    [TestCaseSource(nameof(CasesWhenTextContainsLinkTag))]
    public void Parse_ReturnsIEnumerableTokens_WhenTextContainsLinkTag(string input,
        IEnumerable<Token> expectedResult)
    {
        var result = parser.Parse(input);

        result.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrdering());
    }

    public static IEnumerable<TestCaseData> CasesWhenTextWithOnePairedTag()
    {
        foreach (var (tagType, tagContent) in pairedTags)
        {
            yield return new TestCaseData($"{tagContent}wordA wordB{tagContent}",
                new[] { new Token(tagType, "wordA wordB") });
            yield return new TestCaseData($"wordA {tagContent}wordB wordC{tagContent} wordD",
                new[]
                {
                    new Token(TagType.None, "wordA "), new Token(tagType, "wordB wordC"),
                    new Token(TagType.None, " wordD")
                });
        }
    }

    public static IEnumerable<TestCaseData> CasesWhenTextWithMultipleNonNestedPairedTags()
    {
        yield return new TestCaseData("_wordA_ __wordB__ wordC",
            new[]
            {
                new Token(TagType.Italic, "wordA"), new Token(TagType.None, " "), new Token(TagType.Bold, "wordB"),
                new Token(TagType.None, " wordC")
            });
    }

    public static IEnumerable<TestCaseData> CasesWhenTextWithMultipleNestedPairedTags()
    {
        yield return new TestCaseData("__wordA _wordB_ wordC__",
            new[]
            {
                new Token(TagType.Bold, "wordA _wordB_ wordC",
                [
                    new Token(TagType.None, "wordA "), new Token(TagType.Italic, "wordB"),
                    new Token(TagType.None, " wordC")
                ])
            });
    }

    public static IEnumerable<TestCaseData> CasesWhenTextWithPairedTagWithoutPair()
    {
        foreach (var (tagType, tagContent) in pairedTags)
            yield return new TestCaseData($"{tagContent}wordA",
                new[] { new Token(TagType.None, $"{tagContent}wordA") });
    }

    public static IEnumerable<TestCaseData> CasesWhenEmptyTextInsideTags()
    {
        yield return new TestCaseData("__", new[] { new Token(TagType.None, "__") });
        yield return new TestCaseData("____", new[] { new Token(TagType.None, "____") });
    }

    public static IEnumerable<TestCaseData> CasesTextContainsHeaderTag()
    {
        yield return new TestCaseData("# wordA", new[] { new Token(TagType.Header, "wordA") });
        yield return new TestCaseData("# wordA # ", new[] { new Token(TagType.Header, "wordA # ") });
        yield return new TestCaseData(" # wordA", new[] { new Token(TagType.None, " # wordA") });

        yield return new TestCaseData($"# wordA{Environment.NewLine} # ",
            new[]
            {
                new Token(TagType.Header, "wordA"),
                new Token(TagType.None, $"{Environment.NewLine} # ")
            });

        yield return new TestCaseData($" wordA{Environment.NewLine}{Environment.NewLine}# wordB",
            new[]
            {
                new Token(TagType.None, $" wordA{Environment.NewLine}{Environment.NewLine}"), new Token(TagType.Header, "wordB")
            });
    }

    public static IEnumerable<TestCaseData> CasesWhenTextContainsEscapingTag()
    {
        yield return new TestCaseData(@"\\", new[] { new Token(TagType.Escaping, @"\") });
        yield return new TestCaseData(@"\_wordA_",
            new[] { new Token(TagType.Escaping, "_"), new Token(TagType.None, "wordA"), new Token(TagType.None, "_") });
    }

    public static IEnumerable<TestCaseData> CasesWhenTextContainsOverlappingTags()
    {
        yield return new TestCaseData("__wordA_wordB__wordC_",
            new[] { new Token(TagType.None, "__wordA_wordB__wordC_") });
        yield return new TestCaseData("_wordA__wordB_wordC__ wordD",
            new[] { new Token(TagType.None, "_wordA__wordB_wordC__ wordD") });
    }

    public static IEnumerable<TestCaseData> CasesWhenBoldTagInsideItalicTag()
    {
        yield return new TestCaseData("_wordA__wordB__wordC_",
            new[]
            {
                new Token(TagType.Italic, "wordA__wordB__wordC",
                [
                    new Token(TagType.None, "wordA"), new Token(TagType.None, "__wordB__"),
                    new Token(TagType.None, "wordC")
                ])
            });
    }

    public static IEnumerable<TestCaseData> CasesWhenItalicTagInsideBoldTag()
    {
        yield return new TestCaseData("__wordA_wordB_wordC__",
            new[]
            {
                new Token(TagType.Bold, "wordA_wordB_wordC",
                [
                    new Token(TagType.None, "wordA"), new Token(TagType.Italic, "wordB"),
                    new Token(TagType.None, "wordC")
                ])
            });
    }

    public static IEnumerable<TestCaseData> CasesWhenTextWithNumbersAndContainsBoldItalicTags()
    {
        yield return new TestCaseData("wo__rd1__",
            new[] { new Token(TagType.None, "wo"), new Token(TagType.None, "__rd1__") });
        yield return new TestCaseData("wo_rd1_",
            new[] { new Token(TagType.None, "wo"), new Token(TagType.None, "_rd1_") });
    }

    public static IEnumerable<TestCaseData> CasesWhenTextWithWhiteSpaceAndContainsBoldItalicTags()
    {
        yield return new TestCaseData("_wordA _", new[] { new Token(TagType.None, "_wordA _") });
        yield return new TestCaseData("__wordA __", new[] { new Token(TagType.None, "__wordA __") });
        yield return new TestCaseData("_ wordA_",
            new[] { new Token(TagType.None, "_ wordA"), new Token(TagType.None, "_") });
        yield return new TestCaseData("__ wordA__",
            new[] { new Token(TagType.None, "__ wordA"), new Token(TagType.None, "__") });
    }

    public static IEnumerable<TestCaseData> CasesWhenTextContainsBoldItalicTagsInMiddleWords()
    {
        yield return new TestCaseData("_wor_dA",
            new[] { new Token(TagType.Italic, "wor"), new Token(TagType.None, "dA") });
        yield return new TestCaseData("__wor__dA",
            new[] { new Token(TagType.Bold, "wor"), new Token(TagType.None, "dA") });
        yield return new TestCaseData("_wordA wor_dB",
            new[] { new Token(TagType.None, "_wordA wor_"), new Token(TagType.None, "dB") });
        yield return new TestCaseData("__wordA wor__dB",
            new[] { new Token(TagType.None, "__wordA wor__"), new Token(TagType.None, "dB") });
    }
        
    public static IEnumerable<TestCaseData> CasesWhenTextContainsLinkTag()
    {
        yield return new TestCaseData("[Name Link](https://www.example.com \"Tooltip\")",
            new[] { new TokenTagLink("Name Link", "https://www.example.com", "Tooltip") });
        yield return new TestCaseData("[Name Link](https://www.example.com Tooltip\")",
            new[] { new TokenTagLink("Name Link", "https://www.example.com Tooltip\"") });
        yield return new TestCaseData("Name Link](https://www.example.com Tooltip\")",
            new[] { new Token(TagType.None, "Name Link](https://www.example.com Tooltip\")") });
        yield return new TestCaseData("[Name Link(https://www.example.com Tooltip\")",
            new[] { new Token(TagType.None, "[Name Link(https://www.example.com Tooltip\")") });
        yield return new TestCaseData("[Name Link]https://www.example.com Tooltip\")",
            new[] { new Token(TagType.None, "[Name Link]https://www.example.com Tooltip\")") });
        yield return new TestCaseData("[Name Link](https://www.example.com Tooltip\"",
            new[] { new Token(TagType.None, "[Name Link](https://www.example.com Tooltip\"") });
    }
}