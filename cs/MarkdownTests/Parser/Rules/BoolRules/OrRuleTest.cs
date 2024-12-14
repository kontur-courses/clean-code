using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Tokenizer;
using Markdown.Tokens;

namespace MarkdownTests.Parser.Rules.BoolRules;

[TestFixture]
[TestOf(typeof(OrRule))]
public class OrRuleTest
{
    private readonly MdTokenizer tokenizer = new();
    private readonly OrRule rule = new(TokenType.Word, TokenType.Number);
    
    [TestCase("abc def")]
    [TestCase("123 abc")]
    public void Match_ShouldMatchOneOfRule(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens) as TextNode;
        
        node.Should().NotBeNull();
    }
    [Test]
    public void Match_ShouldMatchFirstAppearance()
    {
        const string text = "abc123";
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens) as TextNode;
        
        node.Should().NotBeNull();
        node.ToText(tokens).Should().Be("abc");
    }
    [TestCase("_abc def ghi_")]
    [TestCase(" 123")]
    public void Match_ShouldNotMatchWrongPattern(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens) as TextNode;
        
        node.Should().BeNull();
    }
}