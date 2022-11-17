using MarkdownRenderer;

// ReSharper disable StringLiteralTypo

namespace MarkdownRenderer_Tests;

public class MdRender_Should
{
    private Md markdown = null!;

    [SetUp]
    public void Setup()
    {
        markdown = new Md();
    }

    [TestCase("abcd", ExpectedResult = "abcd", TestName = "Simple text")]
    [TestCase("_abcd_", ExpectedResult = "<em>abcd</em>", TestName = "Italic one word")]
    [TestCase("_ab cd_", ExpectedResult = "<em>ab cd</em>", TestName = "Italic some words")]
    [TestCase("a_bc_d", ExpectedResult = "a<em>bc</em>d", TestName = "Italic inside word")]
    [TestCase("a_1b_c", ExpectedResult = "a_1b_c", TestName = "Italic inside word with digits")]
    [TestCase("_ab cd_ef", ExpectedResult = "_ab cd_ef", TestName = "Italic inside word with spaces")]
    [TestCase("ab _c_", ExpectedResult = "ab <em>c</em>", TestName = "Italic two words")]
    [TestCase(" _c_", ExpectedResult = " <em>c</em>", TestName = "Italic with space before")]
    [TestCase("__strong__", ExpectedResult = "<strong>strong</strong>", TestName = "Strong one word")]
    [TestCase("__ab _cd_ ef__", ExpectedResult = "<strong>ab <em>cd</em> ef</strong>",
        TestName = "Italic inside strong")]
    [TestCase("_ab __cd__ ef_", ExpectedResult = "<em>ab __cd__ ef</em>", TestName = "Strong inside italic")]
    [TestCase("__abc_", ExpectedResult = "__abc_", TestName = "Not paired tags")]
    [TestCase("__", ExpectedResult = "__", TestName = "Empty string inside italic")]
    [TestCase("____", ExpectedResult = "____", TestName = "Empty string inside strong")]
    [TestCase("_a _b c_ d_", ExpectedResult = "<em>a _b c</em> d_", TestName = "Italic intersections")]
    [TestCase("__ab _cd__ ef_", ExpectedResult = "__ab _cd__ ef_", TestName = "Italic strong intersections")]
    [TestCase("# header", ExpectedResult = "<h1>header</h1>", TestName = "Header element")]
    [TestCase("# Заголовок __с _разными_ символами__",
        ExpectedResult = "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>",
        TestName = "Header with nested elements")]
    public string ReturnCorrectRenderResult(string sourceMd) =>
        markdown.Render(sourceMd);
}