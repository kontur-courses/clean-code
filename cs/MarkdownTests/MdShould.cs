using FluentAssertions;
using Markdown;
using Markdown.Markdown;
using Markdown.Renderers;

namespace MarkdownTests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class MdShould
{
    private readonly Md markdown = new(new TextTokenizer(), new HtmlRenderer());
    private const string TestsFilesDirectory = "Resources";

    [TestCase("test, _te x t_ tt", ExpectedResult = "test, <em>te x t</em> tt",
        TestName = "Render italic text with surrounding regular text")]
    [TestCase("_be_gin", ExpectedResult = "<em>be</em>gin", TestName = "Render italic text at word start")]
    [TestCase("mi_dd_le", ExpectedResult = "mi<em>dd</em>le", TestName = "Render italic text in word middle")]
    [TestCase("the e_nd._", ExpectedResult = "the e<em>nd.</em>", TestName = "Render italic text at word end")]
    public string RenderItalicText_ParseCorrectly(string markdownText) =>
        markdown.Render(markdownText);

    [TestCase("with numbers_12_3 text", ExpectedResult = "with numbers_12_3 text",
        TestName = "Should not parse when contains numbers")]
    [TestCase("_text___", ExpectedResult = "_text___", TestName = "Should not parse with unmatched underscores")]
    [TestCase("text_ text_", ExpectedResult = "text_ text_", TestName = "Should not parse with spaces after tags")]
    [TestCase("_text text _text", ExpectedResult = "_text text _text",
        TestName = "Should not parse with space before end tag")]
    [TestCase("text _text text", ExpectedResult = "text _text text", TestName = "Should not parse with only start tag")]
    [TestCase("text t_ext ___te_xt", ExpectedResult = "text t_ext ___te_xt",
        TestName = "Should not parse with multiple underscores")]
    public string RenderItalicText_NotParseInvalidCases(string markdownText) =>
        markdown.Render(markdownText);

    [TestCase("__text__", ExpectedResult = "<strong>text</strong>", TestName = "Render basic bold text")]
    public string RenderBoldText_ParseCorrectly(string markdownText) =>
        markdown.Render(markdownText);

    [TestCase(@"\_text\_", ExpectedResult = @"_text_", TestName = "Handle single escaped underscore")]
    [TestCase(@"\\\_text\\\_", ExpectedResult = @"\_text\_", TestName = "Handle triple escaped underscore")]
    public string RenderEscapedText_HandleEscapedUnderscores(string markdownText) =>
        markdown.Render(markdownText);

    [TestCase(@"\\_text\\_", ExpectedResult = @"\<em>text\</em>", TestName = "Handle double escaped underscore")]
    [TestCase(@"\\\\_text\\\\_", ExpectedResult = @"\\<em>text\\</em>", TestName = "Handle quad escaped underscore")]
    public string RenderEscapedText_PreserveEscapedCharacters(string markdownText) =>
        markdown.Render(markdownText);

    [TestCase("diffe_rent wo_rds", ExpectedResult = "diffe_rent wo_rds", TestName = "Render italic in different words")]
    [TestCase(@"\__t_t___t__", ExpectedResult = @"__t_t___t__", TestName = "Handle escaped bold start tag")]
    [TestCase("t _t __t__ t_ t", ExpectedResult = @"t <em>t __t__ t</em> t", TestName = "Handle bold inside italic")]
    [TestCase("t __t _t_ t__ t", ExpectedResult = @"t <strong>t <em>t</em> t</strong> t",
        TestName = "Handle nested italic in bold")]
    [TestCase("t _t t __t t_ t__ t", ExpectedResult = "t _t t __t t_ t__ t", TestName = "Handle overlapping tags")]
    [TestCase("__t _t t__ t t_ _t t__ t t_ t__ t", ExpectedResult = "__t _t t__ t t_ <em>t t__ t t</em> t__ t",
        TestName = "Handle complex tag intersections")]
    [TestCase(@"Here sim\bols of shielding\ \should stay.\",
        ExpectedResult = @"Here sim\bols of shielding\ \should stay.\", TestName = "Preserve escaped characters")]
    public string RenderMixedFormatting_HandleComplexCases(string markdownText) =>
        markdown.Render(markdownText);

    [TestCase("# text", ExpectedResult = "<h1>text</h1>", TestName = "Render basic header")]
    [TestCase("# _text_", ExpectedResult = "<h1><em>text</em></h1>", TestName = "Render header with italic text")]
    [TestCase("# t __t _t_ t__", ExpectedResult = "<h1>t <strong>t <em>t</em> t</strong></h1>",
        TestName = "Render header with mixed formatting")]
    public string RenderHeaders_ParseCorrectly(string markdownText) =>
        markdown.Render(markdownText);

    [TestCase("#_text_", ExpectedResult = "#<em>text</em>", TestName = "Should not parse header without space")]
    public string RenderHeaders_NotParseInvalidCases(string markdownText) =>
        markdown.Render(markdownText);

    [TestCase(@"![Alt text](URL)", ExpectedResult = """<img src="URL" alt="Alt text">""",
        TestName = "Handle picture with alt")]
    [TestCase(@"![Alt text URL)", ExpectedResult = "![Alt text URL)",
        TestName = "Not handle picture with alt")]
    public string RenderPictures_Parse(string markdownText) =>
        markdown.Render(markdownText);

    [TestCase("MarkdownSpec.md", TestName = "Convert MarkdownSpec file to HTML")]
    public void RenderMarkdownFile_CreateHtmlFile(string markdownPath)
    {
        var markdownText = File.ReadAllText(Path.Combine(TestsFilesDirectory, markdownPath));
        var htmlPath = Path.ChangeExtension(markdownPath, ".html");

        var actualText = markdown.Render(markdownText);
        var expectedText = File.ReadAllText(Path.Combine(TestsFilesDirectory, $"ExpectedResult_{htmlPath}"));

        actualText.Should().Be(expectedText);
    }
}