using FluentAssertions;
using Markdown.Domain;
using static Markdown.Parsing.InlineTokenizer;
using static Markdown.Parsing.DelimiterMatcher;

namespace Markdown.Tests;

public class DelimiterMatcherTests
{
    [TestCase("")]
    [TestCase("plain text")]
    [TestCase("_text")]
    [TestCase("text_")]
    [TestCase("__text_")]
    [TestCase("_text__")]
    [TestCase("___")]
    [TestCase("word____word")]
    public void FindPairs_NoMatchingDelimiters_ReturnsEmpty(string text)
        => AssertPairs(text, []);

    [TestCase("__text _word__ one_")]
    [TestCase("_text __word_ one__")]
    [TestCase("___text__ word one_")]
    public void FindPairs_IntersectingPairs_ReturnEmpty(string text)
        => AssertPairs(text, []);

    [Test]
    public void FindPairs_StrongInEmphasis_ReturnOnlyEmphasisPair()
    {
        const string text = "_text __word__ one_";

        var expectedPairs = new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 1, true, false),
                new DelimiterRunToken(18, 1, false, true),
                DelimiterType.Emphasis)
        };

        AssertPairs(text, expectedPairs);
    }

    [TestCase("o_ne t_wo")]
    [TestCase("o__ne t__wo")]
    public void FindPairs_DelimitersInDifferentWords_ReturnEmpty(string text)
        => AssertPairs(text, []);

    [TestCaseSource(nameof(EmphasisCases))]
    public void FindPairs_Emphasis_ReturnsExpectedPair(string text, DelimiterPair[] expectedPairs)
        => AssertPairs(text, expectedPairs);

    [TestCaseSource(nameof(StrongCases))]
    public void FindPairs_Strong_ReturnsPair(string text, DelimiterPair[] expectedPairs)
        => AssertPairs(text, expectedPairs);

    [TestCaseSource(nameof(EmphasisInStrongCases))]
    public void FindPairs_EmphasisInStrong_ReturnExpectedPairs(string text, DelimiterPair[] expected)
        => AssertPairs(text, expected);

    [TestCaseSource(nameof(NestedWithinSameTypeCases))]
    public void FindPairs_NestedWithinSameType_ReturnParentPair(string text, DelimiterPair[] expected)
        => AssertPairs(text, expected);

    private static void AssertPairs(string text, DelimiterPair[] expectedPairs)
    {
        // да, в идеале бы мне ручками вводить эталонные токены,
        // но я не хочу тратить на это время, поэтому просто буду использовать токенизацию
        var tokens = Tokenize(text);

        FindPairs(tokens, text).Should().BeEquivalentTo(expectedPairs);
    }

    private static IEnumerable<TestCaseData> EmphasisCases()
    {
        yield return new TestCaseData("_text_", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 1, true, false),
                new DelimiterRunToken(5, 1, false, true),
                DelimiterType.Emphasis)
        });

        yield return new TestCaseData("_three_delimiters_", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 1, true, false),
                new DelimiterRunToken(6, 1, true, true),
                DelimiterType.Emphasis)
        });

        yield return new TestCaseData("_world gears_ _yes_", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 1, true, false),
                new DelimiterRunToken(12, 1, false, true),
                DelimiterType.Emphasis),
            new DelimiterPair(
                new DelimiterRunToken(14, 1, true, false),
                new DelimiterRunToken(18, 1, false, true),
                DelimiterType.Emphasis)
        });

        yield return new TestCaseData("_start_word", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 1, true, false),
                new DelimiterRunToken(6, 1, true, true),
                DelimiterType.Emphasis)
        });

        yield return new TestCaseData("mid_dle_word", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(3, 1, true, true),
                new DelimiterRunToken(7, 1, true, true),
                DelimiterType.Emphasis)
        });

        yield return new TestCaseData("word_end_", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(4, 1, true, true),
                new DelimiterRunToken(8, 1, false, true),
                DelimiterType.Emphasis)
        });

    }

    private static IEnumerable<TestCaseData> StrongCases()
    {
        yield return new TestCaseData("__text__", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 2, true, false),
                new DelimiterRunToken(6, 2, false, true),
                DelimiterType.Strong)
        });

        yield return new TestCaseData("__three__delimiters__", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 2, true, false),
                new DelimiterRunToken(7, 2, true, true),
                DelimiterType.Strong)
        });

        yield return new TestCaseData("____text__", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(2, 2, true, false),
                new DelimiterRunToken(8, 2, false, true),
                DelimiterType.Strong)
        });

        yield return new TestCaseData("__text____", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 2, true, false),
                new DelimiterRunToken(6, 2, false,  true),
                DelimiterType.Strong)
        });
    }

    private static IEnumerable<TestCaseData> EmphasisInStrongCases()
    {
        yield return new TestCaseData("__text _word_ one__", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 2, true, false),
                new DelimiterRunToken(17, 2, false, true),
                DelimiterType.Strong),
            new DelimiterPair(
                new DelimiterRunToken(7, 1, true, false),
                new DelimiterRunToken(12, 1, false, true),
                DelimiterType.Emphasis)
        });

        yield return new TestCaseData("___text word_ one__", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 2, true, false),
                new DelimiterRunToken(17, 2, false, true),
                DelimiterType.Strong),
            new DelimiterPair(
                new DelimiterRunToken(2, 1, true, false),
                new DelimiterRunToken(12, 1, false, true),
                DelimiterType.Emphasis)
        });

        yield return new TestCaseData("__text _word one___", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 2, true, false),
                new DelimiterRunToken(17, 2, false, true),
                DelimiterType.Strong),
            new DelimiterPair(
                new DelimiterRunToken(7, 1, true, false),
                new DelimiterRunToken(16, 1, false, true),
                DelimiterType.Emphasis)
        });

        yield return new TestCaseData("___text___", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 2, true, false),
                new DelimiterRunToken(8, 2, false, true),
                DelimiterType.Strong),
            new DelimiterPair(
                new DelimiterRunToken(2, 1, true, false),
                new DelimiterRunToken(7, 1, false, true),
                DelimiterType.Emphasis)
        });
    }

    private static IEnumerable<TestCaseData> NestedWithinSameTypeCases()
    {
        yield return new TestCaseData("_one _two_ three_", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 1, true, false),
                new DelimiterRunToken(16, 1, false, true),
                DelimiterType.Emphasis)
        });

        yield return new TestCaseData("__one __two__ three__", new[]
        {
            new DelimiterPair(
                new DelimiterRunToken(0, 2, true, false),
                new DelimiterRunToken(19, 2, false, true),
                DelimiterType.Strong)
        });
    }
}