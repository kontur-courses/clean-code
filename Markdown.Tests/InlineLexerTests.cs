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

        Assert.That(actual, Is.Empty);
    }

    [Test]
    public void Tokenize_EmphasisMarkersAroundText_ReturnsTokensInOrder()
    {
        var actual = lexer.Tokenize("_текст_");

        CollectionAssert.AreEqual(
            new[]
            {
                EmphasisMarker("_"),
                Text("текст"),
                EmphasisMarker("_")
            },
            actual);
    }

    [Test]
    public void Tokenize_StrongMarkersAroundText_ReturnsTokensInOrder()
    {
        var actual = lexer.Tokenize("__текст__");

        CollectionAssert.AreEqual(
            new[]
            {
                StrongMarker("__"),
                Text("текст"),
                StrongMarker("__")
            },
            actual);
    }

    [Test]
    public void Tokenize_StrongEmphasisMarkersAroundText_ReturnsTokensInOrder()
    {
        var actual = lexer.Tokenize("___текст___");

        CollectionAssert.AreEqual(
            new[]
            {
                StrongEmphasisMarker("___"),
                Text("текст"),
                StrongEmphasisMarker("___")
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

    [Test]
    public void Tokenize_EscapedUnderscores_ReturnLiteralText()
    {
        var actual = lexer.Tokenize(@"\_текст\_");

        CollectionAssert.AreEqual(
            new[] { Text("_текст_") },
            actual);
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

    [Test]
    public void Tokenize_EscapedPunctuation_RemovesEscapeCharacter()
    {
        var actual = lexer.Tokenize(@"a\!b");

        CollectionAssert.AreEqual(
            new[] { Text("a!b") },
            actual);
    }

    [Test]
    public void Tokenize_BackslashBeforeLetterOrWhitespace_RemainsInText()
    {
        const string input = @"a\bc\ d";
        var actual = lexer.Tokenize(input);

        CollectionAssert.AreEqual(
            new[] { Text(input) },
            actual);
    }

    [Test]
    public void Tokenize_EscapedSymbol_RemovesEscapeCharacter()
    {
        var actual = lexer.Tokenize(@"a\©b");

        CollectionAssert.AreEqual(
            new[] { Text("a©b") },
            actual);
    }

    private static InlineToken Text(string value) =>
        new(InlineTokenKind.Text, value);

    private static InlineToken EmphasisMarker(string value) =>
        new(InlineTokenKind.EmphasisMarker, value);

    private static InlineToken StrongMarker(string value) =>
        new(InlineTokenKind.StrongMarker, value);

    private static InlineToken StrongEmphasisMarker(string value) =>
        new(InlineTokenKind.StrongEmphasisMarker, value);
}
