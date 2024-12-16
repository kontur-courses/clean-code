using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.TagRules;
using Markdown.Parser.Tools;
using Markdown.Tokenizer;

namespace MarkdownTests.Parser.Rules.TagRules;

[TestFixture]
public class BoldRuleTest
{
    private readonly BoldRule rule = new();
    private readonly MarkdownTokenizer tokenizer = new();
    
    [TestCase("Bold")]
    [TestCase("Some bold text with spaces")]
    public void BoldRule_Match_SimpleMatch(string text)
    {
        var tokens = tokenizer.Tokenize($"__{text}__");
        
        var node = rule.Match(tokens) as TagNode;

        node.Should().NotBeNull();
        node.ToText(tokens).Should().Be(text);
        node.NodeType.Should().Be(NodeType.BOLD);
        node.Children.Should().ContainSingle(n => n.NodeType == NodeType.TEXT);
    }

    [TestCase("Italic")]
    [TestCase("Italic tag with inner text")]
    public void BoldRule_Match_MatchWithInnerItalic(string text)
    {
        var tokens = tokenizer.Tokenize($"___{text}___");
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.NodeType.Should().Be(NodeType.BOLD);
        node.ToText(tokens).Should().Be($"{text}");
        node.Children.Should().ContainSingle(n => n.NodeType == NodeType.ITALIC);
    }

    [Test]
    public void BoldRule_Match_MatchWithInnerTextAndItalicTag()
    {
        const string text = "Default text _with inner italic_ tag";
        var tokens = tokenizer.Tokenize($"__{text}__");
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.Children.Select(n => n.NodeType).Should().BeEquivalentTo(
            [NodeType.TEXT, NodeType.ITALIC, NodeType.TEXT], 
            options => options.WithStrictOrdering());
        node.NodeType.Should().Be(NodeType.BOLD);
    }
    
    [TestCase("Bold tag with__123__numbers", 6)]
    [TestCase("Numbers that separates with underscores 12__34__56", 10)]
    public void BoldRule_Match_ShouldNotMatchTextWithNumbers(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        var node = rule.Match(tokens, begin) as TagNode;
        node.Should().BeNull();
    }
    
    [TestCase("Bold tag tha__t in different wor__ds", 5)]
    [TestCase("Bold tag t__hat in different words__", 5)]
    [TestCase("Bold tag that__ in different wo__rds", 5)]
    public void BoldRule_Match_ShouldNotMatchTagInDifferentWords(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        var node = rule.Match(tokens, begin) as TagNode;
        node.Should().BeNull();
    }
}