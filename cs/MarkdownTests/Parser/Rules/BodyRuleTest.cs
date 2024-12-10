using FluentAssertions;
using Markdown;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules;
using Markdown.Parser.Rules.Tools;

namespace MarkdownTests.Parser.Rules;

[TestFixture]
[TestOf(typeof(BodyRule))]
public class BodyRuleTest
{
    private readonly BodyRule rule = new();
    private readonly MdTokenizer tokenizer = new();
    
    [Test]
    public void Match_ShouldMatchCorrectly_WhenSimpleText()
    {
        const string text = 
            """
            abc def ghi
            jkl mno pqrs
            ter uvw xyz
            """;
        var tokens = tokenizer.Tokenize($"{text}\n");
        
        var node = rule.Match(tokens) as TagNode;
        node.Should().NotBeNull();
        node.ToText(tokens).Should().Be(text.Replace("\n", ""));
        node.Children.Should().OnlyContain(n => n.NodeType == NodeType.Paragraph);
    }
    
    [Test]
    public void Match_ShouldMatchCorrectly_WhenTextWithHeader()
    {
        const string text = 
            """
            # abc def ghi
            jkl mno pqr
            """;
        var tokens = tokenizer.Tokenize($"{text}\n");
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.Children.Select(n => n.NodeType).Should().BeEquivalentTo(
            [NodeType.Header, NodeType.Paragraph], options => options.WithStrictOrdering());
        node.ToText(tokens).Should().Be("abc def ghi\rjkl mno pqr");
    }
    
    [Test]
    public void Match_ShouldMatchCorrectly_WhenTextWithEscapedHeader()
    {
        const string text = 
            """
            \# abc def ghi
            jkl mno pqr
            """;
        var tokens = tokenizer.Tokenize($"{text}\n");
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.Children.Select(n => n.NodeType).Should().BeEquivalentTo(
            [NodeType.Escape, NodeType.Paragraph, NodeType.Paragraph], options => options.WithStrictOrdering());
        node.ToText(tokens).Should().Be("# abc def ghi\rjkl mno pqr");
    }

    [Test]
    public void Match_ShouldMatchCorrectly_WhenTextWithUnorderedList()
    {
        const string text =
            """
            * abc def ghi
            * jkl mno pqr
            """;
        var tokens = tokenizer.Tokenize($"{text}\n");
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.Children.Count.Should().Be(1);
        node.Children.First().NodeType.Should().Be(NodeType.UnorderedList);
    }
    
    [Test]
    public void Match_ShouldMatchCorrectly_WhenTextWithEscapedList()
    {
        const string text = 
            """
            \* abc def ghi
            jkl mno pqr
            """;
        var tokens = tokenizer.Tokenize($"{text}\n");
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.Children.Select(n => n.NodeType).Should().BeEquivalentTo(
            [NodeType.Escape, NodeType.Paragraph, NodeType.Paragraph], options => options.WithStrictOrdering());
        node.ToText(tokens).Should().Be("* abc def ghi\rjkl mno pqr");
    }
}