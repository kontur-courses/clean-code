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
        settings.TryAddSetting(new("", "$(text)", "<p>$(text)</p>"));
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
        settings.TryAddSetting(new("", "$(text)", "<p>$(text)</p>"));
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
        settings.TryAddSetting(new("_", "_$(text)_", "<em>$(text)</em>"));
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
        settings.TryAddSetting(new("__", "__$(text)__", "<strong>$(text)</strong>"));
        var md = new Md(input, settings);

        var result = md.Render();

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void WrapBoldInStrong_WhenSettingsForCursiveProvided()
    {
        var text = "__abc defg__";
        var settings = new WrapperSettingsProvider();
        settings.TryAddSetting(new("_", "_$(text)_", "<em>$(text)</em>"));
        settings.TryAddSetting(new("__", "__$(text)__", "<strong>$(text)</strong>"));
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
        var settings = new WrapperSettingsProvider();
        settings.TryAddSetting(new("_", "_$(text)_", "<em>$(text)</em>"));
        settings.TryAddSetting(new("__", "__$(text)__", "<strong>$(text)</strong>"));
        var md = new Md(input, settings);

        var result = md.Render();

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void WrapSeveralLinesInP_WithNestedTags()
    {
        var text = "_cursive_ __bold _cursive in bold___\nsecond line _second line in cursive_ ___bold second with cursive_ still bold__";
        var md = new Md(text);

        var expectedResult = "<p><em>cursive</em> <strong>bold <em>cursive in bold</em></strong></p><p>second line <em>second line in cursive</em> <strong><em>bold second with cursive</em> still bold</strong></p>";
        var actualResult = md.Render();

        actualResult.Should().BeEquivalentTo(expectedResult);

    }

    [TestCase(100, 100)]
    [TestCase(1000, 100)]
    [TestCase(100, 1000)]
    [Timeout(2000)]
    public void WorkLinearlyToItsInput(int actionCount, int textScale)
    {
        var text = "__abc _def_ hkl__ mn";
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