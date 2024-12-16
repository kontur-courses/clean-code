using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.TagRules;
using Markdown.Parser.Tools;
using Markdown.Tokenizer;

namespace MarkdownTests.Parser.Rules.TagRules;

[TestFixture]
public class ItalicRuleTest
{
    private readonly ItalicRule rule = new();
    private readonly MarkdownTokenizer tokenizer = new();
    
    [TestCase("Italic")]
    [TestCase("Italic tag with spaces")]
    public void ItalicRule_Match_SimpleMatch(string text)
    {
        var tokens = tokenizer.Tokenize($"_{text}_");
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.NodeType.Should().Be(NodeType.ITALIC);
        node.Children.Should().ContainSingle(n => n.NodeType == NodeType.TEXT);
        node.ToText(tokens).Should().Be(text);
    }

    [TestCase("Italic tag with_123_numbers", 6)]
    [TestCase("Numbers that separates with underscores 12_34_56", 10)]
    public void ItalicRule_Match_ShouldNotMatchTextWithNumbers(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        var node = rule.Match(tokens, begin) as TagNode;
        node.Should().BeNull();
    }

    [TestCase("Italic tag that_ in different_ words", 5)]
    [TestCase("Italic tag that _ in different words_", 6)]
    public void ItalicRule_Match_ShouldNotMatchWhenSpaceAfterOpenTag(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        var node = rule.Match(tokens, begin) as TagNode;
        node.Should().BeNull();
    }
    
    [TestCase("_abc def g_hi", 0)]
    [TestCase("Italic tag that _in different _words", 6)]
    [TestCase("Italic tag that _in different words _", 6)]
    public void ItalicRule_Match_ShouldNotMatchWhenSpaceBeforeCloseTag(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        var node = rule.Match(tokens, begin) as TagNode;
        node.Should().BeNull();
    }
    
    [TestCase("abc _def__", 2)]
    [TestCase("Not pair __symbols case_", 4)]
    [TestCase("Not pair _symbols case__", 4)]
    public void ItalicRule_Match_DifferentUnderscoresShouldNotMatch(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        var node = rule.Match(tokens, begin) as TagNode;
        node.Should().BeNull();
    }

    [TestCase("__Some text__ with bold in the start")]
    [TestCase("Some text __with bold__ text in the middle")]
    [TestCase("Some text with bold text __in the end__")]
    public void ItalicRule_Match_BoldTagInItalicShouldNotBeMatched(string text)
    {
        var tokens = tokenizer.Tokenize($"_{text}_");
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.ToText(tokens).Should().Be(text);
    }
}