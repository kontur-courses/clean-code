using Moq;
using Moq.Language.Flow;

namespace Markdown.Tests;

public class MarkdownParserTests
{
    private readonly ISetup<ILexer, IEnumerable<Token>> getTokensSetup;
    private readonly MarkdownParser sut;

    public MarkdownParserTests()
    {
        var lexer = new Mock<ILexer>();
        getTokensSetup = lexer.Setup(l => l.GetTokens(It.IsAny<string>()));
        sut = new MarkdownParser(lexer.Object);
    }
    
    [Test]
    public void Parse_ShouldRecognizeItalic()
    {
        var text = "_text_";
        
        getTokensSetup.Returns(new List<Token>
        {
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "text"),
            new Token(TokenType.ItalicSymbol)
        });

        var expected = new[]
        {
            new MarkupToken(MarkupTokenType.OpenItalicSymbol),
            new MarkupToken(MarkupTokenType.Word, "text"),
            new MarkupToken(MarkupTokenType.CloseItalicSymbol)
        };
        
        var actual = sut.Parse(text);
        
        Assert.That(actual, Is.EqualTo(expected));

    }
    
    [Test]
    public void Parse_ShouldRecognizeBold()
    {
        var text = "__text__";

        getTokensSetup.Returns(new List<Token>
        {
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.Word, "text"),
            new Token(TokenType.BoldSymbol)
        });

        var expected = new[]
        {
            new MarkupToken(MarkupTokenType.OpenBoldSymbol),
            new MarkupToken(MarkupTokenType.Word, "text"),
            new MarkupToken(MarkupTokenType.CloseBoldSymbol)
        };

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ShouldKeepUnpairedMarkersAsText()
    {
        var text = "_text";

        getTokensSetup.Returns(new List<Token>
        {
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "text")
        });

        var expected = new[]
        {
            new MarkupToken(MarkupTokenType.Word, "_text")
        };

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ShouldKeepMarkersAsText_WhenContentIsEmpty()
    {
        var text = "____";

        getTokensSetup.Returns(new List<Token>
        {
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.BoldSymbol)
        });

        var expected = new[]
        {
            new MarkupToken(MarkupTokenType.Word, "____")
        };

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ShouldAllowItalicInsideBold()
    {
        var text = "__a_b_c__";

        getTokensSetup.Returns(new List<Token>
        {
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.Word, "a"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "b"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "c"),
            new Token(TokenType.BoldSymbol)
        });

        var expected = new[]
        {
            new MarkupToken(MarkupTokenType.OpenBoldSymbol),
            new MarkupToken(MarkupTokenType.Word, "a"),
            new MarkupToken(MarkupTokenType.OpenItalicSymbol),
            new MarkupToken(MarkupTokenType.Word, "b"),
            new MarkupToken(MarkupTokenType.CloseItalicSymbol),
            new MarkupToken(MarkupTokenType.Word, "c"),
            new MarkupToken(MarkupTokenType.CloseBoldSymbol)
        };

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ShouldKeepBoldMarkersInsideItalicAsText()
    {
        var text = "_a__b__c_";

        getTokensSetup.Returns(new List<Token>
        {
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "a"),
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.Word, "b"),
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.Word, "c"),
            new Token(TokenType.ItalicSymbol)
        });

        var expected = new[]
        {
            new MarkupToken(MarkupTokenType.OpenItalicSymbol),
            new MarkupToken(MarkupTokenType.Word, "a__b__c"),
            new MarkupToken(MarkupTokenType.CloseItalicSymbol)
        };

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ShouldKeepCrossingMarkersAsText()
    {
        var text = "__a_b__c_";

        getTokensSetup.Returns(new List<Token>
        {
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.Word, "a"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "b"),
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.Word, "c"),
            new Token(TokenType.ItalicSymbol)
        });

        var expected = new[]
        {
            new MarkupToken(MarkupTokenType.Word, "__a_b__c_")
        };

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }
}