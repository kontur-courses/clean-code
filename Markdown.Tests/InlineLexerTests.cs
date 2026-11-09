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
    public void Tokenize_PlainText_ReturnsOneTextToken()
    {
        var actual = lexer.Tokenize("обычный текст");

        CollectionAssert.AreEqual(
            new[] { Text("обычный текст") },
            actual);
    }

    [Test]
    public void Tokenize_EmptyText_ReturnsNoTokens()
    {
        var actual = lexer.Tokenize(string.Empty);

        Assert.That(actual, Is.Empty);
    }

    [Test]
    public void Tokenize_EmphasisMarkers_ReturnsTextAndMarkerTokens()
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
    public void Tokenize_StrongMarkers_ReturnsTextAndMarkerTokens()
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
    public void Tokenize_EscapedMarkers_AddsThemToText()
    {
        var actual = lexer.Tokenize(@"\_текст\_");

        CollectionAssert.AreEqual(
            new[] { Text("_текст_") },
            actual);
    }

    [Test]
    public void Tokenize_EscapedEscapeCharacter_ProducesOneBackslash()
    {
        var actual = lexer.Tokenize(@"\\");

        CollectionAssert.AreEqual(
            new[] { Text("\\") },
            actual);
    }

    [Test]
    public void Tokenize_TrailingEscapeCharacter_PreservesBackslash()
    {
        var actual = lexer.Tokenize("текст\\");

        CollectionAssert.AreEqual(
            new[] { Text("текст\\") },
            actual);
    }

    [Test]
    public void Tokenize_EscapedAndUnescapedMarkers_OnlyTokenizesUnescapedMarker()
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
    public void Tokenize_EscapedBackslashBeforeMarkers_StillTokenizesMarkers()
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
    public void Tokenize_EscapedCharacter_RemovesEscapeCharacter()
    {
        var actual = lexer.Tokenize(@"a\!b");

        CollectionAssert.AreEqual(
            new[] { Text("a!b") },
            actual);
    }

    [Test]
    public void Tokenize_EscapeBeforeLetterOrWhitespace_PreservesBackslash()
    {
        const string input = @"a\bc\ d";
        var actual = lexer.Tokenize(input);

        CollectionAssert.AreEqual(
            new[] { Text(input) },
            actual);
    }

    private static InlineToken Text(string value) =>
        new(InlineTokenKind.Text, value);

    private static InlineToken EmphasisMarker(string value) =>
        new(InlineTokenKind.EmphasisMarker, value);

    private static InlineToken StrongMarker(string value) =>
        new(InlineTokenKind.StrongMarker, value);
}
