using NUnit.Framework;
using Markdown.Lexer;

namespace Markdown.Tests;

[TestFixture]
public class InlineLexerTests
{
    private InlineLexer lexer = null!;

    [SetUp]
    public void SetUp()
    {
        lexer = new InlineLexer();
    }

    [Test]
    public void Tokenize_PlainText_ReturnsSingleTextToken()
    {
        var actual = lexer.Tokenize("обычный текст");

        CollectionAssert.AreEqual(
            new[] { Text("обычный текст") },
            actual);
    }

    [Test]
    public void Tokenize_ThreeAdjacentUnderscores_ReturnsStrongEmphasisMarker()
    {
        var actual = lexer.Tokenize("___");

        CollectionAssert.AreEqual(
            new[] { StrongEmphasisMarker("___") },
            actual);
    }

    [Test]
    public void Tokenize_FourAdjacentUnderscores_ReturnsOneStrongEmphasisMarkerAndOneEmphasisMarker()
    {
        var actual = lexer.Tokenize("____");

        CollectionAssert.AreEqual(
            new[]
            {
                StrongEmphasisMarker("___"),
                EmphasisMarker("_")
            },
            actual);
    }

    [Test]
    public void Tokenize_EmptyInput_ReturnsNoTokens()
    {
        var actual = lexer.Tokenize(string.Empty);

        Assert.IsEmpty(actual);
    }

    [TestCase("_текст_", InlineTokenKind.EmphasisMarker, "_", TestName = "Tokenize_EmphasisMarkersAroundText_ReturnsTokensInOrder")]
    [TestCase("__текст__", InlineTokenKind.StrongMarker, "__", TestName = "Tokenize_StrongMarkersAroundText_ReturnsTokensInOrder")]
    [TestCase("___текст___", InlineTokenKind.StrongEmphasisMarker, "___", TestName = "Tokenize_StrongEmphasisMarkersAroundText_ReturnsTokensInOrder")]
    public void Tokenize_MarkerAroundText_ReturnsTokensInOrder(
        string input,
        InlineTokenKind markerKind,
        string marker)
    {
        var actual = lexer.Tokenize(input);

        CollectionAssert.AreEqual(
            new[]
            {
                Marker(markerKind, marker),
                Text("текст"),
                Marker(markerKind, marker)
            },
            actual);
    }

    [Test]
    public void Tokenize_MixedMarkers_PreservesTokenOrder()
    {
        var actual = lexer.Tokenize("__жирный _и курсив_ текст__");

        CollectionAssert.AreEqual(
            new[]
            {
                StrongMarker("__"),
                Text("жирный "),
                EmphasisMarker("_"),
                Text("и курсив"),
                EmphasisMarker("_"),
                Text(" текст"),
                StrongMarker("__")
            },
            actual);
    }

    [TestCase(@"\_текст\_", "_текст_", TestName = "Tokenize_EscapedUnderscores_ReturnLiteralText")]
    [TestCase(@"\# заголовок", "# заголовок", TestName = "Tokenize_EscapedHeadingMarker_RemovesEscapeCharacter")]
    [TestCase(@"\- пункт", "- пункт", TestName = "Tokenize_EscapedListMarker_RemovesEscapeCharacter")]
    public void Tokenize_EscapedMarkupMarkers_ReturnLiteralText(string input, string expected)
    {
        var actual = lexer.Tokenize(input);

        CollectionAssert.AreEqual(
            new[] { Text(expected) }, actual);
    }

    [Test]
    public void Tokenize_EscapedBackslash_ReturnsOneBackslash()
    {
        var actual = lexer.Tokenize(@"\\");

        CollectionAssert.AreEqual(
            new[] { Text("\\") },
            actual);
    }

    [Test]
    public void Tokenize_TrailingBackslash_PreservesBackslash()
    {
        var actual = lexer.Tokenize("текст\\");

        CollectionAssert.AreEqual(
            new[] { Text("текст\\") },
            actual);
    }

    [Test]
    public void Tokenize_EscapedAndUnescapedUnderscores_TokenizesOnlyUnescapedMarker()
    {
        var actual = lexer.Tokenize(@"\_текст_");

        CollectionAssert.AreEqual(
            new[]
            {
                Text("_текст"),
                EmphasisMarker("_")
            },
            actual);
    }

    [Test]
    public void Tokenize_EscapedBackslashBeforeEmphasis_StillTokenizesMarkup()
    {
        var actual = lexer.Tokenize(@"\\_текст_");

        CollectionAssert.AreEqual(
            new[]
            {
                Text("\\"),
                EmphasisMarker("_"),
                Text("текст"),
                EmphasisMarker("_")
            },
            actual);
    }

    [TestCase(@"a\!b", @"a\!b", TestName = "Tokenize_BackslashBeforeNonMarkupPunctuation_RemainsInText")]
    [TestCase(@"a\bc\ d", @"a\bc\ d", TestName = "Tokenize_BackslashBeforeLetterOrWhitespace_RemainsInText")]
    [TestCase(@"a\©b", @"a\©b", TestName = "Tokenize_BackslashBeforeNonMarkupSymbol_RemainsInText")]
    public void Tokenize_BackslashBeforeNonEscapableCharacter_RemainsInText(string input, string expected)
    {
        var actual = lexer.Tokenize(input);

        CollectionAssert.AreEqual(
            new[] { Text(expected) },
            actual);
    }

    private static InlineToken Text(string value) =>
        new(InlineTokenKind.Text, value);

    private static InlineToken Marker(InlineTokenKind kind, string value) =>
        new(kind, value);

    private static InlineToken EmphasisMarker(string value) =>
        new(InlineTokenKind.EmphasisMarker, value);

    private static InlineToken StrongMarker(string value) =>
        new(InlineTokenKind.StrongMarker, value);

    private static InlineToken StrongEmphasisMarker(string value) =>
        new(InlineTokenKind.StrongEmphasisMarker, value);
}
