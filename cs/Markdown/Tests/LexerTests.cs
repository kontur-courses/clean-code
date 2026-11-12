using FluentAssertions;
using Markdown.Parsing;
using Markdown.Parsing.Parsers.InlineParser;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class LexerTests
{
    private Lexer lexer;

    [SetUp]
    public void Setup() => lexer = new Lexer();

    [Test]
    public void Tokenize_PlainText_ReturnsSingleTextToken()
    {
        var tokens = lexer.Tokenize("Просто текст");
        tokens.Should().HaveCount(1);
        tokens[0].Type.Should().Be(TokenType.Text);
        tokens[0].Value.Should().Be("Просто текст");
    }

    [Test]
    public void Tokenize_Italic_ReturnsItalicTokens()
    {
        var tokens = lexer.Tokenize("_текст_");
        tokens.Should().HaveCount(3);
        tokens[0].Type.Should().Be(TokenType.Italic);
        tokens[1].Type.Should().Be(TokenType.Text);
        tokens[2].Type.Should().Be(TokenType.Italic);
    }

    [Test]
    public void Tokenize_TripleMarkers_SplitsIntoTriple()
    {
        var tokens = lexer.Tokenize("___текст___");
        tokens.Should().HaveCount(5);

        tokens.Should().HaveCount(5);
        tokens[0].Type.Should().Be(TokenType.Bold);
        tokens[1].Type.Should().Be(TokenType.Italic);
        tokens[2].Type.Should().Be(TokenType.Text);
        tokens[3].Type.Should().Be(TokenType.Italic);
        tokens[4].Type.Should().Be(TokenType.Bold);
    }

    [Test]
    public void Tokenize_EscapedUnderscore_ReturnsText()
    {
        var tokens = lexer.Tokenize("\\_текст\\_");
        tokens.Should().HaveCount(1);
        tokens[0].Type.Should().Be(TokenType.Text);
        tokens[0].Value.Should().Be("_текст_");
    }

    [Test]
    public void Tokenize_UnderscoresBetweenDigits_ReturnsText()
    {
        var tokens = lexer.Tokenize("12_3");
        tokens.Should().HaveCount(1);
        tokens[0].Type.Should().Be(TokenType.Text);
        tokens[0].Value.Should().Be("12_3");
    }
}