using AwesomeAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class HeadingTests
{
    private static readonly TestCaseData[] Cases =
    {
        RenderCase.Create(
            "Заголовок создается символом #",
            "# Заголовок",
            "<h1>Заголовок</h1>"),

        RenderCase.Create(
            "Заголовок может содержать полужирный текст",
            "# Заголовок __жирный__",
            "<h1>Заголовок <strong>жирный</strong></h1>"),

        RenderCase.Create(
            "Заголовок может содержать курсив внутри полужирного текста",
            "# Заголовок __с _разными_ символами__",
            "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>"),

        RenderCase.Create(
            "Только первый пробельный символ после # принадлежит к маркеру заголовка",
            "#  Заголовок",
            "<h1> Заголовок</h1>"),

        RenderCase.Create(
            "Пустой заголовок",
            "# ",
            "<h1></h1>"),

        RenderCase.Create(
            "Многострочный заголовок",
            "# Первая строка\nвторая строка",
            "<h1>Первая строка\nвторая строка</h1>"),

        RenderCase.Create(
            "Символ # внутри заголовка - обычный текст",
            "# Номер # 5",
            "<h1>Номер # 5</h1>"),

        RenderCase.Create(
            "маркер списка внутри заголовка не создает список",
            "# - не список",
            "<h1>- не список</h1>"),

        RenderCase.Create(
            "Заголовок внутри текста требует пустые строки вокруг него",
            "Первый\n\n# Заголовок\n\nПоследний",
            "<p>Первый</p>\n<h1>Заголовок</h1>\n<p>Последний</p>"),

        RenderCase.Create(
            "# внутри текста не создает заголовок",
            "Текст\n# Не заголовок",
            "<p>Текст\n# Не заголовок</p>"),

        RenderCase.Create(
            "## не заголовок (по заданной спецификации)",
            "## Заголовок",
            "<p>## Заголовок</p>"),

        RenderCase.Create(
            "# без пробельного символа не создает заголовок",
            "#Без пробела",
            "<p>#Без пробела</p>"),

        RenderCase.Create(
            "Один символ # им и остается",
            "#",
            "<p>#</p>"),

        RenderCase.Create(
            "Заголовок не должен создаваться если # не является начальным символом строки",
            " # Заголовок",
            "<p> # Заголовок</p>"),
    };

    [TestCaseSource(nameof(Cases))]
    public void Render_ReturnsExpectedHtml(string input, string expected)
    {
        Md.Render(input).Should().Be(expected);
    }
}
