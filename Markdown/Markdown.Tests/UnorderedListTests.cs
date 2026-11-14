using AwesomeAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class UnorderedListTests
{
    private static readonly TestCaseData[] Cases =
    {
        RenderCase.Create(
            "создание списка",
            "- Первый пункт\n- Второй пункт",
            "<ul><li>Первый пункт</li><li>Второй пункт</li></ul>"),

        RenderCase.Create(
            "единственный элемент в списке",
            "- Единственный пункт",
            "<ul><li>Единственный пункт</li></ul>"),

        RenderCase.Create(
            "\\r не мешает созданию списка",
            "- Первый\r\n- Второй",
            "<ul><li>Первый</li><li>Второй</li></ul>"),

        RenderCase.Create(
            "элемент списка может содержать курсив и полужирный текст",
            "- _Курсив_\n- __Жирный__",
            "<ul><li><em>Курсив</em></li><li><strong>Жирный</strong></li></ul>"),

        RenderCase.Create(
            "специальные символы html е мешают созданию списка",
            "- Пункт с & < >",
            "<ul><li>Пункт с &amp; &lt; &gt;</li></ul>"),

        RenderCase.Create(
            "только первый пробел после - является частью маркера элемента списка",
            "-  Пункт",
            "<ul><li> Пункт</li></ul>"),

        RenderCase.Create(
            "элемент списка может быть пуст",
            "- ",
            "<ul><li></li></ul>"),

        RenderCase.Create(
            "# внутри элемента списка не создает заголовок",
            "- # Не заголовок",
            "<ul><li># Не заголовок</li></ul>"),

        RenderCase.Create(
            "Список не является частью абзаца",
            "До списка\n\n- Первый\n- Второй\n\nПосле списка",
            "<p>До списка</p>\n<ul><li>Первый</li><li>Второй</li></ul>\n<p>После списка</p>"),

        RenderCase.Create(
            "Пустая строка между элементами списка является разделителем между 2х списков",
            "- Первый\n\n- Второй",
            "<ul><li>Первый</li></ul>\n<ul><li>Второй</li></ul>"),

        RenderCase.Create(
            "курсив не переносится между элементами списка",
            "- __Незакрытый\n- маркер__",
            "<ul><li>__Незакрытый</li><li>маркер__</li></ul>"),

        RenderCase.Create(
            "- без пробела не создает список",
            "-Без пробела",
            "<p>-Без пробела</p>"),

        RenderCase.Create(
            "двойной символ - не создает список",
            "-- Не список",
            "<p>-- Не список</p>"),

        RenderCase.Create(
            "Пробел перед - мешает созданию списка",
            " - С отступом",
            "<p> - С отступом</p>"),

        RenderCase.Create(
            "- в середине строки не создает список",
            "Обычный текст с - дефисом",
            "<p>Обычный текст с - дефисом</p>"),

        RenderCase.Create(
            "между абзацем и списком должна быть пустая строка",
            "Текст\n- пункт",
            "<p>Текст\n- пункт</p>"),

        RenderCase.Create(
            "перенос строки внутри элемента списка",
            "- пункт\nтекст",
            "<p>- пункт\nтекст</p>"),

        RenderCase.Create(
            "Строка без маркера превращает весь блок в абзац",
            "- пункт\n-не пункт",
            "<p>- пункт\n-не пункт</p>"),
    };

    [TestCaseSource(nameof(Cases))]
    public void Render_ReturnsExpectedHtml(string input, string expected)
    {
        Md.Render(input).Should().Be(expected);
    }
}
