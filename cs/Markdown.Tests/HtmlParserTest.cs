using System.Collections.Generic;
using Moq;
using Moq.Language.Flow;

namespace Markdown.Tests;

public class HtmlParserTest
{
    private readonly HtmlParser sut;
    private readonly ISetup<IParserFrom, List<MarkupToken>> parseSetup;

    public HtmlParserTest()
    {
        var parserFrom = new Mock<IParserFrom>();
        parseSetup = parserFrom.Setup(p => p.Parse(It.IsAny<string>()));
        sut = new HtmlParser(parserFrom.Object);
    }

    [Test]
    public void Parse_ShouldReturnPlainText()
    {
        var text = "Обычный текст.";
        var tokens = new List<MarkupToken>
        {
            new MarkupToken(MarkupTokenType.Word, "Обычный текст.")
        };

        var expected = "Обычный текст.";
        
        parseSetup.Returns(tokens);

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ShouldRenderItalic()
    {
        var text = "_курсив_";
        var tokens = new List<MarkupToken>
        {
            new MarkupToken(MarkupTokenType.OpenItalicSymbol),
            new MarkupToken(MarkupTokenType.Word, "курсив"),
            new MarkupToken(MarkupTokenType.CloseItalicSymbol)
        };

        var expected = "<em>курсив</em>";
        
        parseSetup.Returns(tokens);

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ShouldRenderBold()
    {
        var text = "__полужирный__";
        var tokens = new List<MarkupToken>
        {
            new MarkupToken(MarkupTokenType.OpenBoldSymbol),
            new MarkupToken(MarkupTokenType.Word, "полужирный"),
            new MarkupToken(MarkupTokenType.CloseBoldSymbol)
        };
        
        parseSetup.Returns(tokens);
        
        var expected = "<strong>полужирный</strong>";

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ShouldRenderItalicInsideBold()
    {
        var text = "Внутри __двойного _одинарное_ тоже__ работает.";
        var tokens = new List<MarkupToken>
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

        parseSetup.Returns(tokens);
        
        var expected =
            "Внутри <strong>двойного <em>одинарное</em> тоже</strong> работает.";

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ShouldRenderHeader()
    {
        var text = "# Заголовок";
        var tokens = new List<MarkupToken>
        {
            new MarkupToken(MarkupTokenType.OpenHeaderSymbol),
            new MarkupToken(MarkupTokenType.Word, "Заголовок"),
            new MarkupToken(MarkupTokenType.CloseHeaderSymbol),
        };

        parseSetup.Returns(tokens);
        
        var expected = "<h1>Заголовок</h1>";

        var actual = sut.Parse(text);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Parse_ShouldReturnEmptyString_WhenNoTokens()
    {
        var tokens = new List<MarkupToken>();

        parseSetup.Returns(tokens);
        
        var expected = "";

        var actual = sut.Parse("");

        Assert.That(actual, Is.EqualTo(expected));
    }
}
