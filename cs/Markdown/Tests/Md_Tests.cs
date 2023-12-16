using System.Diagnostics;
using FluentAssertions;
using Markdown.TagHandlers;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class MdTests
{
    private Md md;

    [SetUp]
    public void SetUp()
    {
        md = new Md();
    }

    [TestCase("\\# abc", ExpectedResult = "# abc")]
    public string MdRender_ShouldIgnoreEscapedTags(string text)
    {
        var rendered = md.Render(text);

        return rendered;
    }   

    [TestCase("# abc", ExpectedResult = "<h1>abc</h1>")]
    [TestCase("_abc_", ExpectedResult = "<em>abc</em>")]
    [TestCase("__abc__", ExpectedResult = "<strong>abc</strong>")]
    [TestCase("[abc](https://abc.abc)", ExpectedResult = "<a href\"https://abc.abc\">abc</a>")]
    public string MdRender_ShouldISupportDifferentTags(string text)
    {
        var rendered = md.Render(text);

        return rendered;
    }

    [TestCase("# abc _abc_", ExpectedResult = "<h1>abc <em>abc</em></h1>")]
    public string MdRender_ShouldRenderNestedTags(string text)
    {
        var rendered = md.Render(text);

        return rendered;
    }
    
    [TestCase("")]
    [TestCase("__")]
    [TestCase("__abc")]
    [TestCase("_ how are you _")]
    [TestCase("#how are you")]
    [TestCase("__ how are _you __ i am_")]
    [TestCase("__ how are _you __ i am_")]
    public void MdRender_ShouldIgnoreInvalidTags(string text)
    {
        var rendered = md.Render(text);
        rendered.Should().Be(text);
    }
    
    [TestCase("# how are you", ExpectedResult = "<h1>how are you</h1>")]
    [TestCase("asd\n# how are you", ExpectedResult = "asd\n<h1>how are you</h1>")]
    [TestCase("asd\n\\# how are you", ExpectedResult = "asd\n# how are you")]
    [TestCase("asd\n# how are you \n abc", ExpectedResult = "asd\n<h1>how are you</h1>\n abc")]
    [TestCase("__how _are_ you__", ExpectedResult = "<strong>how <em>are</em> you</strong>")]
    public string MdRender_ShouldReturnCorrect(string text)
    {
        var rendered = md.Render(text);
        return rendered;
    }

    [Test]
    public void MdRender_ShouldWorkLinearly()
    {
        // TODO
    }
}