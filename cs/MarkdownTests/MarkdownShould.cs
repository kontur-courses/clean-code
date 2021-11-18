using FluentAssertions;
using Markdown;
using NUnit.Framework;

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
}