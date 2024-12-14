using Markdown;
using FluentAssertions;
namespace MarkdownTests;

public class ScreenTests
{
    private Md md;

    [SetUp]
    public void SetUp(){
        md = new Md();
    }
    [Test]
    public void Test_ScreenTests()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("\\# aaa").Should().Be("# aaa", "Экранированный символ в начале. Экранирует заголовочный тэг");
        md.Render("\\#aa\\").Should().Be("\\#aa\\", "Экранированный символ в конце и начале, но тэг неправильно написан. Нет экранизации");
        md.Render("\\\\\\aaa").Should().Be("\\\\aaa", "Три экранированных символов и строка без тэгов. Экранируется 2 экранируемых символа");
        md.Render("\\__aaa__").Should().Be("__aaa__", "Экранированный символ в начале. Экранирует жирный тэг. У второго жирного тэга нет начала, поэтому он так остается");
        md.Render("\\__aaa\\_").Should().Be("__aaa_", "Первый экранирует жирный. Второй курсив");
        md.Render("\\\\aaa").Should().Be("\\aaa", "Два экранированных символов и строка без тэгов. Экранируется экранируемый символ");
        md.Render("\\aaa").Should().Be("\\aaa", "Один экранированный символ и строка без тэгов. Ему нечего экранировать");
        md.Render("\\__aaa\\__").Should().Be("__aaa__", "Два экранированных символа. Экранируют жирные тэги");
        md.Render("\\_aaa_").Should().Be("_aaa_", "Экранированный символ в начале. Экранирует курсивный тэг.  У второго курсивного тэга нет начала, поэтому он так остается");
        md.Render("\\_aaa_\\").Should().Be("_aaa_\\", "Первый экранирует курсив. Второму нечего");
        md.Render("\\__aaa\\").Should().Be("__aaa\\", "Первый экранирует жирный. Второму нечего");
        md.Render("\\\\_aa_").Should().Be("\\<em>aa</em>", "Экранированный символ в начале, экранирует экранируемый.");
    }
}