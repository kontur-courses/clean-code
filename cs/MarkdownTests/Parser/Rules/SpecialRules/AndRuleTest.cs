using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.SpecialRules;
using Markdown.Parser.Tools;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tokens;

namespace MarkdownTests.Parser.Rules.SpecialRules;

[TestFixture]
public class AndRuleTest
{
    private readonly MarkdownTokenizer tokenizer = new();
    private readonly AndRule rule = new(TokenType.WORD, TokenType.NUMBER);

    [TestCase("WordAnd123")]
    [TestCase("BigNumCase12456790211247")]
    public void AndRule_Match_ShouldMatchRightPattern(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens) as SpecNode;

        node.Should().NotBeNull();
        node.Nodes.Select(n => n.NodeType).Should().BeEquivalentTo(
            [NodeType.TEXT, NodeType.TEXT], 
            options => options.WithStrictOrdering());
        node.ToText(tokens).Should().Be(text);
    }

    [TestCase("12345Word")]
    [TestCase("EvenWithRight beginning")]
    [TestCase("This string doesnt match rule")]
    public void AndRule_Match_ShouldNotMatchWrongPattern(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        var node = rule.Match(tokens) as SpecNode;
        node.Should().BeNull();
    }
}