using AwesomeAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class MdApiTests
{
    private const string Document =
        "# Заголовок\n\nТекст с _курсивом_ и __жирным__\n\n- пункт";

    private const string DocumentHtml =
        "<h1>Заголовок</h1>\n" +
        "<p>Текст с <em>курсивом</em> и <strong>жирным</strong></p>\n" +
        "<ul><li>пункт</li></ul>";

    [Test]
    [Description("Рендер должен выдать ошибку при пустом значении аргемента")]
    public void Render_NullText_Throws()
    {
        Action act = () => Md.Render(null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("text");
    }

    [Test]
    [Description("Парсер должен выдать ошибку при пустом значении аргемента")]
    public void Parse_NullText_Throws()
    {
        Action act = () => Md.Parse(null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("text");
    }

    [Test]
    [Description("Parse(text).ToHtml() должен выдать HTML рендер всего документа")]
    public void ParseToHtml_ReturnsExpectedHtml()
    {
        var actual = Md.Parse(Document).ToHtml();

        actual.Should().Be(DocumentHtml);
    }

    [Test]
    [Description("Render должен работать идентично Parse(text).ToHtml().")]
    public void Render_ReturnsSameHtmlAsParseToHtml()
    {
        var actual = Md.Render(Document);

        actual.Should().Be(Md.Parse(Document).ToHtml());
    }

    [Test]
    [Description("Парсинг одного и того же докуменда должен приводить к одному и тому же результату")]
    public void ToHtml_CalledTwice_ReturnsSameHtml()
    {
        var document = Md.Parse(Document);

        var first = document.ToHtml();
        var second = document.ToHtml();

        second.Should().Be(first);
    }
}
