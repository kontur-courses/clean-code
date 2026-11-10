using FluentAssertions;
using Markdown.Tokens;
using Markdown.Transformers;
using NUnit.Framework;
using System.Collections;

namespace Markdown.Tests;

[TestFixture]
public class StringTokenizerTests
{
    public static IEnumerable PlainTextStringTokenizerCases
    {
        get
        {
            yield return new TestCaseData<string, IReadOnlyList<Token>>("Hello World!", [Token.Text("Hello World!")])
                .SetName("PlainText: simple sentence");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("", [])
                .SetName("PlainText: empty string");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("#Hello", [Token.Text("#Hello")])
                .SetName("PlainText: hash without space is not a header");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a # b", [Token.Text("a # b")])
                .SetName("PlainText: hash in the middle is not a header");
        }
    }

    public static IEnumerable HeaderStringTokenizerCases
    {
        get
        {
            yield return new TestCaseData<string, IReadOnlyList<Token>>("# Hello World!",
                    [Token.HeaderMarker(), Token.Text("Hello World!")])
                .SetName("Header: marker and text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("# ",
                    [Token.HeaderMarker()])
                .SetName("Header: marker only");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("# __x__",
                [
                    Token.HeaderMarker(), Token.DoubleUnderscore(true, false), Token.Text("x"),
                    Token.DoubleUnderscore(false, true)
                ])
                .SetName("Header: with double underscore inside");
        }
    }

