using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokenizer;

namespace MarkdownTests.Parser.Rules;

[TestFixture]
[TestOf(typeof(ListItemRule))]
public class ListItemRuleTest
{
    private readonly ListItemRule rule = new();
    private readonly MdTokenizer tokenizer = new();

    [TestCase("abc")]
    [TestCase("abc def ghi")]
    public void Match_ShouldMatch_SimpleListItem(string text)
    {
        var tokens = tokenizer.Tokenize($"* {text}\n");

        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.ToText(tokens).Should().Be(text);
    }
    
    [TestCase("_abc_")]
    [TestCase("__abc__")]
    [TestCase("abc _def_")]
    [TestCase("abc __def__")]
    [TestCase("abc _def_ ghi")]
    [TestCase("abc __def__ ghi")]
    [TestCase("abc __d_e_f__ ghi")]
    public void Match_ShouldMatch_ListItemWithInnerTags(string text)
    {
        var tokens = tokenizer.Tokenize($"* {text}\n");
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
    }

    [Test]
    public void Match_ShouldMatchCorrectly_ComplexListItem()
    {
        const string text = "abc __def__ _ghi_";
        var tokens = tokenizer.Tokenize($"* {text}\n");
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.Children.Count.Should().Be(4);
        node.Children
            .Select(n => n.NodeType).Should().BeEquivalentTo
            ([NodeType.Text, NodeType.Bold, NodeType.Text, NodeType.Italic], 
                o => o.WithStrictOrdering());
    }

    [Test]
    public void Match_ShouldNotMatch_IfNoSpaceAfterAsteriks()
    {
        const string text = "*abc\n";
        
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().BeNull();
    }
    
    [Test]
    public void Match_ShouldNotMatch_IfNoEndOfLine()
    {
        const string text = "* abc";
        
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().BeNull();
    }
}