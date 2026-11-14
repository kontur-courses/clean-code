using AwesomeAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class InlineNestingTests
{
    private static readonly TestCaseData[] Cases =
    {
        RenderCase.Create(
            "курсив внутри полужирного текста",
            "__жирный _курсив_ текст__",
            "<p><strong>жирный <em>курсив</em> текст</strong></p>"),

        RenderCase.Create(
            "несколько курсивов внутри полужирного текста",
            "__один _два_ и _три_ конец__",
            "<p><strong>один <em>два</em> и <em>три</em> конец</strong></p>"),

        RenderCase.Create(
            "полужирный текст не должен преобразовываться если он находится внутри курсива",
            "_курсив __жирный__ текст_",
            "<p><em>курсив __жирный__ текст</em></p>"),

        RenderCase.Create(
            "Пересечение подчерков курсива и полужирного текста ничего не дает",
            "__пересечение _двойных__ и одинарных_",
            "<p>__пересечение _двойных__ и одинарных_</p>"),

        RenderCase.Create(
            "Непарные подчерки",
            "__Непарные_ символы",
            "<p>__Непарные_ символы</p>"),

        RenderCase.Create(
            "незакртый полужирный текст не ломает курсив внутри него",
            "__начало _курсив_ без конца",
            "<p>__начало <em>курсив</em> без конца</p>"),

        RenderCase.Create(
            "курсив не должен распростроняться на несколько абзацев",
            "_начало\n\nконец_",
            "<p>_начало</p>\n<p>конец_</p>"),

        RenderCase.Create(
            "полужирный текст не должен распространяться на несколько абзацев",
            "__начало\n\nконец__",
            "<p>__начало</p>\n<p>конец__</p>"),
    };

    [TestCaseSource(nameof(Cases))]
    public void Render_ReturnsExpectedHtml(string input, string expected)
    {
        Md.Render(input).Should().Be(expected);
    }
}
