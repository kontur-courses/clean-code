using FluentAssertions;
using Markdown;
namespace MarkdownTests;

public class BorderlineCasesTests
{
    private Md md;

    [SetUp]
    public void SetUp(){
        md = new Md();
    }

    [Test]
    public void Test_TextWithDigits()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("__aaa12__3__a").Should().Be("__aaa12__3__a");
        md.Render("__12__3").Should().Be("__12__3");
        md.Render("_12_3").Should().Be("_12_3");
        md.Render("_aaa12_3_a_").Should().Be("<em>aaa12_3_a</em>");
        md.Render("__aaa12_3_a__").Should().Be("<strong>aaa12_3_a</strong>");
        md.Render("_aaa12__3__a_").Should().Be("<em>aaa12__3__a</em>");
        md.Render("__aaa12__3__a__").Should().Be("<strong>aaa12__3__a</strong>");
        md.Render("_123_").Should().Be("<em>123</em>");
        md.Render("__123__").Should().Be("<strong>123</strong>");
        md.Render("_123_  _a_").Should().Be("<em>123</em>  <em>a</em>");
        md.Render("__123__  __a__").Should().Be("<strong>123</strong>  <strong>a</strong>");
    }

    [Test]
    public void Test_TagsWithoutText()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("____").Should().Be("____");
        md.Render("___").Should().Be("___");
        md.Render("__").Should().Be("__");
        md.Render("_").Should().Be("_");
    }
    [Test]
    public void Test_TextWithUnpairedTags()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("__Непарные_").Should().Be("__Непарные_");
        md.Render("_Непарные__").Should().Be("_Непарные__");
        md.Render("__s_ __s").Should().Be("__s_ __s");
        md.Render("__s _s__").Should().Be("<strong>s _s</strong>");
        md.Render("__s_ s__").Should().Be("<strong>s_ s</strong>");
    }

    [Test]
    public void Test_IntersectionOfTags()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("_a __bbb__ a_").Should().Be("<em>a __bbb__ a</em>");
        md.Render("__a _bbb_ a__").Should().Be("<strong>a <em>bbb</em> a</strong>");
        md.Render("__s _s__ _d_").Should().Be("<strong>s _s</strong> <em>d</em>");
        md.Render("__s _s__ _d_").Should().Be("<strong>s _s</strong> <em>d</em>");
        md.Render("__a _bbb__ a_").Should().Be("__a _bbb__ a_");
        md.Render("a__ b_ _c __bb").Should().Be("a__ b_ _c __bb");
        md.Render("__пересечения _двойных__ и одинарных_").Should().Be("__пересечения _двойных__ и одинарных_");
        md.Render("_пересечения __двойных_ и одинарных__").Should().Be("_пересечения __двойных_ и одинарных__");
    }
}