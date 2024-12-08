using FluentAssertions;
using Markdown;
using Markdown.interfaces;
using Markdown.MarkDownConverter;
using Markdown.Token;
using NUnit.Framework;

namespace MarkDownTest;

public class MarkdownConverterTests
{
    private IMarkdownConverter _converter;

    [SetUp]
    public void Setup()
    {
        _converter = new MarkdownConverter();
    }

    [Test]
    public void Convert_TextOnly_ShouldReturnPlainText()
    {
        var tokens = new List<Token>
        {
            Token.CreateText("Hello, World!", 0)
        };

        var result = _converter.Convert(tokens);

        result.Should().Be("Hello, World!");
    }

    [Test]
    public void Convert_StrongText_ShouldReturnStrongHtml()
    {
        var tokens = new List<Token>
        {
            Token.CreateStrong(true, 0),
            Token.CreateText("bold", 2),
            Token.CreateStrong(false, 6)
        };

        var result = _converter.Convert(tokens);

        result.Should().Be("<strong>bold</strong>");
    }

    [Test]
    public void Convert_ItalicText_ShouldReturnItalicHtml()
    {
        var tokens = new List<Token>
        {
            Token.CreateItalic(true, 0),
            Token.CreateText("italic", 1),
            Token.CreateItalic(false, 7)
        };

        var result = _converter.Convert(tokens);

        result.Should().Be("<em>italic</em>");
    }

    [Test]
    public void Convert_StrongAndItalicNested_ShouldReturnNestedHtml()
    {
        var tokens = new List<Token>
        {
            Token.CreateStrong(true, 0),
            Token.CreateText("bold ", 2),
            Token.CreateItalic(true, 7),
            Token.CreateText("italic", 8),
            Token.CreateItalic(false, 14),
            Token.CreateText(" bold", 15),
            Token.CreateStrong(false, 20)
        };

        var result = _converter.Convert(tokens);

        result.Should().Be("<strong>bold <em>italic</em> bold</strong>");
    }

    [Test]
    public void Convert_Header_ShouldReturnHeaderHtml()
    {
        var tokens = new List<Token>
        {
            Token.CreateHeader(2, 0),
            Token.CreateText("Header Text", 2),
            new Token("", TokenType.Header, TagState.Close, 13, 2)
        };

        var result = _converter.Convert(tokens);

        result.Should().Be("<h2>Header Text</h2>");
    }

    [Test]
    public void Convert_HeaderWithFormatting_ShouldReturnFormattedHeaderHtml()
    {
        var tokens = new List<Token>
        {
            Token.CreateHeader(1, 0),
            Token.CreateText("Welcome to ", 1),
            Token.CreateItalic(true, 12),
            Token.CreateText("Markdown", 13),
            Token.CreateItalic(false, 21),
            new Token("", TokenType.Header, TagState.Close, 22, 1)
        };

        var result = _converter.Convert(tokens);

        result.Should().Be("<h1>Welcome to <em>Markdown</em></h1>");
    }

    [Test]
    public void Convert_IncorrectClosingTag_ShouldAddAsText()
    {
        var tokens = new List<Token>
        {
            Token.CreateStrong(true, 0),
            Token.CreateText("bold ", 2),
            Token.CreateItalic(true, 7),
            Token.CreateText("italic", 8),
            Token.CreateStrong(false, 14),
            Token.CreateText(" text_", 16)
        };

        var result = _converter.Convert(tokens);


        result.Should().Be("<strong>bold <em>italic__ text_</em></strong>");
    }

    [Test]
    public void Convert_UnmatchedClosingTag_ShouldAddAsText()
    {
        var tokens = new List<Token>
        {
            Token.CreateItalic(false, 0),
            Token.CreateText("text_", 1)
        };

        var result = _converter.Convert(tokens);

        result.Should().Be("_text_");
    }

    [Test]
    public void Convert_UnclosedTags_ShouldCloseRemainingTags()
    {
        var tokens = new List<Token>
        {
            Token.CreateStrong(true, 0),
            Token.CreateText("bold", 2)
        };

        var result = _converter.Convert(tokens);

        result.Should().Be("<strong>bold</strong>");
    }

    [Test]
    public void Convert_Link_ShouldReturnLinkHtml()
    {
        var tokens = new List<Token>
        {
            Token.CreateLink("пример ссылки", "https://example.com", 0)
        };

        var result = _converter.Convert(tokens);

        result.Should().Be("<a href=\"https://example.com\">пример ссылки</a>");
    }
    
    
    [Test]
    public void Convert_StrongTextWithLink_ShouldReturnCorrectHtml()
    {
        var tokens = new List<Token>
        {
            Token.CreateStrong(true, 0),
            Token.CreateText("Посетите ", 2),
            Token.CreateLink("сайт", "https://example.com", 11),
            Token.CreateStrong(false, 31)
        };
    
        var result = _converter.Convert(tokens);
    
        result.Should().Be("<strong>Посетите <a href=\"https://example.com\">сайт</a></strong>");
    }
    
    
}