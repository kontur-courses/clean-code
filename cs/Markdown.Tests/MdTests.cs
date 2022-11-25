using System.Diagnostics;
using Markdown.Convert;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
[Parallelizable]
public class MdTests
{
    [TestCase("# heading", "<h1>heading</h1>")]
    [TestCase("_italics_", "<em>italics</em>")]
    [TestCase("__bold__", "<strong>bold</strong>")]
    public void Should_RenderSingleTag(string input, string expectedResult)
    {
        var converter = new Md(new MarkdownToHtmlConverter());
        var result = converter.Render(input);
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [TestCase("# heading _italics_", "<h1>heading <em>italics</em></h1>")]
    [TestCase("# heading __bold__", "<h1>heading <strong>bold</strong></h1>")]
    [TestCase("__bold _italics_ bold__", "<strong>bold <em>italics</em> bold</strong>")]
    [TestCase("# heading __bold _italics_ bold__", "<h1>heading <strong>bold <em>italics</em> bold</strong></h1>")]
    public void Should_RenderNestedTags(string input, string expectedResult)
    {
        var converter = new Md(new MarkdownToHtmlConverter());
        var result = converter.Render(input);
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [TestCase("# heading\r\n", "<h1>heading\r\n</h1>")]
    [TestCase("_italics_\r\n", "<em>italics</em>\r\n")]
    [TestCase("__bold__\r\n", "<strong>bold</strong>\r\n")]
    [TestCase("# heading\r\n _italics_", "<h1>heading\r\n</h1> <em>italics</em>")]
    [TestCase("# heading\r\n __bold__", "<h1>heading\r\n</h1> <strong>bold</strong>")]
    public void Should_RenderTextWithWhitespace(string input, string expectedResult)
    {
        var converter = new Md(new MarkdownToHtmlConverter());
        var result = converter.Render(input);
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [TestCase("\\_not_", "_not_")]
    [TestCase("\\__not_\\_", "_<em>not</em>_")]
    [TestCase("__not\\__", "__not__")]
    [TestCase("\\", "\\")]
    [TestCase("\\a", "\\a")]
    public void ShouldNot_RenderEscapedTags(string input, string expectedResult)
    {
        var converter = new Md(new MarkdownToHtmlConverter());
        var result = converter.Render(input);
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [TestCase("_italics\r\n_", "_italics\r\n_")]
    [TestCase("__italics\r\n__", "__italics\r\n__")]
    public void ShouldNot_RenderSeparatedTags(string input, string expectedResult)
    {
        var converter = new Md(new MarkdownToHtmlConverter());
        var result = converter.Render(input);
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [TestCase("_aaa__bbb_ccc__", "_aaa__bbb_ccc__")]
    [TestCase("__aaa_bbb__ccc_", "__aaa_bbb__ccc_")]
    public void ShouldNot_RenderIntersectingTags(string input, string expectedResult)
    {
        var converter = new Md(new MarkdownToHtmlConverter());
        var result = converter.Render(input);
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [TestCase("123_456_789", "123_456_789")]
    [TestCase("123__456__789", "123__456__789")]
    public void ShouldNot_RenderTagsBetweenNumbers(string input, string expectedResult)
    {
        var converter = new Md(new MarkdownToHtmlConverter());
        var result = converter.Render(input);
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [TestCase("# afa")]
    [TestCase("# heading __strong _strong em_ strong__ heading _em_ __y_afa_ ghg _jkj_u__\r\n")]
    [TestCaseSource(nameof(GetSpecificationText))]
    public void Should_BeLinear(string input)
    {
        const int repeatCount = 25;
        var sw = new Stopwatch();
        sw.Reset();

        var converter = new Md(new MarkdownToHtmlConverter());
        sw.Start();
        converter.Render(input);
        sw.Stop();
        var elapsed = sw.Elapsed.Ticks;
        sw.Reset();

        var text = Enumerable.Repeat(input, repeatCount).ToString();

        sw.Start();
        converter.Render(text!);
        sw.Stop();

        Assert.That(sw.Elapsed.Ticks / elapsed, Is.LessThanOrEqualTo(repeatCount));
    }

    private static IEnumerable<string> GetSpecificationText()
    {
        var directory = Directory.GetParent(TestContext.CurrentContext.TestDirectory)!.Parent!.Parent!.Parent!.Parent!
            .ToString();
        const string file = "MarkdownSpec.md";
        var fullName = Path.Combine(directory, file);
        var task = File.ReadAllTextAsync(fullName);
        var result = task.Result;
        yield return result;
    }
}