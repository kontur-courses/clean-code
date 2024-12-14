using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokenizer;
using Markdown.Tokens;

namespace MarkdownTests.Parser.Rules.BoolRules;

[TestFixture]
[TestOf(typeof(AndRule))]
public class AndRuleTest
{
    private readonly MdTokenizer tokenizer = new();
    private readonly AndRule rule = new(TokenType.Word, TokenType.Number);

    [TestCase("abc123")]
    public void Match_ShouldMatch_WhenRightPattern(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens) as SpecNode;
        
        node.Should().NotBeNull();
        node.Nodes.Select(n => n.NodeType).Should().BeEquivalentTo(
            [NodeType.Text, NodeType.Text], options => options.WithStrictOrdering());
        node.ToText(tokens).Should().Be(text);
    }

    [TestCase("123abc")]
    [TestCase("abc")]
    [TestCase("123")]
    public void Match_ShouldNotMatch_WhenWrongPattern(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens) as SpecNode;
        
        node.Should().BeNull();
    }
}