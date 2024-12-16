using FluentAssertions;
using Markdown.Generator.Generators;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.TagRules;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tokens;

namespace MarkdownTests.Generator.Generators;

[TestFixture]
public class HtmlGeneratorTest
{
    private readonly BodyRule rule = new();
    private readonly HtmlGenerator generator = new();
    private readonly MarkdownTokenizer tokenizer = new();

    [Test]
    public void HtmlGenerator_Render_Headline()
    {
        var root = GenerateNode("# Headline", out var tokens);
        var html = generator.Render(root, tokens);
        html.Should().Be("<div><h1>Headline</h1></div>");
    }

    [Test]
    public void HtmlGenerator_Render_Bold()
    {
        var root = GenerateNode("Text with __bold__ tag", out var tokens);
        var html = generator.Render(root, tokens);
        html.Should().Be("<div><p>Text with <strong>bold</strong> tag</p></div>");
    }

    [Test]
    public void HtmlGenerator_Render_Italic()
    {
        var root = GenerateNode("Text with _italic_ tag", out var tokens);
        var html = generator.Render(root, tokens);
        html.Should().Be("<div><p>Text with <em>italic</em> tag</p></div>");
    }

    [Test]
    public void HtmlGenerator_Render_PlainText()
    {
        var root = GenerateNode("Just a plain text", out var tokens);
        var html = generator.Render(root, tokens);
        html.Should().Be("<div><p>Just a plain text</p></div>");
    }

    [Test]
    public void HtmlGenerator_Render_NestedTags()
    {
        var root = GenerateNode("# Nested _headline_ __tag__", out var tokens);
        var html = generator.Render(root, tokens);
        html.Should().Be("<div><h1>Nested <em>headline</em> <strong>tag</strong></h1></div>");
    }

    private Node GenerateNode(string text, out List<Token> tokens)
    {
        tokens = tokenizer.Tokenize($"{text}\n");
        return rule.Match(tokens)!;
    }
}