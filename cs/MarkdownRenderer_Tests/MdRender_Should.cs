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
    [TestCase("_ab_cd", ExpectedResult = "<em>ab</em>cd", TestName = "Italic inside word")]
    [TestCase("ab _cd_", ExpectedResult = "ab <em>cd</em>", TestName = "Italic two words")]
    [TestCase("ab _cd_ef _gh_ _ij_", ExpectedResult = "ab <em>cd</em>ef <em>gh</em> <em>ij</em>", TestName = "Many words")]
    public string ReturnCorrectRenderResult(string sourceMd) => 
        markdown.Render(sourceMd);
}