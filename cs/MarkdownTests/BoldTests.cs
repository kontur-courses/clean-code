using FluentAssertions;
using Markdown;

namespace MarkdownTests;

public class BoldTests
{
    private Md md;

    [SetUp]
    public void SetUp()
    {
        md = new Md();
    }
    [Test]
    public void Test_StandartBoldWord()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("__abc__4__").Should().Be("<strong>abc__4</strong>");
        md.Render("__abc__4").Should().Be("__abc__4");
        md.Render("__aaaa__").Should().Be("<strong>aaaa</strong>");
    }

    [Test]
    public void Test_StandartBoldWords()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("__aaaa__bbbb__cc__").Should().Be("<strong>aaaa</strong>bbbb<strong>cc</strong>");
        md.Render("__WORD__ BOB __PON__").Should().Be("<strong>WORD</strong> BOB <strong>PON</strong>");
        md.Render("__aa    bb__  __cc   aa__").Should().Be("<strong>aa    bb</strong>  <strong>cc   aa</strong>");
        md.Render("__aa    bb__").Should().Be("<strong>aa    bb</strong>");
        md.Render("__aa__ __bb__" + '\n' + "__aa__ __bb__").Should().Be("<strong>aa</strong> <strong>bb</strong>" + '\n' + "<strong>aa</strong> <strong>bb</strong>");
    }

    [Test]
    public void Test_BoldWithOtherTags()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("#  __aaa__").Should().Be("<h1><strong>aaa</strong></h1>");
        md.Render("__aaa_b_a__").Should().Be("<strong>aaa<em>b</em>a</strong>");
        md.Render("#  __aaa __").Should().Be("<h1>__aaa __</h1>");
        md.Render("_a __bbb__").Should().Be("_a <strong>bbb</strong>");
        md.Render("__a _bbb__").Should().Be("<strong>a _bbb</strong>");
        md.Render("#  __aaa__ __bb__").Should().Be("<h1><strong>aaa</strong> <strong>bb</strong></h1>");
        md.Render("__aaa _b_  _cc_ a__").Should().Be("<strong>aaa <em>b</em>  <em>cc</em> a</strong>");
        md.Render("__aaa _b_  _cc _ a__").Should().Be("<strong>aaa <em>b</em>  _cc _ a</strong>");
    }

    [Test]
    public void Test_BoldInPartOfWord()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("__нач__але").Should().Be("<strong>нач</strong>але");
        md.Render("сер__еди__не").Should().Be("сер<strong>еди</strong>не");
        md.Render("кон__це.__").Should().Be("кон<strong>це.</strong>");
        md.Render("aa  сер__еди__не").Should().Be("aa  сер<strong>еди</strong>не");
        md.Render("кон__це.__ bb").Should().Be("кон<strong>це.</strong> bb");
    }

    [Test]
    public void Test_BoldSeveralWords()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("ра__зных словах__").Should().Be("ра__зных словах__");
        md.Render("ра__зных сл__овах").Should().Be("ра__зных сл__овах");
    }

    [Test]
    public void Test_BoldTextWithSpaces()
    {
        using var scope = new FluentAssertions.Execution.AssertionScope();
        md.Render("__        __").Should().Be("");
        md.Render("__ подчерки__").Should().Be("__ подчерки__");
        md.Render("__подчерки __").Should().Be("__подчерки __");
    }
}