    public static IEnumerable UnderscoreStringTokenizerCases
    {
        get
        {
            yield return new TestCaseData<string, IReadOnlyList<Token>>("_Hello World!_",
                    [Token.Underscore(true, false), Token.Text("Hello World!"), Token.Underscore(false, true)])
                .SetName("Underscore: wraps text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("_a",
                    [Token.Underscore(true, false), Token.Text("a")])
                .SetName("Underscore: at line start can only open");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a_",
                    [Token.Text("a"), Token.Underscore(false, true)])
                .SetName("Underscore: at line end can only close");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a_b",
                    [Token.Text("a"), Token.Underscore(true, true), Token.Text("b")])
                .SetName("Underscore: inside word can open and close");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a _b",
                    [Token.Text("a "), Token.Underscore(true, false), Token.Text("b")])
                .SetName("Underscore: space before and letter after can only open");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a_ b",
                    [Token.Text("a"), Token.Underscore(false, true), Token.Text(" b")])
                .SetName("Underscore: letter before and space after can only close");
        }
    }


    public static IEnumerable NotDelimiterStringTokenizerCases
    {
        get
        {
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a _ b",
                    [Token.Text("a _ b")])
                .SetName("NotDelimiter: surrounded by spaces stays text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("_ a",
                    [Token.Text("_ a")])
                .SetName("NotDelimiter: space after at line start stays text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a __ b",
                    [Token.Text("a __ b")])
                .SetName("NotDelimiter: double underscore surrounded by spaces stays text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("12_3",
                    [Token.Text("12_3")])
                .SetName("NotDelimiter: between digits stays text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("1_a",
                    [Token.Text("1_a")])
                .SetName("NotDelimiter: digit before stays text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a_1",
                    [Token.Text("a_1")])
                .SetName("NotDelimiter: digit after stays text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("_12_",
                    [Token.Text("_12_")])
                .SetName("NotDelimiter: underscores around a number stay text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("_a1_",
                    [Token.Underscore(true, false), Token.Text("a1_")])
                .SetName("NotDelimiter: closing underscore after digit stays text (current behavior)");
        }
    }

    public static IEnumerable DoubleUnderscoreStringTokenizerCases
    {
        get
        {
            yield return new TestCaseData<string, IReadOnlyList<Token>>("__a__",
                    [Token.DoubleUnderscore(true, false), Token.Text("a"), Token.DoubleUnderscore(false, true)])
                .SetName("DoubleUnderscore: wraps text");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("__Hello World!_",
                    [Token.DoubleUnderscore(true, false), Token.Text("Hello World!"), Token.Underscore(false, true)])
                .SetName("DoubleUnderscore: mismatched closing underscore");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a__b",
                    [Token.Text("a"), Token.DoubleUnderscore(true, true), Token.Text("b")])
                .SetName("DoubleUnderscore: inside word");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("____",
                    [Token.DoubleUnderscore(true, false), Token.DoubleUnderscore(false, true)])
                .SetName("DoubleUnderscore: four underscores split into two pairs");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("___",
                    [Token.DoubleUnderscore(true, false), Token.Underscore(false, true)])
                .SetName("DoubleUnderscore: three underscores are greedy, double first");
        }
    }

    public static IEnumerable MixedStringTokenizerCases
    {
        get
        {
            yield return new TestCaseData<string, IReadOnlyList<Token>>("_a __b__ c_",
                [
                    Token.Underscore(true, false), Token.Text("a "),
                    Token.DoubleUnderscore(true, false), Token.Text("b"), Token.DoubleUnderscore(false, true),
                    Token.Text(" c"), Token.Underscore(false, true)
                ])
                .SetName("Mixed: double underscore nested in single");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("раз_ных слова_х",
                [
                    Token.Text("раз"), Token.Underscore(true, true),
                    Token.Text("ных слова"), Token.Underscore(true, true), Token.Text("х")
                ])
                .SetName("Mixed: underscores in different words");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("___Три прочерка___",
                [
                    Token.DoubleUnderscore(true, false), Token.Underscore(true, true),
                    Token.Text("Три прочерка"),
                    Token.DoubleUnderscore(true, true), Token.Underscore(false, true)
                ])
                .SetName("DoubleUnderscore: three underscores on both sides");
        }
    }

    public static IEnumerable MixedParagraphsStringTokenizerCases
    {
        get
        {
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a\nb",
                    [Token.Text("a"), Token.ParagraphBreak(), Token.Text("b")])
                .SetName("Paragraphs: two plain lines");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("# a\nb",
                    [Token.HeaderMarker(), Token.Text("a"), Token.ParagraphBreak(), Token.Text("b")])
                .SetName("Paragraphs: header then plain line");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a\n# b",
                    [Token.Text("a"), Token.ParagraphBreak(), Token.HeaderMarker(), Token.Text("b")])
                .SetName("Paragraphs: plain line then header");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("# a\n# b",
                [
                    Token.HeaderMarker(), Token.Text("a"), Token.ParagraphBreak(),
                    Token.HeaderMarker(), Token.Text("b")
                ])
                .SetName("Paragraphs: two headers");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("# \nb",
                    [Token.HeaderMarker(), Token.ParagraphBreak(), Token.Text("b")])
                .SetName("Paragraphs: empty header then plain line");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a\n\nb",
                    [Token.Text("a"), Token.ParagraphBreak(), Token.ParagraphBreak(), Token.Text("b")])
                .SetName("Paragraphs: empty line gives two breaks");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("# a\n\nb",
                [
                    Token.HeaderMarker(), Token.Text("a"), Token.ParagraphBreak(),
                    Token.ParagraphBreak(), Token.Text("b")
                ])
                .SetName("Paragraphs: header, empty line, plain line");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("\n",
                    [Token.ParagraphBreak()])
                .SetName("Paragraphs: only newline");
        }
    }


    public static IEnumerable EscapeStringTokenizerCases
    {
        get
        {
            yield return new TestCaseData<string, IReadOnlyList<Token>>("\\_a\\_",
                    [Token.Text("_a"), Token.Text("_")])
                .SetName("Escape: both underscores escaped stay text, tokens get split");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("\\_a_",
                    [Token.Text("_a"), Token.Underscore(false, true)])
                .SetName("Escape: escaped opening underscore, closing stays delimiter");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("\\\\",
                    [Token.Text("\\")])
                .SetName("Escape: escaped backslash");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("\\\\_a_",
                [
                    Token.Text("\\"), Token.Underscore(true, true), Token.Text("a"),
                    Token.Underscore(false, true)
                ])
                .SetName("Escape: escaped backslash does not escape following underscore");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("сим\\волы",
                    [Token.Text("сим\\волы")])
                .SetName("Escape: backslash before ordinary character stays");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("a\\",
                    [Token.Text("a\\")])
                .SetName("Escape: backslash at line end stays");
            yield return new TestCaseData<string, IReadOnlyList<Token>>("\\# a",
                    [Token.Text("# a")])
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