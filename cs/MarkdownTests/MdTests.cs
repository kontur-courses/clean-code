using FluentAssertions;
using Markdown.Markdown;

namespace MarkdownTests;

public class MdTests
{
    private Md md;

    [SetUp]
    public void Setup()
    {
        md = new Md();
    }

    [Test]
    public void Render_ShouldReturnSimpleText()
    {
        const string text = "Hello, world!";
        md.Render(text).Should().Be(text);
    }

    [Test]
    public void Render_ShouldReturnCursiveText()
    {
        md.Render("Hello, _world_!").Should().Be("Hello, <em>world</em>!");
    }
    [Test]
    public void Render_ShouldReturn_HtmlString()
    {
        const string text = "#заголовок __с жирным текстом__\n" +
                         "Просто текст, в котором _курсивные_ выделения\n" +
                         "__Есть жирный текст__\n" +
                         "__А вот жирный текст _с курсивом_ внутри _и ещё курсив_ в жирном__\n" +
                         "_Вот __это_ не__ сработает\n" +
                         "_И вот так __тоже__ нет_\n" +
                         "Это - _ - просто подчёркивание\n" +
                         "Так_ не работает_\n" +
                         "И _ вот так _ тоже\n" +
                         "В с_лов_е можно выделять, а в цифрах 1_23_ нет\n" +
                         "Ещу можно сделать просто _курсив_\n";
        const string expectedResult = "<h1>заголовок <strong>с жирным текстом</strong></h1>" +
                                      "Просто текст, в котором <em>курсивные</em> выделения<br />" +
                                      "<strong>Есть жирный текст</strong><br />" +
                                      "<strong>А вот жирный текст <em>с курсивом</em> внутри <em>и ещё курсив</em> в жирном</strong><br />" +
                                      "_Вот __это_ не__ сработает<br />" +
                                      "<em>И вот так __тоже__ нет</em><br />" +
                                      "Это - _ - просто подчёркивание<br />" +
                                      "Так_ не работает_<br />" +
                                      "И _ вот так _ тоже<br />" +
                                      "В с<em>лов</em>е можно выделять, а в цифрах 1_23_ нет<br />" +
                                      "Ещу можно сделать просто <em>курсив</em><br />";
        var result = new Md().Render(text);
        result.Should().Be(expectedResult);
    }

    [Test]
    public void Render_ShouldCorrectlyHandle_NewlineAfterHeader()
    {
        const string text = "#заголовок __с жирным текстом__\nПросто текст, в котором _курсивные_ выделения\n";
        md.Render(text).Should()
            .Be(
                "<h1>заголовок <strong>с жирным текстом</strong></h1>Просто текст, в котором <em>курсивные</em> выделения<br />");
    }

    [Test]
    public void Render_ShouldNotCreate_DoubleText()
    {
        const string text = "__А вот жирный текст _с курсивом_ внутри _и ещё курсив_ в жирном__\n";
        md.Render(text).Should().Be("<strong>А вот жирный текст <em>с курсивом</em> внутри <em>и ещё курсив</em> в жирном</strong><br />");
    }

    [TestCase("_Вот __это_ не__ сработает\n", "_Вот __это_ не__ сработает<br />")]
    [TestCase("_Вот __это _не__ сработает\n", "_Вот __это _не__ сработает<br />")]
    [TestCase("__Вот _это __не_ сработает\n", "__Вот _это __не_ сработает<br />")]
    [TestCase("__Вот _это __не_ сработает, это - _да_\n", "__Вот _это __не_ сработает, это - <em>да</em><br />")]
    [TestCase("__Вот_это__не_сработает\n", "__Вот_это__не_сработает<br />")]
    public void Render_ShouldCorrectlyHandle_Overlaps(string text, string expected)
    {
        md.Render(text).Should().Be(expected);
    }
}