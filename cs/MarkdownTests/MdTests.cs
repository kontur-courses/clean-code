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
    
    [TestCase("_text_", "<em>text</em>")]
    [TestCase("_te_xt", "<em>te</em>xt")]
    [TestCase("te_xt_", "te<em>xt</em>")]
    [TestCase("te_x_t", "te<em>x</em>t")]
    [TestCase("_1a_a1_", "_1a_a1_")]
    [TestCase("_a", "_a")]
    [TestCase("_1a_", "_1a_")]
    [TestCase("___11___", "___11___")]
    [TestCase("___11_ text__", "<strong>_11_ text</strong>")]
    [TestCase("_text__text_", "_text__text_")]
    [TestCase("_text__text_text_", "_text__text<em>text</em>")]
    [TestCase("__text__", "<strong>text</strong>")]
    [TestCase("__t_e_xt__", "<strong>t<em>e</em>xt</strong>")]
    [TestCase("_te_xt_", "<em>te</em>xt_")]
    [TestCase("text_", "text_")]
    [TestCase("text", "text")]
    [TestCase("_te_ xt_", "<em>te</em> xt_")]
    public void ItalicTest(string text, string expected)
    {
        var r = Builder.Build(sut.Checker_Token_(sut.Parse(text)));
        r.Should().Be(expected);
    }
    
    [TestCase(@"_te\\_ xt_", "<em>te\\</em> xt_")]
    [TestCase(@"\# _text", "# _text")]
    [TestCase(@"_te\xt_", @"<em>te\xt</em>")]
    [TestCase(@"\_te\xt_", @"_te\xt_")]
    [TestCase(@"_\te\xt_", @"<em>\te\xt</em>")]
    public void EscapeTagTests(string text, string expected)
    {
        var r = Builder.Build(sut.Checker_Token_(sut.Parse(text)));
        r.Should().Be(expected);
    }
    
    [TestCase("# _text_\n", "<h1><em>text</em></h1>")]
    [TestCase("# _text_\r\n", "<h1><em>text</em></h1>")]
    public void HeaderTagTests(string text, string expected)
    {
        var r = Builder.Build(sut.Checker_Token_(sut.Parse(text)));
        r.Should().Be(expected);
    }
    
    [TestCase("_t__ex__t_", "<em>t__ex__t</em>")]
    [TestCase("te_xt te_xt", "te_xt te_xt")]
    [TestCase("__t_ex_t__", "<strong>t<em>ex</em>t</strong>")]
    [TestCase("_ text _", "_ text _")]
    [TestCase("_text _", "_text _")]
    [TestCase("_ text_", "_ text_")]
    
    [TestCase("_text _ text_text_", "<em>text _ text_text</em>")]
    [TestCase("_text te_xt", "_text te_xt")]
    [TestCase("te_xt te_xt", "te_xt te_xt")]
    [TestCase("_te__x_t__", "_te__x_t__")]
    [TestCase("_te__x_t", "_te__x_t")]
    [TestCase("____", "____")]
    [TestCase("_text text_ _text te_xt", "<em>text text</em> _text te_xt")]
    public void TagMixedTests(string text, string expected)
    {
        var r = Builder.Build(sut.Checker_Token_(sut.Parse(text)));
        r.Should().Be(expected);
    }
}