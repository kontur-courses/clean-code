using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokenizer;

namespace MarkdownTests.Parser.Rules;

[TestFixture]
[TestOf(typeof(BoldRule))]
public class BoldRuleTest
{
    private readonly BoldRule rule = new();
    private readonly MdTokenizer tokenizer = new();

    [TestCase("abc")]
    [TestCase("abc def ghi")]
    public void Match_ShouldMatch_SimpleText(string text)
    {
        var tokens = tokenizer.Tokenize($"__{text}__");

        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.ToText(tokens).Should().Be(text);
        node.NodeType.Should().Be(NodeType.Bold);
        node.Children.Should().ContainSingle(n => n.NodeType == NodeType.Text);
    }

    [TestCase("abc")]
    [TestCase("abc def ghi")]
    public void Match_ShouldMatch_InnerItalic(string text)
    {
        var tokens = tokenizer.Tokenize($"___{text}___");
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.NodeType.Should().Be(NodeType.Bold);
        node.ToText(tokens).Should().Be($"{text}");
        node.Children.Should().ContainSingle(n => n.NodeType == NodeType.Italic);
    }

    [TestCase("abc def _ghi_")]
    [TestCase("_abc_ def ghi")]
    public void Match_ShouldMatch_TextWithItalicTagAfterOpenedBoldOrBeforeClosedBold(string text)
    {
        var tokens = tokenizer.Tokenize($"__{text}__");
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.Children
            .Select(n => n.NodeType).Should().HaveCount(2)
            .And.BeEquivalentTo([NodeType.Text, NodeType.Italic]);
        node.NodeType.Should().Be(NodeType.Bold);
    }

    [Test]
    public void Match_ShouldMatch_TextWithInnerItalicTag()
    {
        const string text = "abc _def_ ghi";
        var tokens = tokenizer.Tokenize($"__{text}__");

        var node = rule.Match(tokens) as TagNode;

        node.Should().NotBeNull();
        node.Children
            .Select(n => n.NodeType)
            .Should().HaveCount(3)
            .And.BeEquivalentTo([NodeType.Text, NodeType.Italic, NodeType.Text]);
        node.NodeType.Should().Be(NodeType.Bold);
    }

    [TestCase("a__bc__", 1, ExpectedResult = "bc")]
    [TestCase("a__b__c", 1, ExpectedResult = "b")]
    [TestCase("__a__bc", 0, ExpectedResult = "a")]
    [TestCase("__a*__bc", 0, ExpectedResult = "a*")]
    [TestCase("__a/__bc", 0, ExpectedResult = "a/")]
    [TestCase("f__a#__bc", 1, ExpectedResult = "a#")]
    public string Match_ShouldMatch_WhenTagInsideWord(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens, begin) as TagNode;
        
        node.Should().NotBeNull();
        node.Children.Should().Contain(n => n.NodeType == NodeType.Text);
        return node.Children.ToText(tokens);
    }

    [TestCase("a__bc_def_gh__i", 1)]
    public void Match_ShouldMatch_WhenTagInsideWordAndItalicTagInsideTag(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens, begin) as TagNode;
        
        node.Should().NotBeNull();
        node.Children
            .Select(n => n.NodeType)
            .Should().HaveCount(3)
            .And.BeEquivalentTo([NodeType.Text, NodeType.Italic, NodeType.Text]);
        node.NodeType.Should().Be(NodeType.Bold);
    }

    [TestCase("abc__123__def", 1)]
    [TestCase("abc__123__", 1)]
    [TestCase("__123__abc", 0)]
    [TestCase("abc de__123__f", 3)]
    public void Match_ShouldNotMatch_Numbers(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens, begin) as TagNode;
        
        node.Should().BeNull();
    }
    
    [TestCase("ab__c def__", 1)]
    [TestCase("__abc d__ef", 0)]
    [TestCase("a__bc d__ef", 1)]
    public void Match_ShouldNotMatch_WhenTagInDifferentWords(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens, begin) as TagNode;
        
        node.Should().BeNull();
    }

    [TestCase("__abc __", 0)]
    [TestCase("__ abc__", 0)]
    [TestCase("__abc def __", 0)]
    [TestCase("__ abc def__", 0)]
    public void Match_ShouldNotMatch_WhenSpaceIsAfterOpeningTagOrBeforeClosing
        (string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens, begin) as TagNode;
        
        node.Should().BeNull();
    }
}