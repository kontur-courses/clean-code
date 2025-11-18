using FluentAssertions;
using Markdown;
using Markdown.Tags;

namespace MarkdownTests.TagTests;


[TestFixture]
public class HeaderTagTests
{
    private Md mdRenderer;

    [OneTimeSetUp]
    public void Setup()
    {
        var supportedTags = new List<ITag> { new HeaderTag() };
        mdRenderer = new Md(supportedTags);
    }

    [Test]
    public void WhenHeaderAtStartOfLine_ConvertsCorrectly()
    {
        var text = "# Заголовок";
        var expected = "<h1>Заголовок</h1>";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenHeaderInMiddleOfText_NotConverted()
    {
        var text = "Текст # заголовок не должен преобразовываться";
        var expected = "Текст # заголовок не должен преобразовываться";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenMultipleHeaders_ConvertsCorrectly()
    {
        var text = "# Первый заголовок\n# Второй заголовок";
        var expected = "<h1>Первый заголовок</h1>\n<h1>Второй заголовок</h1>";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenHeaderAfterNewline_ConvertsCorrectly()
    {
        var text = "Предыдущий абзац\n# Заголовок после перевода строки";
        var expected = "Предыдущий абзац\n<h1>Заголовок после перевода строки</h1>";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenHeaderWithOnlyHash_NotConverted()
    {
        var text = "#";
        var expected = "#";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenHeaderWithOnlyHashAndSpace_ConvertsToEmptyH1()
    {
        var text = "# ";
        var expected = "<h1></h1>";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenHeaderWithMultipleHashes_NotConverted()
    {
        var text = "## Двойной хэш не должен работать";
        var expected = "## Двойной хэш не должен работать";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenHeaderWithoutSpaceAfterHash_NotConverted()
    {
        var text = "#Заголовок без пробела";
        var expected = "#Заголовок без пробела";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }
    
    
    [Test]
    public void WhenHeaderWithMultipleSpacesAfterHash_ConvertsCorrectly()
    {
        var text = "#   Заголовок с несколькими пробелами";
        var expected = "<h1>  Заголовок с несколькими пробелами</h1>";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }
}