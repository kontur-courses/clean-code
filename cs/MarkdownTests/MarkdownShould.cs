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
        var result=md.Render();

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
    public void ReturnText_WhenNoSettingsProvided()
    {
        var text = "abc defg";
        var settings=new WrapperSettingsProvider();
        var md = new Md(text,settings);

        var expectedResult = text;
        var result=md.Render();

        result.Should().BeEquivalentTo(expectedResult);
    }
}