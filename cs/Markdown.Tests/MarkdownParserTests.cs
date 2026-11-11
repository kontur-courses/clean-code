using System.Collections.Generic;
using Moq;
using Moq.Language.Flow;


namespace Markdown.Tests;

public class MarkdownParserTests
{
    private readonly ISetup<ILexer, List<Token>> getTokensSetup;
    private readonly MarkdownParser sut;

    public MarkdownParserTests()
    {
        var lexer = new Mock<ILexer>();
        getTokensSetup = lexer.Setup(l => l.GetTokens(It.IsAny<string>()));
        sut = new MarkdownParser(lexer.Object);
    }

    [Test]
    public void Parse_ShouldAllowItalicInsideBold()
    {
        var text = "Внутри __двойного _одинарное_ тоже__ работает.";

        getTokensSetup.Returns(new List<Token>
        {
            new Token(TokenType.Word, "Внутри"),
            new Token(TokenType.Space),
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.Word, "двойного"),
            new Token(TokenType.Space),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "одинарное"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "тоже"),
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "работает.")
        });

        var expected = new[]
        {
            new MarkupToken(MarkupTokenType.Word, "Внутри "),
            new MarkupToken(MarkupTokenType.OpenBoldSymbol),
            new MarkupToken(MarkupTokenType.Word, "двойного "),
            new MarkupToken(MarkupTokenType.OpenItalicSymbol),
            new MarkupToken(MarkupTokenType.Word, "одинарное"),
            new MarkupToken(MarkupTokenType.CloseItalicSymbol),
            new MarkupToken(MarkupTokenType.Word, " тоже"),
            new MarkupToken(MarkupTokenType.CloseBoldSymbol),
            new MarkupToken(MarkupTokenType.Word, " работает.")
        };

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ShouldKeepBoldMarkersInsideItalicAsText()
    {
        var text = "Внутри _одинарного __двойное__ не_ работает.";

        getTokensSetup.Returns(new List<Token>
        {
            new Token(TokenType.Word, "Внутри"),
            new Token(TokenType.Space),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "одинарного"),
            new Token(TokenType.Space),
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.Word, "двойное"),
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "не"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "работает.")
        });

        var expected = new[]
        {
            new MarkupToken(MarkupTokenType.Word, "Внутри "),
            new MarkupToken(MarkupTokenType.OpenItalicSymbol),
            new MarkupToken(MarkupTokenType.Word, "одинарного __двойное__ не"),
            new MarkupToken(MarkupTokenType.CloseItalicSymbol),
            new MarkupToken(MarkupTokenType.Word, " работает.")
        };

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ShouldKeepMarkersInsideWordsWithDigitsAsText()
    {
        var text = "Подчерки внутри текста c цифрами_12_3 не считаются выделением";

        getTokensSetup.Returns(new List<Token>
        {
            new Token(TokenType.Word, "Подчерки"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "внутри"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "текста"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "c"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "цифрами"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "12"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "3"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "не"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "считаются"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "выделением"),
        });

        var expected = new[]
        {
            new MarkupToken(MarkupTokenType.Word, text)
        };

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ShouldAllowItalicInsideWords()
    {
        var text = "Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._";

        getTokensSetup.Returns(new List<Token>
        {
            new Token(TokenType.Word, "Однако"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "выделять"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "часть"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "слова"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "они"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "могут:"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "и"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "в"),
            new Token(TokenType.Space),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "нач"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "але,"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "и"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "в"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "сер"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "еди"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "не,"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "и"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "в"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "кон"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "це."),
            new Token(TokenType.ItalicSymbol)
        });

        var expected = new[]
        {
            new MarkupToken(
                MarkupTokenType.Word,
                "Однако выделять часть слова они могут: и в "),
            new MarkupToken(MarkupTokenType.OpenItalicSymbol),
            new MarkupToken(MarkupTokenType.Word, "нач"),
            new MarkupToken(MarkupTokenType.CloseItalicSymbol),
            new MarkupToken(MarkupTokenType.Word, "але, и в сер"),
            new MarkupToken(MarkupTokenType.OpenItalicSymbol),
            new MarkupToken(MarkupTokenType.Word, "еди"),
            new MarkupToken(MarkupTokenType.CloseItalicSymbol),
            new MarkupToken(MarkupTokenType.Word, "не, и в кон"),
            new MarkupToken(MarkupTokenType.OpenItalicSymbol),
            new MarkupToken(MarkupTokenType.Word, "це."),
            new MarkupToken(MarkupTokenType.CloseItalicSymbol)
        };

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ShouldKeepMarkersInsideDifferentWordsAsText()
    {
        var text = "Выделение в ра_зных сл_овах не работает.";

        getTokensSetup.Returns(new List<Token>
        {
            new Token(TokenType.Word, "Выделение"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "в"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "ра"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "зных"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "сл"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "овах"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "не"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "работает.")
        });

        var expected = new[]
        {
            new MarkupToken(MarkupTokenType.Word, text)
        };

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ShouldKeepUnpairedMarkersAsText()
    {
        var text = "__Непарные_ не считаются выделением.";

        getTokensSetup.Returns(new List<Token>
        {
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.Word, "Непарные"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "не"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "считаются"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "выделением.")
        });

        var expected = new[]
        {
            new MarkupToken(MarkupTokenType.Word, text)
        };

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ShouldKeepOpeningMarkersBeforeWhitespaceAsText()
    {
        var text = "Эти_ подчерки_ не считаются выделением";

        getTokensSetup.Returns(new List<Token>
        {
            new Token(TokenType.Word, "Эти"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "подчерки"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "не"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "считаются"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "выделением"),
        });

        var expected = new[]
        {
            new MarkupToken(MarkupTokenType.Word, text)
        };

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    public void Parse_ShouldNotCloseItalicAfterWhitespace()
    {
        var text = "Эти _подчерки _не считаются_ окончанием выделения ";

        getTokensSetup.Returns(new List<Token>
        {
            new Token(TokenType.Word, "Эти"),
            new Token(TokenType.Space),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "подчерки"),
            new Token(TokenType.Space),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "не"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "считаются"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "окончанием"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "выделения"),
            new Token(TokenType.Space)
        });

        var expected = new[]
        {
            new MarkupToken(MarkupTokenType.Word, "Эти _подчерки "),
            new MarkupToken(MarkupTokenType.OpenItalicSymbol),
            new MarkupToken(MarkupTokenType.Word, "не считаются"),
            new MarkupToken(MarkupTokenType.CloseItalicSymbol),
            new MarkupToken(MarkupTokenType.Word, " окончанием выделения ")
        };

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ShouldKeepCrossingMarkersAsText()
    {
        var text = "В случае __пересечения _двойных__ и одинарных_ подчерков они не считаются тэгами.";

        getTokensSetup.Returns(new List<Token>
        {
            new Token(TokenType.Word, "В"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "случае"),
            new Token(TokenType.Space),
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.Word, "пересечения"),
            new Token(TokenType.Space),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "двойных"),
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "и"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "одинарных"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "подчерков"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "они"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "не"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "считаются"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "тэгами.")
        });

        var expected = new[]
        {
            new MarkupToken(MarkupTokenType.Word, text)
        };

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ShouldKeepMarkersAsText_WhenContentIsEmpty()
    {
        var text = "Если внутри подчерков пустая строка ____ то они не считаются тэгами";

        getTokensSetup.Returns(new List<Token>
        {
            new Token(TokenType.Word, "Если"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "внутри"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "подчерков"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "пустая"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "строка"),
            new Token(TokenType.Space),
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "то"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "они"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "не"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "считаются"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "тэгами")
        });

        var expected = new[]
        {
            new MarkupToken(MarkupTokenType.Word, text)
        };

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }
}