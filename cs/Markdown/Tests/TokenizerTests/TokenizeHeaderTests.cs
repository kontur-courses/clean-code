using Markdown.Tokenizer;
using NUnit.Framework;

namespace Markdown.Tests.TokenizerTests;

[TestFixture]
public class TokenizeHeaderTests
{
    [TestCase("# текст")]
    public void Tokenize_SimpleHeader_ReturnsCorrectTokens(string markdownText)
    {
        var expectedTokens = new List<Token>
        {
            new("#", TokenType.Hash),
            new(" ", TokenType.Space),
            new("текст", TokenType.Text),
            new("", TokenType.Eof)
        };

        TokenAssert.AssertToken(markdownText, expectedTokens);
    }

    [TestCase("#текст")]
    public void Tokenize_NoSpaceAfterHash_ReturnsCorrectTokens(string markdownText)
    {
        var expectedTokens = new List<Token>
        {
            new("#", TokenType.Text),
            new("текст", TokenType.Text),
            new("", TokenType.Eof)
        };

        TokenAssert.AssertToken(markdownText, expectedTokens);
    }

    [TestCase("# текст \n# текст")]
    public void Tokenize_TwoHeadersInDifferentLines_ReturnsCorrectTokens(string markdownText)
    {
        var expectedTokens = new List<Token>
        {
            new("#", TokenType.Hash),
            new(" ", TokenType.Space),
            new("текст", TokenType.Text),
            new(" ", TokenType.Space),
            new("\n", TokenType.NewLine),
            new("#", TokenType.Hash),
            new(" ", TokenType.Space),
            new("текст", TokenType.Text),
            new("", TokenType.Eof)
        };

        TokenAssert.AssertToken(markdownText, expectedTokens);
    }

    [TestCase(" # текст")]
    public void Tokenize_X_ReturnsCorrectTokens(string markdownText)
    {
        var expectedTokens = new List<Token>
        {
            new(" ", TokenType.Space),
            new("#", TokenType.Text),
            new(" ", TokenType.Space),
            new("текст", TokenType.Text),
            new("", TokenType.Eof)
        };

        TokenAssert.AssertToken(markdownText, expectedTokens);
    }

    [TestCase("#   текст")]
    public void Tokenize_ManySpacesAfterHash_ReturnsCorrectTokens(string markdownText)
    {
        var expectedTokens = new List<Token>
        {
            new("#", TokenType.Hash),
            new(" ", TokenType.Space),
            new(" ", TokenType.Space),
            new(" ", TokenType.Space),
            new("текст", TokenType.Text),
            new("", TokenType.Eof)
        };

        TokenAssert.AssertToken(markdownText, expectedTokens);
    }
}