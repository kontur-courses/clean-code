using FluentAssertions;
using Markdown.Tokens;
using NUnit.Framework;
using System.Collections;
using Markdown.Tokens.Transformers;

namespace Markdown.Tests;

[TestFixture]
public class StringTokenizerTests
{
    public static IEnumerable PlainTextStringTokenizerCases
    {
        get
        {
            yield return new TestCaseData<string, IReadOnlyList<Token>>("Hello World!", [new TextToken("Hello World!")])
                .SetName("PlainText: simple sentence");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("", [])
                .SetName("PlainText: empty string");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("#Hello", [new TextToken("#Hello")])
                .SetName("PlainText: hash without space is not a header");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a # b", [new TextToken("a # b")])
                .SetName("PlainText: hash in the middle is not a header");
        }
    }

    public static IEnumerable HeaderStringTokenizerCases
    {
        get
        {
            yield return new TestCaseData<string, IReadOnlyList<Token>>("# Hello World!",
                    [new HeaderMarkerToken(), new TextToken("Hello World!")])
                .SetName("Header: marker and text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("# ",
                    [new HeaderMarkerToken()])
                .SetName("Header: marker only");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("# __x__",
                [
                    new HeaderMarkerToken(), new DoubleUnderscoreToken(true, false), new TextToken("x"),
                    new DoubleUnderscoreToken(false, true)
                ])
                .SetName("Header: with double underscore inside");
        }
    }

