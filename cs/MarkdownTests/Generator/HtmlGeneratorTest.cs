using FluentAssertions;
using Markdown.Generator;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules;
using Markdown.Tokenizer;
using Markdown.Tokens;

namespace MarkdownTests.Generator;

[TestFixture]
[TestOf(typeof(HtmlGenerator))]
public class HtmlGeneratorTest
{
    private readonly BodyRule rule = new();
    private readonly HtmlGenerator generator = new();
    private readonly MdTokenizer tokenizer = new();
    
    [Test]
    public void Render_ShouldCorrectlyRenderHeader()
    {
        var root = GenerateNode("# abc def", out var tokens);
        var html = generator.Render(root, tokens);
        html.Should().Be("<div><h1>abc def</h1></div>");
    }
    [Test]
    public void Render_ShouldCorrectlyRenderBold()
    {
        var root = GenerateNode("abc __def__ ghi", out var tokens);
        var html = generator.Render(root, tokens);
        html.Should().Be("<div><p>abc <strong>def</strong> ghi</p></div>");
    }
    [Test]
    public void Render_ShouldCorrectlyRenderItalic()
    {
        var root = GenerateNode("abc _def_ ghi", out var tokens);
        var html = generator.Render(root, tokens);
        html.Should().Be("<div><p>abc <em>def</em> ghi</p></div>");
    }
    [Test]
    public void Render_ShouldCorrectlyRenderPlainText()
    {
        var root = GenerateNode("abc def ghi jkl", out var tokens);
        var html = generator.Render(root, tokens);
        html.Should().Be("<div><p>abc def ghi jkl</p></div>");
    }
    [Test]
    public void Render_ShouldCorrectlyRenderNestedTags()
    {
        var root = GenerateNode("# abc _def_ __ghi__", out var tokens);
        var html = generator.Render(root, tokens);
        html.Should().Be("<div><h1>abc <em>def</em> <strong>ghi</strong></h1></div>");
    }

    [Test]
    public void Render_ShouldCorrectlyRenderUnorderedList()
    {
        var root = GenerateNode("* abc def\n* ghi jkl", out var tokens);
        var html = generator.Render(root, tokens);
        html.Should().Be("<div><ul><li>abc def</li><li>ghi jkl</li></ul></div>");
    }
    
    private Node? GenerateNode(string text, out List<Token> tokens)
    {
        tokens = tokenizer.Tokenize($"{text}\n");
        return rule.Match(tokens);
    }
}