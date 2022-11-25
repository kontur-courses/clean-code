using Markdown.Html;
using Markdown.Interfaces;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class RendererTests
{
    private Renderer renderer;
    private IBuilder builder;

    public RendererTests()
    {
       builder = new HtmlBuilder();
    }

    [SetUp]
    public void SetUp()
    {
        var tokenParser = new HtmlTokenParser();
        renderer = new Renderer(tokenParser, builder);
    }
    
    [TestCase("abcdefg", TestName = "Word without tags", ExpectedResult = "<p>abcdefg</p>")]
    [TestCase("# H", TestName = "Heading with white space", ExpectedResult = "<h1>H</h1>")]
    [TestCase("_A_", TestName="Cursive", ExpectedResult = "<p><em>A</em></p>")]
    [TestCase("__H__",  TestName="Bold", ExpectedResult = "<p><strong>H</strong></p>")]
    [TestCase("в _нач_але, и в сер_еди_не, и в кон_це._",  TestName="Highlighting a part of a word", 
        ExpectedResult = "<p>в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em></p>")]
    public string Parse_SuccessPath_ShouldReturnHtmlFormatString(string markdownData)
    {
        return renderer.Render(markdownData);
    }
    
    [TestCase("#H",  TestName = "Heading without white space", ExpectedResult = "<p>#H</p>")]
    [TestCase("# _H_", TestName = "Heading with nested cursive tag", ExpectedResult = "<h1><em>H</em></h1>")]
    [TestCase("# __H__", TestName = "Heading with nested bold tag", ExpectedResult = "<h1><strong>H</strong></h1>")]
    public string Parse_HeadingTags_ShouldReturnHtmlFormatString(string markdownData)
    {
        return renderer.Render(markdownData);
    }
    
    [TestCase("__A_", TestName="Cursive unpaired tags", ExpectedResult = "<p>__A_</p>")]
    [TestCase("_H__",  TestName="Bold unpaired tags", ExpectedResult = "<p>_H__</p>")]
    [TestCase("_A A_",  TestName="Cursive in different words", ExpectedResult = "<p>_A A_</p>")]
    [TestCase("__H H__",  TestName="Bold in different words", ExpectedResult = "<p>__H H__</p>")]
    [TestCase("_123_",  TestName="Cursive with numbers inside", ExpectedResult = "<p>_123_</p>")]
    [TestCase("__пересечения _двойных__",  TestName="Intersections of double and single markups", ExpectedResult = "<p>__пересечения _двойных__</p>")]
    [TestCase("______________",  TestName="Empty line inside the markup", ExpectedResult = "<p>______________</p>")]
    public string Parse_HighlightingTags_WithIncorrectMarkup(string markdownData)
    {
        return renderer.Render(markdownData);
    }
}