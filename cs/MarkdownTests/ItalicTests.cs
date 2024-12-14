using Markdown;
using FluentAssertions;
using System.Reflection.Metadata;
using System.ComponentModel.DataAnnotations;
namespace MarkdownTests;

public class ItailcTests
{
    private Md md;

    [SetUp]
    public void SetUp(){
        md = new Md();
    }
    [Test]
    public void Test_StandartItalicText()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("_abc_4").Should().Be("_abc_4");
        md.Render("_abc_4_").Should().Be("<em>abc_4</em>");
        md.Render("_abc w_ a _b s_").Should().Be("<em>abc w</em> a <em>b s</em>");
        md.Render("_aaaa_").Should().Be("<em>aaaa</em>");
        md.Render("_aaaa_bbbb_cc_").Should().Be("<em>aaaa</em>bbbb<em>cc</em>");
        md.Render("_aaaa_  bbb").Should().Be("<em>aaaa</em>  bbb");
        md.Render("_aaaa_    _bbbb_ _cc_").Should().Be("<em>aaaa</em>    <em>bbbb</em> <em>cc</em>");
        md.Render("_aaa   bbb_").Should().Be("<em>aaa   bbb</em>");
        md.Render("ab_c_de").Should().Be("ab<em>c</em>de");
        md.Render("ab_c d_a").Should().Be("ab_c d_a");
        md.Render("\\ _a_").Should().Be("\\ <em>a</em>");
        md.Render("_bbb" + "\n" + "bb_").Should().Be("<em>bbb" + "\n" + "bb</em>");
    }

    [Test]
    public void Test_NestedInItalicTags()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("_some __text__ in_").Should().Be("<em>some __text__ in</em>");
        md.Render("#  _aaa_").Should().Be("<h1><em>aaa</em></h1>");
        md.Render("_aaa__b__a_").Should().Be("<em>aaa__b__a</em>");
        md.Render("_aaa __b__ a_").Should().Be("<em>aaa __b__ a</em>");
    }
    [Test]
    public void Test_ItalicPartOfWord()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("_нач_але").Should().Be("<em>нач</em>але");
        md.Render("сер_еди_не").Should().Be("сер<em>еди</em>не");
        md.Render("кон_це._").Should().Be("кон<em>це.</em>");
    }

    [Test]
    public void Test_ItalicSeveralWords()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("ра_зных сл_овах").Should().Be("ра_зных сл_овах");
        md.Render("ра_зных словах_").Should().Be("ра_зных словах_");
    }

    [Test]
    public void Test_ItalicTextWithSpaces()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("_        _").Should().Be("");
        md.Render("_ подчерки_").Should().Be("_ подчерки_");
        md.Render("_подчерки _").Should().Be("_подчерки _");
    }
}