using System.Text;
using FluentAssertions;
using Markdown.Filter;
using Markdown.Lexer;
using Markdown.Renderer;
using Markdown.TokenConverter;
using Markdown.Tokens.Types;

namespace MarkdownTests.Renderer;

[TestFixture]
public class MarkdownRendererTests
{
    private MarkdownRenderer renderer = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var lexer = new MarkdownLexerBuilder(new MarkdownFilter(), '\\')
            .WithTokenType(new EmphasisToken())
            .WithTokenType(new StrongToken())
            .WithTokenType(new HeaderToken())
            .Build();

        var tokenConverter = new MarkdownTokenConverter();

        renderer = new MarkdownRenderer(lexer, tokenConverter);
    }

    [Test, Timeout(1500)]
    public void Render_HasSufficientPerformance_OnLongInputText()
    {
        var text = new StringBuilder();

        for (var i = 0; i < 250000; i++)
            text.Append("_a_ ");

        renderer.Render(text.ToString());
    }

    [TestCase(null)]
    [TestCase("")]
    public void Render_ThrowsArgumentException_OnNullOrEmptyParameters(string text)
    {
        Assert.Throws<ArgumentException>(() => renderer.Render(text));
    }

    [TestCase("__a__ text\n_a_", "<strong>a</strong> text\n<em>a</em>")]
    [TestCase("__a__ text\r\n_a_", "<strong>a</strong> text\n<em>a</em>")]
    [TestCase("__a__ text\n# _a_", "<strong>a</strong> text\n<h1><em>a</em></h1>")]
    [TestCase("\r\n\r\n_a_\n# a", "<em>a</em>\n<h1>a</h1>")]
    [TestCase("\n", "")]
    public void Render_ReturnsCorrectResult_OnMultipleLines(string initial, string expected)
    {
        var result = renderer.Render(initial);

        result
            .Should()
            .Be(expected);
    }
}