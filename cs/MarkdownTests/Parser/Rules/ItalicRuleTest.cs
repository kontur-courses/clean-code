using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokenizer;

namespace MarkdownTests.Parser.Rules;

[TestFixture]
[TestOf(typeof(ItalicRule))]
public class ItalicRuleTest
{
    private readonly ItalicRule rule = new();
    private readonly MdTokenizer tokenizer = new();
    
    [TestCase("abc")]
    [TestCase("abc def ghi jkl")]
    public void Match_ShouldMatch_SimpleText(string text)
    {
        var tokens = tokenizer.Tokenize($"_{text}_");

        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.NodeType.Should().Be(NodeType.Italic);
        node.Children.Should().ContainSingle(n => n.NodeType == NodeType.Text);
        node.ToText(tokens).Should().Be(text);
    }

    
    [TestCase("ab#c d/ef g*hi jkl")]
    [TestCase("#/*")]
    public void Match_ShouldMatch_TextWithSpecialCharacters(string text)
    {
        var tokens = tokenizer.Tokenize($"_{text}_");
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.NodeType.Should().Be(NodeType.Italic);
    }
    
    [TestCase("abc def ghi_123_jkl", 5)]
    [TestCase("def 12_34_56 ghi jkl", 3)]
    public void Match_ShouldNotMatch_TextWithNumbers(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens, begin) as TagNode;
        
        node.Should().BeNull();
    }

    [TestCase("ab_cde_f", 1, ExpectedResult = "cde")]
    [TestCase("abcd_ef_", 1, ExpectedResult = "ef")]
    [TestCase("abc _de_fghi", 2, ExpectedResult = "de")]
    [TestCase("_ab_c", 0, ExpectedResult = "ab")]
    [TestCase("ab_c_", 1, ExpectedResult = "c")]
    [TestCase("a_*b_", 1, ExpectedResult = "*b")]
    [TestCase("a_/b_", 1, ExpectedResult = "/b")]
    [TestCase("a_#b_", 1, ExpectedResult = "#b")]
    public string Match_ShouldMatch_TagInWord(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens, begin) as TagNode;
        
        node.Should().NotBeNull();
        node.NodeType.Should().Be(NodeType.Italic);
        node.Children.Should().ContainSingle(n => n.NodeType == NodeType.Special);
        return node.ToText(tokens);
    }

    [TestCase("ab_c def gh_i", 1)]
    [TestCase("ab_c def ghi_", 1)]
    [TestCase("_abc def g_hi", 0)]
    public void Match_ShouldNotMatch_TagInDifferentWords(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);

        var node = rule.Match(tokens, begin) as TagNode;
        
        node.Should().BeNull();
    }

    [TestCase("abc_ def_", 1)]
    [TestCase("abc _def _ghi", 2)]
    public void Match_ShouldNotMatch_WhenSpaceAfterOpenTagOrBeforeClosingTag
        (string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens, begin) as TagNode;
        
        node.Should().BeNull();
    }

    [TestCase("abc __def_", 2)]
    [TestCase("abc _def__", 2)]
    public void Match_ShouldNotMatch_DifferentUnderscores(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens, begin) as TagNode;
        
        node.Should().BeNull();
    }

    [TestCase("abc _def __ghi jkl_ mno__", 2)]
    public void Match_ShouldNotMatch_IntersectedUnderscores(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens, begin) as TagNode;
        
        node.Should().BeNull();
    }
    
    [TestCase("__abc def__ ghi jkl")]
    [TestCase("abc __def ghi__ jkl")]
    [TestCase("abc def __ghi jkl__")]
    public void ItalicRule_Match_BoldTagInItalicShouldNotBeMatched(string text)
    {
        var tokens = tokenizer.Tokenize($"_{text}_");
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.ToText(tokens).Should().Be(text);
    }
}