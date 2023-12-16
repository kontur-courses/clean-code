using FluentAssertions;
using Markdown;

namespace MarkdownTests;

public class MdTests
{
    private Md sut;

    [SetUp]
    public void Setup()
    {
        sut = new Md();
    }

    [TestCase("_text_", "<em>text</em>", TestName = "Italic")]
    [TestCase("_te_xt", "<em>te</em>xt", TestName = "Italic tag end in text")]
    [TestCase("te_xt_", "te<em>xt</em>", TestName = "Italic tag start in text")]
    [TestCase("te_x_t", "te<em>x</em>t", TestName = "Italic tag in text")]
    [TestCase("__a_b_c__", "<strong>a<em>b</em>c</strong>", TestName = "Italic in Bold")]
    [TestCase("_1a_a1_", "_1a_a1_", TestName = "digit word not italic")]
    [TestCase("_1a_", "_1a_", TestName = "Mixes text is not italic")]
    public void Render_HandleItalicText_ShouldBeExpected(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [TestCase(@"_te\\_ xt_", "<em>te\\</em> xt_", TestName = "double escaped character")]
    [TestCase(@"\# _text", "# _text", TestName = " escaped character with header tag")]
    [TestCase(@"_te\xt_", @"<em>te\xt</em>", TestName = " escaped character in text")]
    [TestCase(@"\_te\xt_", @"_te\xt_", TestName = " escaped character ith opening tag")]
    [TestCase(@"_\te\xt_", @"<em>\te\xt</em>", TestName = " escaped character after tag")]
    public void EscapeTagTests(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }
    
    [TestCase("__text__", "<strong>text</strong>", TestName = "Bold")]
    [TestCase("__tex__t", "<strong>tex</strong>t", TestName = "Bold tag end in text")]
    [TestCase("te__xt__", "te<strong>xt</strong>", TestName = "Bold tag start in text")]
    [TestCase("te__x__t", "te<strong>x</strong>t", TestName = "Bold tag in text")]
    [TestCase("_a__b__c_", "<em>a__b__c</em>", TestName = "Bold In Italic")]
    [TestCase("__a __ b__", @"<strong>a __ b</strong>", TestName = "Bold with lonely tag in middle")]   
    [TestCase("__11__", "__11__", TestName = "digit no Bold")]
    public void Render_HandleBoldText_ShouldBeExpected(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }
    
    [TestCase("a_a b_b", @"a_a b_b", TestName = "Different words")]
    [TestCase("____", "____", TestName = "text Without any words")]
    [TestCase("__ _a_ __", @"__ <em>a</em> __", TestName = "Triple symbol")]
    [TestCase("a_ b_", @"a_ b_", TestName = "Invalid open symbol")]
    [TestCase("a_ b_", @"a_ b_", TestName = "Invalid open symbol")]
    [TestCase("_text__text_", "_text__text_", TestName = "tag intersection")]
    [TestCase("_a", "_a", TestName = "Lonely open symbol")]
    [TestCase("__a _b_ _c_ d__", @"<strong>a <em>b</em> <em>c</em> d</strong>", TestName = "Multiply italic in bold")]
    [TestCase("a_", "a_", TestName = "Lonely close symbol")]
    [TestCase("text", "text", TestName = "text without tags")]
    [TestCase("text _", "text _", TestName = "space before closing tag")]
    [TestCase("_ text", "_ text", TestName = "space after opening tag")]
    public void Render_HandleTextWithPairedTags_ShouldBeExpected(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }
//    [TestCase(@"# a", @"<h1>a</h1>", TestName = "Title")]
//    [TestCase(@"\# a", @"#a", TestName = "Screen title")]
    //   [TestCase(@"\\# a", @"<h1>a</h1>", TestName = "Double screen title")]
    [TestCase("# _text_\n", "<h1><em>text</em></h1>")]
    [TestCase("# _text_\r\n", "<h1><em>text</em></h1>")]
    public void HeaderTagTests(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }
}
