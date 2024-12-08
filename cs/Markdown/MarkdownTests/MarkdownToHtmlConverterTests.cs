using FluentAssertions;
using Markdown;

namespace MarkdownTests;

[TestFixture]
public class MarkdownToHtmlConverterTests
{
    private MarkdownToHtmlConverter converter;

    [SetUp]
    public void SetUp()
    {
        var converters = new List<HtmlTokenConverter>() { 
            new BoldTokenHtmlConverter(),
            new HeaderTokenHtmlConverter(),
            new ItalicTokenHtmlConverter(),
            new TextTokenHtmlConverter(),
            new LinkTokenHtmlConverter()
        };
        converter = new MarkdownToHtmlConverter(converters);
    }

    [Test]
    public void Converter_ReturnEmptyString_WithNoTokens()
    {
        var tokens = new List<Token>();
        
        var actualText = converter.Convert(tokens);

        actualText.Should().Be(string.Empty);
    }

    [Test]
    public void Converter_ThrowException_WhenTokensIsNull()
    {
        var action = () => converter.Convert(null);
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Converter_ConvertOneTokenToHtml()
    {
        var tokens = new List<Token>() { new SimpleToken(TokenType.Italic, "simple text")};

        var actualText = converter.Convert(tokens);
        var expectedText = $"<em>simple text</em>";

        actualText.Should().Be(expectedText);
    }

    [Test]
    public void Converter_ConvertSeveralSimpleTokens()
    {
        var tokens = new List<Token>()
        {
            new SimpleToken(TokenType.SimpleText, "simple text "),
            new SimpleToken(TokenType.Italic, "italic"),
            new SimpleToken(TokenType.Bold, " bold")
        };

        var actualText = converter.Convert(tokens);
        var expectedText = "simple text <em>italic</em><strong> bold</strong>";

        actualText.Should().Be(expectedText);
    }

    [Test]
    public void Converter_ConvertOneComplexToken()
    {
        var childrens = new List<Token>()
        {
            new SimpleToken(TokenType.Italic, "italic"),
            new SimpleToken(TokenType.SimpleText, " text "),
            new SimpleToken(TokenType.Bold, "bold")
        };

        var tokens = new List<Token>()
        {
            new ComplexToken(TokenType.Bold, childrens)
        };

        var actualText = converter.Convert(tokens);
        var expectedText = "<strong><em>italic</em> text <strong>bold</strong></strong>";

        actualText.Should().Be(expectedText);
    }

    [Test]
    public void Converter_ConvertSeveralComplexToken()
    {
        var childrens = new List<Token>()
        {
            new SimpleToken(TokenType.Italic, "italic"),
            new SimpleToken(TokenType.SimpleText, " text "),
            new SimpleToken(TokenType.Bold, "bold")
        };

        var tokens = new List<Token>()
        {
            new ComplexToken(TokenType.Bold, childrens),
            new ComplexToken(TokenType.Italic, childrens)
        };

        var actualText = converter.Convert(tokens);
        var expectedText = "<strong><em>italic</em> text <strong>bold</strong></strong>" +
            "<em><em>italic</em> text <strong>bold</strong></em>";

        actualText.Should().Be(expectedText);
    }

    [Test]
    public void Converter_ConvertSimpleAndComplexToken()
    {
        var childrens = new List<Token>()
        {
            new SimpleToken(TokenType.Italic, "italic"),
            new SimpleToken(TokenType.SimpleText, " text "),
            new SimpleToken(TokenType.Bold, "bold")
        };

        var tokens = new List<Token>()
        {
            new SimpleToken(TokenType.Bold, "a"),
            new ComplexToken(TokenType.Bold, childrens)
        };

        var actualText = converter.Convert(tokens);
        var expectedText = "<strong>a</strong><strong><em>italic</em> text <strong>bold</strong></strong>";

        actualText.Should().Be(expectedText);
    }

    [Test]
    public void Converter_ConvertTokenWithArgument()
    {
        var tokens = new List<Token>()
        {
            new SimpleToken(TokenType.Bold, "a"),
            new TokenWithArgument(TokenType.Link, "a", "b")
        };

        var actualText = converter.Convert(tokens);
        var expectedText = "<strong>a</strong><a href=\"b\">a</a>";

        actualText.Should().Be(expectedText);
    }
}
