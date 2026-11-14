using AwesomeAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class StrongTests
{
    private static readonly TestCaseData[] Cases =
    {
        RenderCase.Create(
            "Двойной подчерк создает полужирный текст",
            "__жирный__",
            "<p><strong>жирный</strong></p>"),

        RenderCase.Create(
            "полужирный текст внутри предложения",
            "текст __жирный__ дальше",
            "<p>текст <strong>жирный</strong> дальше</p>"),

        RenderCase.Create(
            "несколько полужирных фрагментов текста в одной строке",
            "__первый__ и __второй__",
            "<p><strong>первый</strong> и <strong>второй</strong></p>"),

        RenderCase.Create(
            "полужирный текст в середине слова",
            "сер__еди__не",
            "<p>сер<strong>еди</strong>не</p>"),

        RenderCase.Create(
            "полужирный текст в разных словах",
            "ра__зных сл__овах",
            "<p>ра__зных сл__овах</p>"),

        RenderCase.Create(
            "двойной подчерк без закрывающего маркера",
            "__непарный текст",
            "<p>__непарный текст</p>"),

        RenderCase.Create(
            "пробельный символ после открывающего двойного подчерка",
            "__ текст__",
            "<p>__ текст__</p>"),

        RenderCase.Create(
            "пробельный символ перед закрывающим двойным подчерком",
            "__текст __",
            "<p>__текст __</p>"),

        RenderCase.Create(
            "3 и более подчерков не являются маркерами",
            "___текст___",
            "<p>___текст___</p>"),
    };

    [TestCaseSource(nameof(Cases))]
    public void Render_ReturnsExpectedHtml(string input, string expected)
    {
        Md.Render(input).Should().Be(expected);
    }
}
