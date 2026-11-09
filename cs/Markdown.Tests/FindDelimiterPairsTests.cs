using FluentAssertions;
using Markdown.Domain;
using static Markdown.InlineTokenizer;
using static Markdown.MarkdownParser;

namespace Markdown.Tests;

public class FindDelimiterPairsTests
{
    [TestCase("")]
    [TestCase("plain text")]
    [TestCase("_text")]
    [TestCase("text_")]
    [TestCase("__text_")]
    [TestCase("_text__")]
    [TestCase("___")]
    [TestCase("word____word")]
    public void FindTokenPairs_NoMatchingDelimiters_ReturnsEmpty(string text)
    {
        var tokens = Tokenize(text);

        FindDelimiterTokenPairs(tokens, text).Should().BeEmpty();
    }

    [TestCaseSource(nameof(EmphasisCases))]
    public void FindTokenPairs_Emphasis_ReturnsExpectedPair(
        string text,
        DelimiterPair[] expectedPairs)
    {
        var tokens = Tokenize(text);

        FindDelimiterTokenPairs(tokens, text).Should().BeEquivalentTo(expectedPairs);
    }

    private static IEnumerable<TestCaseData> EmphasisCases()
    {
        yield return new TestCaseData("_text_", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 1, true, false),
                new DelimiterRunToken(5, 1, false, true) )
        });

        yield return new TestCaseData("_three_delimiters_", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 1, true, false),
                new DelimiterRunToken(6, 1, true, true))
        });

        yield return new TestCaseData("_world gears_ _yes_", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 1, true, false),
                new DelimiterRunToken(12, 1, false, true)),
            new DelimiterPair(
                new DelimiterRunToken(14, 1, true, false),
                new DelimiterRunToken(18, 1, false, true))
        });

        yield return new TestCaseData("_start_word", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 1, true, false),
                new DelimiterRunToken(6, 1, true, true))
        });

        yield return new TestCaseData("mid_dle_word", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(3, 1, true, true),
                new DelimiterRunToken(7, 1, true, true))
        });

        yield return new TestCaseData("word_end_", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(4, 1, true, true),
                new DelimiterRunToken(8, 1, false, true))
        });

    }

    [TestCaseSource(nameof(StrongCases))]
    public void FindTokenPairs_Strong_ReturnsPair(
        string text,
        DelimiterPair[] expectedPairs)
    {
        var tokens = Tokenize(text);

        FindDelimiterTokenPairs(tokens, text).Should().BeEquivalentTo(expectedPairs);
    }

    private static IEnumerable<TestCaseData> StrongCases()
    {
        yield return new TestCaseData("__text__", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 2, true, false),
                new DelimiterRunToken(6, 2, false, true))
        });

        yield return new TestCaseData("__three__delimiters__", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 2, true, false),
                new DelimiterRunToken(7, 2, true, true))
        });

        yield return new TestCaseData("____text__", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(2, 2, true, false),
                new DelimiterRunToken(8, 2, false, true))
        });

        yield return new TestCaseData("__text____", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 2, true, false),
                new DelimiterRunToken(6, 2, false,  true))
        });
    }

    [TestCase("__text _word__ one_")]
    [TestCase("_text __word_ one__")]
    [TestCase("___text__ word one_")]
    public void FindTokenPairs_IntersectingPairs_ReturnEmpty(string text)
    {
        FindDelimiterTokenPairs(Tokenize(text), text).Should().BeEmpty();
    }

    [TestCaseSource(nameof(EmphasisInStrongCases))]
    public void FindTokenPairs_EmphasisInStrong_ReturnExpectedPairs(
        string text,
        DelimiterPair[] expected)
    {
        var pairs = FindDelimiterTokenPairs(Tokenize(text), text);

        pairs.Should().BeEquivalentTo(expected);
    }

    private static IEnumerable<TestCaseData> EmphasisInStrongCases()
    {
        yield return new TestCaseData("__text _word_ one__", new []
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 2, true, false),
                new DelimiterRunToken(17, 2, false, true)),
            new DelimiterPair(
                new DelimiterRunToken(7, 1, true, false),
                new DelimiterRunToken(12, 1, false, true))
        });

        yield return new TestCaseData("___text word_ one__", new []
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 2, true, false),
                new DelimiterRunToken(17, 2, false, true)),
            new DelimiterPair(
                new DelimiterRunToken(2, 1, true, false),
                new DelimiterRunToken(12, 1, false, true))
        });

        yield return new TestCaseData("__text _word one___", new []
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 2, true, false),
                new DelimiterRunToken(17, 2, false, true)),
            new DelimiterPair(
                new DelimiterRunToken(7, 1, true, false),
                new DelimiterRunToken(16, 1, false, true))
        });

        yield return new TestCaseData("___text___", new []
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 2, true, false),
                new DelimiterRunToken(8, 2, false, true)),
            new DelimiterPair(
                new DelimiterRunToken(2, 1, true, false),
                new DelimiterRunToken(7, 1, false, true))
        });
    }

    [Test]
    public void FindTokenPairs_StrongInEmphasis_ReturnOnlyEmphasisPair()
    {
        const string text = "_text __word__ one_";

        var expectedPairs = new []
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 1, true, false),
                new DelimiterRunToken(18, 1, false, true))
        };

        FindDelimiterTokenPairs(Tokenize(text), text).Should().BeEquivalentTo(expectedPairs);
    }

    [TestCaseSource(nameof(NestedWithinSameTypeCases))]
    public void FindTokenPairs_NestedWithinSameType_ReturnParentPair(
        string text,
        DelimiterPair[] expected)
    {
        var tokens = Tokenize(text);

        FindDelimiterTokenPairs(tokens, text).Should().BeEquivalentTo(expected);
    }

    private static IEnumerable<TestCaseData> NestedWithinSameTypeCases()
    {
        yield return new TestCaseData("_one _two_ three_", new []
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 1, true, false),
                new DelimiterRunToken(16, 1, false, true))
        });

        yield return new TestCaseData("__one __two__ three__", new []
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 2, true, false),
                new DelimiterRunToken(19, 2, false, true))
        });
    }

    [TestCase("o_ne t_wo")]
    [TestCase("o__ne t__wo")]
    public void FindTokenPairs_DelimitersInDifferentWords_ReturnEmpty(string text)
    {
        var tokens = Tokenize(text);

        FindDelimiterTokenPairs(tokens, text).Should().BeEmpty();
    }

}