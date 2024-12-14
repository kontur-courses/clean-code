using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokenizer;

namespace MarkdownTests.Parser.Rules;

[TestFixture]
[TestOf(typeof(ParagraphRule))]
public class ParagraphRuleTest
{
    private readonly ParagraphRule rule = new();
    private readonly MdTokenizer tokenizer = new();
    
    [TestCase("abcdefghi")]
    [TestCase("abc def ghi ")]
    public void Match_ShouldMatch_SimpleCase(string text)
    {
        var tokens = tokenizer.Tokenize($"{text}\n");

        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.NodeType.Should().Be(NodeType.Paragraph);
        node.Consumed.Should().Be(tokens.Count);
        node.ToText(tokens).Should().Be(text);
    }

    [Test]
    public void Match_ShouldMatch_WhenParagraphWithInnerTags()
    {
        const string text = "abc _def_ __ghi jkl__";
        var tokens = tokenizer.Tokenize($"{text}\n");
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.NodeType.Should().Be(NodeType.Paragraph);
        
        node.Consumed.Should().Be(tokens.Count);
        node.Children.Select(n => n.NodeType)
            .Should().HaveCount(4)
            .And.BeEquivalentTo([NodeType.Text, NodeType.Italic, NodeType.Bold, NodeType.Text]);
        
        node.Children
            .First(n => n.NodeType == NodeType.Bold)
            .ToText(tokens).Should().Be("ghi jkl");
        node.Children
            .First(n => n.NodeType == NodeType.Italic)
            .ToText(tokens).Should().Be("def");
    }

    [TestCase("_abc __def ghi_ jkl__")]
    [TestCase("_abc __def ghi jkl")]
    public void Match_ShouldMatchAsText_WhenInnerTagsIntersect(string text)
    {
        var tokens = tokenizer.Tokenize($"{text}\n");
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.ToText(tokens).Should().Be(text);
        node.NodeType.Should().Be(NodeType.Paragraph);
        node.Children.Should().OnlyContain(n => n.NodeType == NodeType.Text);
    }
    
    [TestCase(@"abc \_def\_ ghi", ExpectedResult = "abc _def_ ghi")]
    public string? Match_ShouldMatchCorrectly_WhenTagsInParagraphAreEscaped(string text)
    {
        var tokens = tokenizer.Tokenize($"{text}\n");
        var node = rule.Match(tokens) as TagNode;
        return node?.ToText(tokens);
    }
}