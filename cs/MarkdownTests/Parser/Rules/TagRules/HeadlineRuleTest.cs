using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.TagRules;
using Markdown.Parser.Tools;
using Markdown.Tokenizer;

namespace MarkdownTests.Parser.Rules.TagRules;

[TestFixture]
public class HeadlineRuleTest
{
    private readonly HeadlineRule rule = new();
    private readonly MarkdownTokenizer tokenizer = new();
    
    [TestCase("# Headline\n", ExpectedResult = "Headline")]
    [TestCase("# Simple headline\n", ExpectedResult = "Simple headline")]
    public string? HeadlineRule_Match_SimpleHeadlineMatch(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        var node = rule.Match(tokens);
        return node?.ToText(tokens);
    }
    
    [Test]
    public void HeadlineRule_Match_HeadlineWithInnerTagsMatch()
    {
        const string text = "Headline with _italic_ and __bold__ tags";
        var tokens = tokenizer.Tokenize($"# {text}\n");
        
        var node = rule.Match(tokens) as TagNode;

        node.Should().NotBeNull();
        node.Children.Select(n => n.NodeType).Should().BeEquivalentTo(
            [NodeType.TEXT, NodeType.ITALIC, NodeType.TEXT, NodeType.BOLD, NodeType.TEXT],
            options => options.WithStrictOrdering());
        node.Children.First(n => n.NodeType == NodeType.BOLD).ToText(tokens).Should().Be("bold");
        node.Children.First(n => n.NodeType == NodeType.ITALIC).ToText(tokens).Should().Be("italic");
    }
}