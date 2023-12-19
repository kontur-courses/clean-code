using System.Text;
using FluentAssertions;
using Markdown.Renderer;
using Markdown.TokenConverter;

namespace MarkdownTests.Renderer;

[TestFixture]
public class MarkdownRendererTests
{
    private readonly MarkdownRenderer renderer = new(TestDataFactory.Lexer, new MarkdownTokenConverter());

    [Test, Timeout(5000)]
    public void Render_HasSufficientPerformance_OnLongInputText()
    {
        var text = new StringBuilder();

        for (var i = 0; i < 250000; i++)
            text.Append("_a_ __2 _ ");

        renderer.Render(text.ToString());
    }

    [Test]
    public void Render_ThrowsArgumentException_WhenTextIsNull()
    {
        Assert.Throws<ArgumentException>(() => renderer.Render(null!));
    }

    [TestCase("__a__ text\n_a_", "<strong>a</strong> text\n<em>a</em>")]
    [TestCase("__a__ text\r\n_a_", "<strong>a</strong> text\n<em>a</em>")]
    [TestCase("__a__ text\n# _a_", "<strong>a</strong> text\n<h1><em>a</em></h1>")]
    [TestCase("\r\n\r\n_a_\n# a", "<em>a</em>\n<h1>a</h1>")]
    [TestCase("\n", "")]
    public void Render_ReturnsCorrectResult_OnMultipleLines(string initial, string expected)
    {
        AssertRenderedStringReturnedCorrectResult(initial, expected);
    }

    [TestCase("__wasd__ \n#header", "<strong>wasd</strong> \n#header")]
    [TestCase("__wasd__ # header", "<strong>wasd</strong> # header")]
    [TestCase("#Заголовок _с разными_ __символами__", "#Заголовок <em>с разными</em> <strong>символами</strong>")]
    [TestCase("# _wasd_", "<h1><em>wasd</em></h1>")]
    [TestCase(@"\\_a_ _12_ a __a__", @"\<em>a</em> <em>12</em> a <strong>a</strong>")]
    [TestCase(@"_ a_ _a_ _a _ \d", @"_ a_ <em>a</em> _a _ \d")]
    public void Render_ReturnsCorrectResult_OnComplexInputText(string initial, string expected)
    {
        AssertRenderedStringReturnedCorrectResult(initial, expected);
    }

    private void AssertRenderedStringReturnedCorrectResult(string initial, string expected)
    {
        var result = renderer.Render(initial);

        result
            .Should()
            .Be(expected);
    }
}   