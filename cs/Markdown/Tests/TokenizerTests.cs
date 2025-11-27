using Markdown.Tokenizing;
using Markdown.Tokenizing.Tokens;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class TokenizerTests
{
    private void PrintTokens(List<Token> tokens)
    {
        var index = 1;
        foreach (var token in tokens)
        {
            Console.WriteLine($"{index++}. {token.Type}: \"{token.Value}\"");
        }
    }

    private void AssertTokensAreEqual(List<Token> expected, List<Token> actual)
    {
        Console.WriteLine("Actual");
        PrintTokens(actual);
        Console.WriteLine("Expected");
        PrintTokens(expected);
        Assert.That(expected.Count, Is.EqualTo(actual.Count), "Token count mismatch");
        for (var i = 0; i < expected.Count; i++)
        {
            Assert.That(expected[i].Type, Is.EqualTo(actual[i].Type), $"Token {i} type mismatch");
            Assert.That(expected[i].Value, Is.EqualTo(actual[i].Value), $"Token {i} value mismatch");
        }
    }

    [Test]
    public void Tokenizer_SimpleString()
    {
        var text = "Hello, world";
        var expected = new List<Token>
        {
            new Token { Type = TokenType.Text, Value = "Hello," },
            new Token { Type = TokenType.WhiteSpace },
            new Token { Type = TokenType.Text, Value = "world" }
        };
        var tokens = new Tokenizer().Tokenize(text);
        AssertTokensAreEqual(expected, tokens);
    }

    [Test]
    public void Tokenizer_CommonAndBold()
    {
        var text = "Hello, __world__";
        var expected = new List<Token>
        {
            new Token { Type = TokenType.Text, Value = "Hello," },
            new Token { Type = TokenType.WhiteSpace },
            new Token { Type = TokenType.Underscore },
            new Token { Type = TokenType.Underscore },
            new Token { Type = TokenType.Text, Value = "world" },
            new Token { Type = TokenType.Underscore },
            new Token { Type = TokenType.Underscore },
        };
        var tokens = new Tokenizer().Tokenize(text);
        AssertTokensAreEqual(expected, tokens);
    }

    [Test]
    public void Tokenizer_IncompleteBold()
    {
        var text = "Hello, __world";
        var expected = new List<Token>
        {
            new Token { Type = TokenType.Text, Value = "Hello," },
            new Token { Type = TokenType.WhiteSpace },
            new Token { Type = TokenType.Underscore },
            new Token { Type = TokenType.Underscore },
            new Token { Type = TokenType.Text, Value = "world" }
        };
        var tokens = new Tokenizer().Tokenize(text);
        AssertTokensAreEqual(expected, tokens);
    }

    [Test]
    public void Tokenizer_ItalicInBold()
    {
        var text = "Hello, __world _brbrbr___";
        var expected = new List<Token>
        {
            new Token { Type = TokenType.Text, Value = "Hello," },
            new Token { Type = TokenType.WhiteSpace },
            new Token { Type = TokenType.Underscore },
            new Token { Type = TokenType.Underscore },
            new Token { Type = TokenType.Text, Value = "world" },
            new Token { Type = TokenType.WhiteSpace },
            new Token { Type = TokenType.Underscore },
            new Token { Type = TokenType.Text, Value = "brbrbr" },
            new Token { Type = TokenType.Underscore },
            new Token { Type = TokenType.Underscore },
            new Token { Type = TokenType.Underscore }
        };
        var tokens = new Tokenizer().Tokenize(text);
        AssertTokensAreEqual(expected, tokens);
    }

    [Test]
    public void Tokenizer_Header()
    {
        var text = "# Hello, world";
        var expected = new List<Token>
        {
            new Token { Type = TokenType.Hashtag },
            new Token { Type = TokenType.WhiteSpace },
            new Token { Type = TokenType.Text, Value = "Hello," },
            new Token { Type = TokenType.WhiteSpace },
            new Token { Type = TokenType.Text, Value = "world" }
        };
        var tokens = new Tokenizer().Tokenize(text);
        AssertTokensAreEqual(expected, tokens);
    }

    [Test]
    public void Tokenizer_Link()
    {
        var text = "[Google](https://google.com)";
        var expected = new List<Token>
        {
            new Token { Type = TokenType.LeftSquareBracket },
            new Token { Type = TokenType.Text, Value = "Google" },
            new Token { Type = TokenType.RightSquareBracket },
            new Token { Type = TokenType.LeftBracket },
            new Token { Type = TokenType.Text, Value = "https://google.com" },
            new Token { Type = TokenType.RightBracket }
        };
        var tokens = new Tokenizer().Tokenize(text);
        AssertTokensAreEqual(expected, tokens);
    }
}