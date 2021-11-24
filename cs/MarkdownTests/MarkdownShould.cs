using FluentAssertions;
using Markdown;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;

namespace MarkdownTests;
public class MarkdownShould
{
    [Test]
    public void WrapSimpleStringInP()
    {
        var text = "abc defg";
        var settings = new WrapperSettingsProvider();
        settings.TryAddSetting(new("", "<p>", "$(text)", "<p>$(text)</p>", true));
        var md = new Md(text, settings);

        var expectedResult = "<p>abc defg</p>";
        var result = md.Render();

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void WrapSeveralLinesInSeparateP()
    {
        var text = "abc\ndefg";
        var settings = new WrapperSettingsProvider();
        settings.TryAddSetting(new("", "<p>", "$(text)", "<p>$(text)</p>", true));
        var md = new Md(text, settings);

        var expectedResult = "<p>abc</p><p>defg</p>";
        var result = md.Render();

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void ReturnText_WhenEmptySettingsProvided()
    {
        var text = "abc defg";
        var settings = new WrapperSettingsProvider();
        var md = new Md(text, settings);

        var expectedResult = text;
        var result = md.Render();

        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestCase("_abc defg_", "<em>abc defg</em>")]
    [TestCase("_abc_ defg", "<em>abc</em> defg")]
    [TestCase("abc _defg_", "abc <em>defg</em>")]
    public void WrapCursiveInEm(string input, string expectedResult)
    {
        var settings = new WrapperSettingsProvider();
        settings.TryAddSetting(new("_", "<em>", "_$(text)_", "<em>$(text)</em>"));
        var md = new Md(input, settings);

        var result = md.Render();

        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestCase("__abc defg__", "<strong>abc defg</strong>")]
    [TestCase("__abc__ defg", "<strong>abc</strong> defg")]
    [TestCase("abc __defg__", "abc <strong>defg</strong>")]
    public void WrapBoldInStrong(string input, string expectedResult)
    {
        var settings = new WrapperSettingsProvider();
        settings.TryAddSetting(new("__", "<strong>", "__$(text)__", "<strong>$(text)</strong>"));
        var md = new Md(input, settings);

        var result = md.Render();

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void WrapBoldInStrong_WhenSettingsForCursiveProvided()
    {
        var text = "__abc defg__";
        var settings = new WrapperSettingsProvider();
        settings.TryAddSetting(new("_", "<em>", "_$(text)_", "<em>$(text)</em>"));
        settings.TryAddSetting(new("__", "<strong>", "__$(text)__", "<strong>$(text)</strong>"));
        var md = new Md(text, settings);

        var expectedResult = "<strong>abc defg</strong>";
        var result = md.Render();

        result.Should().BeEquivalentTo(expectedResult);
    }

    [TestCase("___abc_ defg__", "<strong><em>abc</em> defg</strong>")]
    [TestCase("__abc _defg___", "<strong>abc <em>defg</em></strong>")]
    [TestCase("___abc defg___", "<strong><em>abc defg</em></strong>")]
    public void WrapCursiveInEm_WhenNestedInBold(string input, string expectedResult)
    {
        var settings = new WrapperSettingsProvider(
            new("_", "<em>", "_$(text)_", "<em>$(text)</em>", nestingLevel: 2),
            new("__", "<strong>", "__$(text)__", "<strong>$(text)</strong>", nestingLevel: 1));
        var md = new Md(input, settings);

        var result = md.Render();

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void WrapSeveralLinesInP_WithNestedTags()
    {
        var text = "_cursive_ __bold _cursive in bold___\nsecond line _second line in cursive_ ___bold second with cursive_ still bold__";
        var settings = new WrapperSettingsProvider(
            new("", "<p>", "$(text)", "<p>$(text)</p>", true),
            new("_", "<em>", "_$(text)_", "<em>$(text)</em>", nestingLevel: 2),
            new("__", "<strong>", "__$(text)__", "<strong>$(text)</strong>", nestingLevel: 1));
        var md = new Md(text, settings);

        var expectedResult = "<p><em>cursive</em> <strong>bold <em>cursive in bold</em></strong></p><p>second line <em>second line in cursive</em> <strong><em>bold second with cursive</em> still bold</strong></p>";
        var actualResult = md.Render();

        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void WrapLineInH1_WhenTagProvided()
    {
        var text = "#some header";
        var settings = new WrapperSettingsProvider();
        settings.TryAddSetting(new("#", "<h1>", "#$(text)", "<h1>$(text)</h1>", true));
        var md = new Md(text, settings);

        var expectedResult = "<h1>some header</h1>";
        var actualResult = md.Render();

        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [TestCase(@"\_abc\_", "_abc_")]
    [TestCase(@"\__abc\__", "__abc__")]
    [TestCase(@"\\__abc__", @"\<strong>abc</strong>")]
    [TestCase(@"__\\_abc_ a__", @"<strong>\<em>abc</em> a</strong>")]
    [TestCase(@"__a \\_abc___", @"<strong>a \<em>abc</em></strong>")]
    public void NotWrap_WhenTagEscaped(string input, string expectedResult)
    {
        var md = new Md(input);

        var actualResult = md.Render();

        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [TestCase("[aa](some/link)", @"<a href=""some/link"">aa</a>")]
    [TestCase("![aa](some/link)", @"<img src=""some/link"" alt=""aa""/>")]
    public void Wrap_WithAdditionalSpecification(string input, string expectedResult)
    {
        var setting = new WrapperSettingsProvider(
            new TagSetting("[", "<a>", "[$(text)]($(link))", @"<a href=""$(link)"">$(text)</a>", nestingLevel: int.MaxValue),
            new TagSetting("![", "<img>", "![$(alt)]($(link))", @"<img src=""$(link)"" alt=""$(alt)""/>", nestingLevel: int.MaxValue));
        var md = new Md(input, setting);

        var actualResult = md.Render();

        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [TestCase("_cursive em_", "<em>cursive em</em>", TestName = "Cursive specification", Category = "Specification")]
    [TestCase("__bold strong__", "<strong>bold strong</strong>", TestName = "Bold specification", Category = "Specification")]
    [TestCase("#header h1", "<h1>header h1</h1>", TestName = "Header specification", Category = "Specification")]
    [TestCase(@"\_not cursive em\_", @"_not cursive em_", TestName = "Escape specification", Category = "Specification")]
    [TestCase(@"_curs\_ive em_", "<em>curs_ive em</em>", TestName = "No tag end on escaped char specification", Category = "Specification")]
    [TestCase(@"\\_cursive em_", @"\<em>cursive em</em>", TestName = "Escape escape char specification", Category = "Specification")]
    [TestCase(@"_curs\ive em_", @"<em>curs\ive em</em>", TestName = "Escape char stays when escapes nothing specification", Category = "Specification")]
    [TestCase("__bold _with cursiv___", "<strong>bold <em>with cursiv</em></strong>", TestName = "Nesting cursive in bold specification", Category = "Specification")]
    [TestCase("_cursive __without bold___", "<em>cursive __without bold__</em>", TestName = "No nesting bold in cursive specification", Category = "Specification")]
    [TestCase("numbers1_23_", "numbers1_23_", TestName = "No breaking numbers specification", Category = "Specification")]
    [TestCase("wo_rds_", "wo<em>rds</em>", TestName = "Can break single word specification", Category = "Specification")]
    [TestCase("so_me wo_rds", "so_me wo_rds", TestName = "No breaking across different words specification", Category = "Specification")]
    [TestCase("__paragraph_ a", "_<em>paragraph</em> a", TestName = "No tags without pair specification 1", Category = "Specification")]
    [TestCase("__paragraph_ a", "__paragraph_ a", TestName = "No tags without pair specification 2", Category = "Specification")]
    [TestCase("spaced_ word_", "spaced_ word_", TestName = "No whitespace after opening specification", Category = "Specification")]
    [TestCase("_spaced _word", "_spaced _word", TestName = "No whitespace before closing specification", Category = "Specification")]
    [TestCase("__bold _cursiv__ intersection_", "__bold _cursiv__ intersection_", TestName = "No tags intersection specification", Category = "Specification")]
    [TestCase("__", "__", TestName = "No empty tags specification", Category = "Specification")]
    public void WrapAccordingToSpecification(string input, string expectedResult)
    {
        var md = new Md(input);

        var actualResult = md.Render();

        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [TestCase(100, 100)]
    [TestCase(1000, 100)]
    [TestCase(100, 1000)]
    [Timeout(3000)]
    public void WorkLinearlyToItsInput(int actionCount, int textScale)
    {
        var text = "#__abc _def_\nhkl__ mn";
        WarmUp(text);
        Action action = () => RunMd(text);
        var scaledText = string.Join('\n', Enumerable.Repeat(text, textScale));
        Action scaledAction = () => RunMd(scaledText);
        var timer = new Stopwatch();

        GC.Collect();
        timer.Start();
        for (var i = 0; i < actionCount; i++)
        {
            action();
        }

        timer.Stop();
        var averageTime = timer.Elapsed / actionCount;

        timer.Reset();
        GC.Collect();

        timer.Start();
        for (var i = 0; i < actionCount; i++)
        {
            scaledAction();
        }

        timer.Stop();
        var averageTimeScaled = timer.Elapsed / actionCount;

        averageTimeScaled.Should().BeLessThan(averageTime * textScale * textScale);
        TestContext.WriteLine($"Time on input with {text.Length} length: {averageTime}");
        TestContext.WriteLine($"Time on input with {scaledText.Length} length: {averageTimeScaled}");
    }

    private static void WarmUp(string text)
    {
        for (var i = 0; i < 100; i++)
        {
            var a = RunMd(text);
        }
    }

    private static string RunMd(string input)
    {
        var md = new Md(input);

        return md.Render();
    }
}