    public static IEnumerable UnderscoreStringTokenizerCases
    {
        get
        {
            yield return new TestCaseData<string, IReadOnlyList<Token>>("_Hello World!_",
                    [new UnderscoreToken(true, false), new TextToken("Hello World!"), new UnderscoreToken(false, true)])
                .SetName("Underscore: wraps text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("_a",
                    [new UnderscoreToken(true, false), new TextToken("a")])
                .SetName("Underscore: at line start can only open");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a_",
                    [new TextToken("a"), new UnderscoreToken(false, true)])
                .SetName("Underscore: at line end can only close");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a_b",
                    [new TextToken("a"), new UnderscoreToken(true, true), new TextToken("b")])
                .SetName("Underscore: inside word can open and close");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a _b",
                    [new TextToken("a "), new UnderscoreToken(true, false), new TextToken("b")])
                .SetName("Underscore: space before and letter after can only open");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a_ b",
                    [new TextToken("a"), new UnderscoreToken(false, true), new TextToken(" b")])
                .SetName("Underscore: letter before and space after can only close");
        }
    }

    
    public static IEnumerable NotDelimiterStringTokenizerCases
    {
        get
        {
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a _ b",
                    [new TextToken("a _ b")])
                .SetName("NotDelimiter: surrounded by spaces stays text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("_ a",
                    [new TextToken("_ a")])
                .SetName("NotDelimiter: space after at line start stays text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a __ b",
                    [new TextToken("a __ b")])
                .SetName("NotDelimiter: double underscore surrounded by spaces stays text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("12_3",
                    [new TextToken("12_3")])
                .SetName("NotDelimiter: between digits stays text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("1_a",
                    [new TextToken("1_a")])
                .SetName("NotDelimiter: digit before stays text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a_1",
                    [new TextToken("a_1")])
                .SetName("NotDelimiter: digit after stays text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("_12_",
                    [new TextToken("_12_")])
                .SetName("NotDelimiter: underscores around a number stay text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("_a1_",
                    [new UnderscoreToken(true, false), new TextToken("a1_")])
                .SetName("NotDelimiter: closing underscore after digit stays text (current behavior)");
        }
    }

    public static IEnumerable DoubleUnderscoreStringTokenizerCases
    {
        get
        {
            yield return new TestCaseData<string, IReadOnlyList<Token>>("__a__",
                    [new DoubleUnderscoreToken(true, false), new TextToken("a"), new DoubleUnderscoreToken(false, true)])
                .SetName("DoubleUnderscore: wraps text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("__Hello World!_",
                    [new DoubleUnderscoreToken(true, false), new TextToken("Hello World!"), new UnderscoreToken(false, true)])
                .SetName("DoubleUnderscore: mismatched closing underscore");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a__b",
                    [new TextToken("a"), new DoubleUnderscoreToken(true, true), new TextToken("b")])
                .SetName("DoubleUnderscore: inside word");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("____",
                    [new DoubleUnderscoreToken(true, false), new DoubleUnderscoreToken(false, true)])
                .SetName("DoubleUnderscore: four underscores split into two pairs");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("___",
                    [new DoubleUnderscoreToken(true, false), new UnderscoreToken(false, true)])
                .SetName("DoubleUnderscore: three underscores are greedy, double first");
        }
    }

    public static IEnumerable MixedStringTokenizerCases
    {
        get
        {
            yield return new TestCaseData<string, IReadOnlyList<Token>>("_a __b__ c_",
                [
                    new UnderscoreToken(true, false), new TextToken("a "),
                    new DoubleUnderscoreToken(true, false), new TextToken("b"), new DoubleUnderscoreToken(false, true),
                    new TextToken(" c"), new UnderscoreToken(false, true)
                ])
                .SetName("Mixed: double underscore nested in single");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("раз_ных слова_х",
                [
                    new TextToken("раз"), new UnderscoreToken(true, true),
                    new TextToken("ных слова"), new UnderscoreToken(true, true), new TextToken("х")
                ])
                .SetName("Mixed: underscores in different words");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("___Три прочерка___",
                [
                    new DoubleUnderscoreToken(true, false), new UnderscoreToken(true, true),
                    new TextToken("Три прочерка"),
                    new DoubleUnderscoreToken(true, true), new UnderscoreToken(false, true)
                ])
                .SetName("DoubleUnderscore: three underscores on both sides");
        }
    }

    public static IEnumerable MixedParagraphsStringTokenizerCases
    {
        get
        {
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a\nb",
                    [new TextToken("a"), new ParagraphBreakToken(), new TextToken("b")])
                .SetName("Paragraphs: two plain lines");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("# a\nb",
                    [new HeaderMarkerToken(), new TextToken("a"), new ParagraphBreakToken(), new TextToken("b")])
                .SetName("Paragraphs: header then plain line");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a\n# b",
                    [new TextToken("a"), new ParagraphBreakToken(), new HeaderMarkerToken(), new TextToken("b")])
                .SetName("Paragraphs: plain line then header");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("# a\n# b",
                [
                    new HeaderMarkerToken(), new TextToken("a"), new ParagraphBreakToken(),
                    new HeaderMarkerToken(), new TextToken("b")
                ])
                .SetName("Paragraphs: two headers");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("# \nb",
                    [new HeaderMarkerToken(), new ParagraphBreakToken(), new TextToken("b")])
                .SetName("Paragraphs: empty header then plain line");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a\n\nb",
                    [new TextToken("a"), new ParagraphBreakToken(), new ParagraphBreakToken(), new TextToken("b")])
                .SetName("Paragraphs: empty line gives two breaks");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("# a\n\nb",
                [
                    new HeaderMarkerToken(), new TextToken("a"), new ParagraphBreakToken(),
                    new ParagraphBreakToken(), new TextToken("b")
                ])
                .SetName("Paragraphs: header, empty line, plain line");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("\n",
                    [new ParagraphBreakToken()])
                .SetName("Paragraphs: only newline");
        }
    }

    
    public static IEnumerable EscapeStringTokenizerCases
    {
        get
        {
            yield return new TestCaseData<string, IReadOnlyList<Token>>("\\_a\\_",
                    [new TextToken("_a_")])
                .SetName("Escape: both underscores escaped stay text, merged into one token");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("\\_a_",
                    [new TextToken("_a"), new UnderscoreToken(false, true)])
                .SetName("Escape: escaped opening underscore, closing stays delimiter");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("\\\\",
                    [new TextToken("\\")])
                .SetName("Escape: escaped backslash");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("\\\\_a_",
                [
                    new TextToken("\\"), new UnderscoreToken(true, true), new TextToken("a"),
                    new UnderscoreToken(false, true)
                ])
                .SetName("Escape: escaped backslash does not escape following underscore");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("сим\\волы",
                    [new TextToken("сим\\волы")])
                .SetName("Escape: backslash before ordinary character stays");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a\\",
                    [new TextToken("a\\")])
                .SetName("Escape: backslash at line end stays");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("\\# a",
                    [new TextToken("# a")])
                .SetName("Escape: escaped hash is not a header");
        }
    }

    [TestCaseSource(nameof(PlainTextStringTokenizerCases))]
    [TestCaseSource(nameof(HeaderStringTokenizerCases))]
    [TestCaseSource(nameof(UnderscoreStringTokenizerCases))]
    [TestCaseSource(nameof(NotDelimiterStringTokenizerCases))]
    [TestCaseSource(nameof(DoubleUnderscoreStringTokenizerCases))]
    [TestCaseSource(nameof(MixedStringTokenizerCases))]
    [TestCaseSource(nameof(MixedParagraphsStringTokenizerCases))]
    [TestCaseSource(nameof(EscapeStringTokenizerCases))]
    public void StringTokenizer_ReturnsExpectedArray(string markdown, IReadOnlyList<Token> expected)
    {
        var result = StringTokenizer.Tokenize(markdown);

        result.Should().BeEqualTo(expected);
    }
}