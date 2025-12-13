using Markdown.Tokenizer;
using NUnit.Framework;

namespace Markdown.Tests.TokenizerTests;

[TestFixture]
public class TokenizeEscapeTests
{
    [TestCase(@"\_текст_")]
    public void Tokenize_EscapedUnderscore(string markdown)
    {
        var expected = new List<Token>
        {
            new(@"\", TokenType.Escape),
            new("_", TokenType.Underscore),
            new("текст", TokenType.Text),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase(@"\\_текст\\_")]
    public void Tokenize_DoubleEscapedUnderscoresAroundText(string markdown)
    {
        var expected = new List<Token>
        {
            new(@"\", TokenType.Escape),
            new(@"\", TokenType.Escape),
            new("_", TokenType.Underscore),
            new("текст", TokenType.Text),
            new(@"\", TokenType.Escape),
            new(@"\", TokenType.Escape),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase(@"\# текст")]
    public void Tokenize_EscapedHeader(string markdown)
    {
        var expected = new List<Token>
        {
            new(@"\", TokenType.Escape),
            new("#", TokenType.Text),
            new(" ", TokenType.Space),
            new("текст", TokenType.Text),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase(@"\![Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)")]
    public void Tokenize_EscapedExclamationInImage(string markdown)
    {
        var expected = new List<Token>
        {
            new(@"\", TokenType.Escape),
            new("!", TokenType.Exclamation),
            new("[", TokenType.LBracket),
            new("Cat", TokenType.Text),
            new("]", TokenType.RBracket),
            new("(", TokenType.LParenthesis),
            new("https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg", TokenType.Text),
            new(")", TokenType.RParenthesis),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase(@"!\[Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)")]
    public void Tokenize_EscapedAltInImage(string markdown)
    {
        var expected = new List<Token>
        {
            new("!", TokenType.Text),
            new(@"\", TokenType.Escape),
            new("[", TokenType.LBracket),
            new("Cat", TokenType.Text),
            new("]", TokenType.RBracket),
            new("(", TokenType.LParenthesis),
            new("https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg", TokenType.Text),
            new(")", TokenType.RParenthesis),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase(@"![Cat]\(https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)")]
    public void Tokenize_EscapedUrlInImage(string markdown)
    {
        var expected = new List<Token>
        {
            new("!", TokenType.Exclamation),
            new("[", TokenType.LBracket),
            new("Cat", TokenType.Text),
            new("]", TokenType.RBracket),
            new(@"\", TokenType.Escape),
            new("(", TokenType.LParenthesis),
            new("https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg", TokenType.Text),
            new(")", TokenType.RParenthesis),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }

    [TestCase(@"т\екс\т")]
    public void Tokenize_TextWithMultipleEscapes(string markdown)
    {
        var expected = new List<Token>
        {
            new("т", TokenType.Text),
            new(@"\", TokenType.Text),
            new("екс", TokenType.Text),
            new(@"\", TokenType.Text),
            new("т", TokenType.Text),
            new("", TokenType.Eof)
        };
        TokenAssert.AssertToken(markdown, expected);
    }
}