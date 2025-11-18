using FluentAssertions;
using Markdown.Tokenizing;
using NUnit.Framework;

namespace Markdown.Tests;

public class LexerTests
{
    private List<Token> Tokens(string text)
    {
        var lexer =  new Lexer();
        return lexer.Tokenize(text);
    }
    private List<TokenType> Types(string text)
    {
        var lexer =  new Lexer();
        return lexer.Tokenize(text).Select(t => t.Type).ToList();
    }

    [TestCase("hello!")]
    public void Lexer_ProduceTextToken_WhenSimpleWord(string input)
    {
        var tokens = Tokens(input);
        tokens.Should().HaveCount(2);
        tokens[0].Type.Should().Be(TokenType.Text);
        tokens[0].Value.Should().Be(input);
    }


    [TestCase("a b")]
    public void Lexer_ProduceWhitespaceToken_WhenSpaceEncountered(string input)
    {
        var tokens = Tokens(input);
        tokens[1].Type.Should().Be(TokenType.Whitespace);
        tokens[1].Value.Should().Be(" ");
    }


    [TestCase("a\tb")]
    public void Lexer_ProduceWhitespaceTokenWithTabValue_WhenTabEncountered(string input)
    {
        var tokens = Tokens(input);
        tokens[1].Type.Should().Be(TokenType.Whitespace);
        tokens[1].Value.Should().Be("\t");
    }


    [TestCase("a\nb")]
    public void Lexer_ProduceEndOfLineToken_WhenNewlineEncountered(string input)
    {
        var tokens = Tokens(input);
        tokens[1].Type.Should().Be(TokenType.EndOfLine);
        tokens[2].Type.Should().Be(TokenType.Text);
    }


    [TestCase("# hello!")]
    public void Lexer_ProduceHashToken_WhenHashEncountered(string input)
    {
        var tokens = Tokens(input);
        tokens[0].Type.Should().Be(TokenType.Hash);
        tokens[1].Type.Should().Be(TokenType.Whitespace);
        tokens[2].Type.Should().Be(TokenType.Text);
    }


    [TestCase("")]
    [TestCase(" ")]
    [TestCase("hello!")]
    public void Lexer_PlaceEndOfFileTokenLast_WhenTokenizingAnyInput(string input)
    {
        var tokens = Tokens(input);
        tokens[^1].Type.Should().Be(TokenType.EndOfFile);
    }


    [Test]
    public void Lexer_ProduceUnderscoreToken_WhenSingleUnderscoreEncountered()
    {
        var tokens = Tokens("_");
        tokens[0].Type.Should().Be(TokenType.Underscore);
    }


    [Test]
    public void Lexer_ProduceDoubleUnderscoreToken_WhenTwoUnderscoresEncountered()
    {
        var tokens = Tokens("__");
        tokens[0].Type.Should().Be(TokenType.DoubleUnderscore);
    }


    [Test]
    public void Lexer_SplitIntoDoubleAndSingleUnderscore_WhenThreeUnderscoresEncountered()
    {
        Types("___").Should().Equal(
            TokenType.DoubleUnderscore,
            TokenType.Underscore,
            TokenType.EndOfFile
        );
    }


    [Test]
    public void Lexer_ProduceEscapeToken_WhenBackslashEncountered()
    {
        var tokens = Tokens("\\_");
        tokens[0].Type.Should().Be(TokenType.Escape);
        tokens[1].Type.Should().Be(TokenType.Underscore);
    }


    [Test]
    public void Lexer_RecognizeBracketAndParenTokens_WhenBracketCharactersEncountered()
    {
        Types("[]()").Should().Equal(
            TokenType.OpenBracket,
            TokenType.CloseBracket,
            TokenType.OpenParen,
            TokenType.CloseParen,
            TokenType.EndOfFile
        );
    }

    [Test]
    public void Lexer_ProduceAutoLinkTokens_ForAngles()
    {
        var tokens = Tokens("<https://example.com>");
        tokens.Select(t => t.Type).Should().Equal(
            TokenType.AutoLinkOpen,
            TokenType.Text,
            TokenType.AutoLinkClose,
            TokenType.EndOfFile);
        tokens[1].Value.Should().Be("https://example.com");
    }

    [Test]
    public void Lexer_EmitSeparateTokens_ForInvalidAutoLink()
    {
        var tokens = Tokens("<ftp://example.com>");
        tokens.Select(t => t.Type).Should().Equal(
            TokenType.AutoLinkOpen,
            TokenType.Text,
            TokenType.AutoLinkClose,
            TokenType.EndOfFile);
        tokens[1].Value.Should().Be("ftp://example.com");
    }
}