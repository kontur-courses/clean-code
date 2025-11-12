using FluentAssertions;
using Markdown.Tokenizing;
using NUnit.Framework;

namespace Markdown.Tests;

public class TokenizerTests
{
    private Tokenizer tokenizer = null!;

    [SetUp]
    public void SetUp()
        => tokenizer = new Tokenizer();

    private static List<Token> Tokens(string text)
        => Tokenizer.Tokenize(text);

    private static List<TokenType> Types(string text)
        => Tokenizer.Tokenize(text).Select(t => t.Type).ToList();


    [TestCase("hello!")]
    public void Tokenizer_ShouldProduceTextToken_WhenSimpleWord_Test(string input)
    {
        var tokens = Tokens(input);
        tokens.Should().HaveCount(2);
        tokens[0].Type.Should().Be(TokenType.Text);
        tokens[0].Value.Should().Be(input);
    }


    [TestCase("a b")]
    public void Tokenizer_ShouldProduceWhitespaceToken_WhenSpaceEncountered_Test(string input)
    {
        var tokens = Tokens(input);
        tokens[1].Type.Should().Be(TokenType.Whitespace);
        tokens[1].Value.Should().Be(" ");
    }


    [TestCase("a\tb")]
    public void Tokenizer_ShouldProduceWhitespaceTokenWithTabValue_WhenTabEncountered_Test(string input)
    {
        var tokens = Tokens(input);
        tokens[1].Type.Should().Be(TokenType.Whitespace);
        tokens[1].Value.Should().Be("\t");
    }


    [TestCase("a\nb")]
    public void Tokenizer_ShouldProduceEndOfLineToken_WhenNewlineEncountered_Test(string input)
    {
        var tokens = Tokens(input);
        tokens[1].Type.Should().Be(TokenType.EndOfLine);
        tokens[2].Type.Should().Be(TokenType.Text);
    }


    [TestCase("# hello!")]
    public void Tokenizer_ShouldProduceHashToken_WhenHashEncountered_Test(string input)
    {
        var tokens = Tokens(input);
        tokens[0].Type.Should().Be(TokenType.Hash);
        tokens[1].Type.Should().Be(TokenType.Whitespace);
        tokens[2].Type.Should().Be(TokenType.Text);
    }


    [TestCase("")]
    [TestCase(" ")]
    [TestCase("hello!")]
    public void Tokenizer_ShouldPlaceEndOfFileTokenLast_WhenTokenizingAnyInput_Test(string input)
    {
        var tokens = Tokens(input);
        tokens[^1].Type.Should().Be(TokenType.EndOfFile);
    }


    [Test]
    public void Tokenizer_ShouldProduceUnderscoreToken_WhenSingleUnderscoreEncountered_Test()
    {
        var tokens = Tokens("_");
        tokens[0].Type.Should().Be(TokenType.Underscore);
    }


    [Test]
    public void Tokenizer_ShouldProduceDoubleUnderscoreToken_WhenTwoUnderscoresEncountered_Test()
    {
        var tokens = Tokens("__");
        tokens[0].Type.Should().Be(TokenType.DoubleUnderscore);
    }


    [Test]
    public void Tokenizer_ShouldSplitIntoDoubleAndSingleUnderscore_WhenThreeUnderscoresEncountered_Test()
    {
        Types("___").Should().Equal(
            TokenType.DoubleUnderscore,
            TokenType.Underscore,
            TokenType.EndOfFile
        );
    }


    [Test]
    public void Tokenizer_ShouldProduceEscapeToken_WhenBackslashEncountered_Test()
    {
        var tokens = Tokens("\\_");
        tokens[0].Type.Should().Be(TokenType.Escape);
        tokens[1].Type.Should().Be(TokenType.Underscore);
    }


    [Test]
    public void Tokenizer_ShouldRecognizeBracketAndParenTokens_WhenBracketCharactersEncountered_Test()
    {
        Types("[]()").Should().Equal(
            TokenType.LeftBracket,
            TokenType.RightBracket,
            TokenType.LeftParen,
            TokenType.RightParen,
            TokenType.EndOfFile
        );
    }
}