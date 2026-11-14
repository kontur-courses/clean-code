using AwesomeAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class EscapingTests
{
    private static readonly TestCaseData[] Cases =
    {
        RenderCase.Create(
            "Экранированные подчерки",
            "\\_текст\\_",
            "<p>_текст_</p>"),

        RenderCase.Create(
            "Экранированные подчерки внутри слова",
            "a\\_b_c_d",
            "<p>a_b<em>c</em>d</p>"),

        RenderCase.Create(
            "Экранированные символы обратного слеша",
            "\\\\",
            "<p>\\</p>"),

        RenderCase.Create(
            "Экранированный символ обратного слеша перед подчерком",
            "\\\\_текст_",
            "<p>\\<em>текст</em></p>"),

        RenderCase.Create(
            "Обратный символ в конце строки остается",
            "текст\\",
            "<p>текст\\</p>"),

        RenderCase.Create(
            "Экранированный символ заголовка не создает заголовок",
            "\\# Заголовок",
            "<p># Заголовок</p>"),

        RenderCase.Create(
            "Экранированный символ заголовка не создает заголовок",
            "\\- Это не список",
            "<p>- Это не список</p>"),

        RenderCase.Create(
            "Экранированный тэг HTML",
            "\\<b>",
            "<p>&lt;b&gt;</p>"),
    };

    [TestCaseSource(nameof(Cases))]
    public void Render_ReturnsExpectedHtml(string input, string expected)
    {
        Md.Render(input).Should().Be(expected);
    }
}
