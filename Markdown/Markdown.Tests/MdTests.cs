using AwesomeAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

public class MdTests
{
    [Test]
    public void Render_PlainText_ReturnsParagraph()
    {
        const string input = "Обычный текст";
        const string expected = "<p>Обычный текст</p>";

        var actual = Md.Render(input);

        actual.Should().Be(expected);
    }
    
    [Test]
    public void Render_Emphasis_ReturnsEmTag()
    {
        const string input = "_курсив_";

        var actual = Md.Render(input);

        actual.Should().Be("<p><em>курсив</em></p>");
    }

    [Test]
    public void Render_HtmlCharacters_EscapesSpecialCharacters()
    {
        const string input = "A < B & C > D";

        var actual = Md.Render(input);

        actual.Should().Be("<p>A &lt; B &amp; C &gt; D</p>");
    }
    
    [TestCase("_нач_але", "<p><em>нач</em>але</p>")]
    [TestCase("сер_еди_не", "<p>сер<em>еди</em>не</p>")]
    [TestCase("кон_це._", "<p>кон<em>це.</em></p>")]
    [TestCase("ра_зных сл_овах", "<p>ра_зных сл_овах</p>")]
    [TestCase("\\_текст\\_", "<p>_текст_</p>")]
    [TestCase("____", "<p>____</p>")]
    [TestCase("___текст___", "<p>___текст___</p>")]
    [TestCase("_ текст_", "<p>_ текст_</p>")]
    [TestCase("_текст _", "<p>_текст _</p>")]
    [TestCase("цифрами_12_3", "<p>цифрами_12_3</p>")]
    [TestCase("непарный _текст", "<p>непарный _текст</p>")]
    [TestCase("_несколько слов_", "<p><em>несколько слов</em></p>")]
    public void Render_Emphasis_HandlesDifferentCases(
        string input,
        string expected)
    {
        var actual = Md.Render(input);

        actual.Should().Be(expected);
    }
    
    [TestCase("__жирный__", "<p><strong>жирный</strong></p>")]
    [TestCase("текст __жирный__ дальше",
        "<p>текст <strong>жирный</strong> дальше</p>")]
    [TestCase("__жирный _курсив_ текст__",
        "<p><strong>жирный <em>курсив</em> текст</strong></p>")]
    [TestCase("_курсив __жирный__ текст_",
        "<p><em>курсив __жирный__ текст</em></p>")]
    [TestCase("__пересечение _двойных__ и одинарных_",
        "<p>__пересечение _двойных__ и одинарных_</p>")]
    [TestCase("__Непарные_ символы",
        "<p>__Непарные_ символы</p>")]
    [TestCase("__первый__ и __второй__",
        "<p><strong>первый</strong> и <strong>второй</strong></p>")]
    [TestCase("_первый_ и _второй_",
        "<p><em>первый</em> и <em>второй</em></p>")]
    [TestCase("__начало _курсив_ без конца",
        "<p>__начало <em>курсив</em> без конца</p>")]
    [TestCase("__один _два_ и _три_ конец__",
        "<p><strong>один <em>два</em> и <em>три</em> конец</strong></p>")]
    public void Render_InlineFormatting_HandlesDifferentCases(
        string input,
        string expected)
    {
        var actual = Md.Render(input);

        actual.Should().Be(expected);
    }
    
    [TestCase("# Заголовок", "<h1>Заголовок</h1>")]
    [TestCase(
        "# Заголовок __жирный__",
        "<h1>Заголовок <strong>жирный</strong></h1>")]
    [TestCase(
        "# Заголовок __с _разными_ символами__",
        "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
    [TestCase("## Заголовок", "<p>## Заголовок</p>")]
    [TestCase("#Без пробела", "<p>#Без пробела</p>")]
    [TestCase(" # Заголовок", "<p> # Заголовок</p>")]
    [TestCase("\\# Заголовок", "<p># Заголовок</p>")]
    public void Render_Heading_HandlesDifferentCases(
        string input,
        string expected)
    {
        var actual = Md.Render(input);

        actual.Should().Be(expected);
    }
    
    [TestCase(
        "Первый абзац\n\nВторой абзац",
        "<p>Первый абзац</p>\n<p>Второй абзац</p>")]
    [TestCase(
        "Первая строка\nвторая строка",
        "<p>Первая строка\nвторая строка</p>")]
    [TestCase(
        "Первый\n\n# Заголовок\n\nПоследний",
        "<p>Первый</p>\n<h1>Заголовок</h1>\n<p>Последний</p>")]
    [TestCase(
        "Текст\n\n\nЕщё текст",
        "<p>Текст</p>\n<p>Ещё текст</p>")]
    [TestCase(
        "Текст\n   \nЕщё текст",
        "<p>Текст</p>\n<p>Ещё текст</p>")]
    public void Render_Blocks_HandlesDifferentCases(
        string input,
        string expected)
    {
        var actual = Md.Render(input);

        actual.Should().Be(expected);
    }
    
    [Test]
    public void Render_InlineFormatting_DoesNotCrossParagraphs()
    {
        const string input = "_начало\n\nконец_";
        const string expected =
            "<p>_начало</p>\n<p>конец_</p>";

        var actual = Md.Render(input);

        actual.Should().Be(expected);
    }

    [Test]
    public void Render_Blocks_HandlesWindowsLineEndings()
    {
        const string input = "Первый абзац\r\n\r\nВторой абзац";
        const string expected =
            "<p>Первый абзац</p>\n<p>Второй абзац</p>";

        var actual = Md.Render(input);

        actual.Should().Be(expected);
    }
    
    [TestCase(
        "- Первый пункт\n- Второй пункт",
        "<ul><li>Первый пункт</li><li>Второй пункт</li></ul>")]

    [TestCase(
        "- _Курсив_\n- __Жирный__",
        "<ul><li><em>Курсив</em></li><li><strong>Жирный</strong></li></ul>")]

    [TestCase(
        "До списка\n\n- Первый\n- Второй\n\nПосле списка",
        "<p>До списка</p>\n<ul><li>Первый</li><li>Второй</li></ul>\n<p>После списка</p>")]

    [TestCase(
        "\\- Это не список",
        "<p>- Это не список</p>")]

    [TestCase(
        "-Без пробела",
        "<p>-Без пробела</p>")]

    [TestCase(
        "-- Не список",
        "<p>-- Не список</p>")]

    [TestCase(
        " - С отступом",
        "<p> - С отступом</p>")]

    [TestCase(
        "- Пункт с & < >",
        "<ul><li>Пункт с &amp; &lt; &gt;</li></ul>")]
    public void Render_UnorderedList_HandlesDifferentCases(
        string input,
        string expected)
    {
        var actual = Md.Render(input);

        actual.Should().Be(expected);
    }
    
    [TestCase(
        "- Первый\n\n- Второй",
        "<ul><li>Первый</li></ul>\n<ul><li>Второй</li></ul>")]

    [TestCase(
        "- __Незакрытый\n- маркер__",
        "<ul><li>__Незакрытый</li><li>маркер__</li></ul>")]

    [TestCase(
        "- ",
        "<ul><li></li></ul>")]

    [TestCase(
        "Обычный текст с - дефисом",
        "<p>Обычный текст с - дефисом</p>")]
    public void Render_UnorderedList_HandlesBoundaries(
        string input,
        string expected)
    {
        var actual = Md.Render(input);

        actual.Should().Be(expected);
    }
}