using Markdown.Tokenizer;
using NUnit.Framework;

namespace Markdown.Tests.TokenizerTests;

[TestFixture]
public class TokenizeImageTests
{
    [TestCase("![Cat](https://example.com/image.jpg)")]
    public void Tokenize_ValidUrlAndAlt(string markdown)
    {
        var expected = new List<Token>
        {
            new("!", TokenType.Exclamation),
            new("[", TokenType.LBracket),
            new("Cat", TokenType.Text),
            new("]", TokenType.RBracket),
            new("(", TokenType.LParenthesis),
            new("https://example.com/image.jpg", TokenType.Text),
            new(")", TokenType.RParenthesis),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("!\n[Cat](https://example.com/image.jpg)")]
    public void Tokenize_NewlineAfterExclamation(string markdown)
    {
        var expected = new List<Token>
        {
            new("!", TokenType.Text),
            new("\n", TokenType.NewLine),
            new("[", TokenType.LBracket),
            new("Cat", TokenType.Text),
            new("]", TokenType.RBracket),
            new("(", TokenType.LParenthesis),
            new("https://example.com/image.jpg", TokenType.Text),
            new(")", TokenType.RParenthesis),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("![C[]at](https://example.com/image.jpg)")]
    public void Tokenize_AltWithBrackets(string markdown)
    {
        var expected = new List<Token>
        {
            new("!", TokenType.Exclamation),
            new("[", TokenType.LBracket),
            new("C", TokenType.Text),
            new("[", TokenType.LBracket),
            new("]", TokenType.RBracket),
            new("at", TokenType.Text),
            new("]", TokenType.RBracket),
            new("(", TokenType.LParenthesis),
            new("https://example.com/image.jpg", TokenType.Text),
            new(")", TokenType.RParenthesis),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("![Cat](https://example.com()/image.jpg)")]
    public void Tokenize_UrlWithParenthesis(string markdown)
    {
        var expected = new List<Token>
        {
            new("!", TokenType.Exclamation),
            new("[", TokenType.LBracket),
            new("Cat", TokenType.Text),
            new("]", TokenType.RBracket),
            new("(", TokenType.LParenthesis),
            new("https://example.com", TokenType.Text),
            new("(", TokenType.LParenthesis),
            new(")", TokenType.RParenthesis),
            new("/image.jpg", TokenType.Text),
            new(")", TokenType.RParenthesis),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("![](https://example.com/image.jpg)")]
    public void Tokenize_EmptyAltText(string markdown)
    {
        var expected = new List<Token>
        {
            new("!", TokenType.Exclamation),
            new("[", TokenType.LBracket),
            new("]", TokenType.RBracket),
            new("(", TokenType.LParenthesis),
            new("https://example.com/image.jpg", TokenType.Text),
            new(")", TokenType.RParenthesis),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase("![__Cat__](https://example.com/image.jpg)")]
    public void Tokenize_AltWithBold(string markdown)
    {
        var expected = new List<Token>
        {
            new("!", TokenType.Exclamation),
            new("[", TokenType.LBracket),
            new("__", TokenType.DoubleUnderscore),
            new("Cat", TokenType.Text),
            new("__", TokenType.DoubleUnderscore),
            new("]", TokenType.RBracket),
            new("(", TokenType.LParenthesis),
            new("https://example.com/image.jpg", TokenType.Text),
            new(")", TokenType.RParenthesis),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }
}