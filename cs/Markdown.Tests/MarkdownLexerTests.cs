using System.Collections.Generic;

namespace Markdown.Tests;

public class MarkdownLexerTests
{
    private readonly MarkdownLexer sut = new MarkdownLexer();

    [Test]
    public void GetTokens_ShouldTokenizeTextWithItalicMarkers()
    {
        var text = "_123 2_";
        var expected = new List<Token>
        {
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "123"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "2"),
            new Token(TokenType.ItalicSymbol)
        };

        var actual = sut.GetTokens(text);
        
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void GetTokens_ShouldTokenizeTextWithBoldMarkers()
    {
        var text = "__123 33__";
        var expected = new List<Token>
        {
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.Word, "123"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "33"),
            new Token(TokenType.BoldSymbol)
        };

        var actual = sut.GetTokens(text);

        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void GetTokens_ShouldTokenizeHeaderMarker()
    {
        var text = "# 123 123";
        var expected = new List<Token>
        {
            new Token(TokenType.Header),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "123"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "123")
        };

        var actual = sut.GetTokens(text);

        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void GetTokens_ShouldTokenizeConsecutiveBoldMarkers()
    {
        var text = "qwe______123";
        var expected = new List<Token>
        {
            new Token(TokenType.Word, "qwe"),
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.Word, "123")
        };

        var actual = sut.GetTokens(text);

        Assert.That(actual, Is.EqualTo(expected));
    }
    
    
    [Test]
    public void GetTokens_ShouldTokenizeNewLine()
    {
        var text = "# 123 123\r\n123";
        var expected = new List<Token>
        {
            new Token(TokenType.Header),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "123"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "123"),
            new Token(TokenType.NewLine),
            new Token(TokenType.Word, "123")
        };

        var actual = sut.GetTokens(text);

        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void GetTokens_ShouldHandleEscaping()
    {
        var text = @"\__123";
        var expected = new List<Token>
        {
            new Token(TokenType.Word, "_"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "123")
        };

        var actual = sut.GetTokens(text);

        Assert.That(actual, Is.EqualTo(expected));
    }
 
    
    [Test]
    public void GetTokens_ShouldTokenizeSeveralNewLines()
    {
        var text = "asd\r\n\r\nasd";
        var expected = new List<Token>
        {
            new Token(TokenType.Word, "asd"),
            new Token(TokenType.NewLine),
            new Token(TokenType.NewLine),
            new Token(TokenType.Word, "asd")
        };

        var actual = sut.GetTokens(text);

        Assert.That(actual, Is.EqualTo(expected));
    }
  
}
