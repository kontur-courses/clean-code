using FluentAssertions;
using Markdown.Parser.Nodes.TagNodes;
using Markdown.Parser.Rules.TagRules;
using Markdown.Parser.Tools;
using Markdown.Tokenizer;

namespace MarkdownTests.Parser.Rules.TagRules;

[TestFixture]
public class HrefRuleTest
{
    private readonly HrefRule rule = new();
    private readonly MarkdownTokenizer tokenizer = new();

    [Test]
    public void HrefRule_Match_SimpleHref()
    {
        const string text = "[Simple href](google.com)";
        var tokens = tokenizer.Tokenize($"{text}");
        
        var node = rule.Match(tokens) as HrefNode;
        
        node.Should().NotBeNull();
        node.Href.Should().Be("google.com");
        node.Children.ToText(tokens).Should().Be("Simple href");
    }
}