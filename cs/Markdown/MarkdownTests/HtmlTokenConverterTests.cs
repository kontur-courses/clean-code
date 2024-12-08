using FluentAssertions;
using Markdown;

namespace MarkdownTests;

[TestFixture]
public class HtmlTokenConverterTests
{
    [Test]
    public void Converter_ThrowException_WithUncorrectTokenType()
    {
        var token = new SimpleToken(TokenType.Italic, "a");
        var converter = new BoldTokenHtmlConverter();

        var action = () => converter.Convert(token);

        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void BoldConverter_ConvertToHtml()
    {
        var content = "a";
        var token = new SimpleToken(TokenType.Bold, content);
        var converter = new BoldTokenHtmlConverter();

        var actualText = converter.Convert(token);
        var expectedText = $"<strong>{content}</strong>";

        actualText.Should().Be(expectedText);
    }

    [Test]
    public void ItalicConverter_ConvertToHtml()
    {
        var content = "a";
        var token = new SimpleToken(TokenType.Italic, content);
        var converter = new ItalicTokenHtmlConverter();

        var actualText = converter.Convert(token);
        var expectedText = $"<em>{content}</em>";

        actualText.Should().Be(expectedText);
    }

    [Test]
    public void HeaderConverter_ConvertToHtml()
    {
        var content = "a";
        var token = new SimpleToken(TokenType.Header, content);
        var converter = new HeaderTokenHtmlConverter();

        var actualText = converter.Convert(token);
        var expectedText = $"<h1>{content}</h1>";

        actualText.Should().Be(expectedText);
    }

    [Test]
    public void SimpleTextConverter_ConvertToHtml()
    {
        var content = "a";
        var token = new SimpleToken(TokenType.SimpleText, content);
        var converter = new TextTokenHtmlConverter();

        var actualText = converter.Convert(token);

        actualText.Should().Be(content);
    }

    [Test]
    public void LinkConverter_ConvertToHtml()
    {
        var content = "ссылка";
        var link = "https://google.com";
        var token = new TokenWithArgument(TokenType.Link, content, link);
        var converter = new LinkTokenHtmlConverter();

        var actualText = converter.Convert(token);
        var expectedText = $"<a href=\"{content}\">{link}</a>";

        actualText.Should().Be(expectedText);
    }

}
