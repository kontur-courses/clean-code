using AwesomeAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class ParagraphTests
{
    private static readonly TestCaseData[] Cases =
    {
        RenderCase.Create(
            "Обычный текст становится параграфом",
            "Обычный текст",
            "<p>Обычный текст</p>"),

        RenderCase.Create(
            "Пустой вход",
            "",
            ""),

        RenderCase.Create(
            "Прпобелы и переносы строк",
            "  \n \n",
            ""),

        RenderCase.Create(
            "Пустая строка создает новый абзац",
            "Первый абзац\n\nВторой абзац",
            "<p>Первый абзац</p>\n<p>Второй абзац</p>"),

        RenderCase.Create(
            "несколько пустых строк как единый разделитель",
            "Текст\n\n\nЕщё текст",
            "<p>Текст</p>\n<p>Ещё текст</p>"),

        RenderCase.Create(
            "пустая строка состоящая только из пробельных символов",
            "Текст\n   \nЕщё текст",
            "<p>Текст</p>\n<p>Ещё текст</p>"),

        RenderCase.Create(
            "один перенос строки остается в том же абзаце",
            "Первая строка\nвторая строка",
            "<p>Первая строка\nвторая строка</p>"),

        RenderCase.Create(
            "перенос строки в конце не создает новый абзац",
            "Текст\n",
            "<p>Текст</p>"),

        RenderCase.Create(
            "символы конца строки из Windows разделяют абзацы",
            "Первый абзац\r\n\r\nВторой абзац",
            "<p>Первый абзац</p>\n<p>Второй абзац</p>"),

        RenderCase.Create(
            "Разрыв строки из Windows внутри абзаца приводится к символу LF",
            "Первая строка\r\nвторая строка",
            "<p>Первая строка\nвторая строка</p>"),

        RenderCase.Create(
            "Одиночный символ \\r интерпретируется как разрыв строки",
            "Первая строка\rвторая строка",
            "<p>Первая строка\nвторая строка</p>"),

        RenderCase.Create(
            "Отступ в начале строки сохраняется",
            "  Текст с отступом",
            "<p>  Текст с отступом</p>"),

        RenderCase.Create(
            "# внутри текста не создает заголовок",
            "Привет # мир",
            "<p>Привет # мир</p>"),

        RenderCase.Create(
            "спецсимволы html рендерятся нормально",
            "A < B & C > D",
            "<p>A &lt; B &amp; C &gt; D</p>"),
    };

    [TestCaseSource(nameof(Cases))]
    public void Render_ReturnsExpectedHtml(string input, string expected)
    {
        Md.Render(input).Should().Be(expected);
    }
}
