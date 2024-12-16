using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.SpecialRules;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tokens;

namespace MarkdownTests.Parser.Rules.SpecialRules;

[TestFixture]
public class OrRuleTest
{
    private readonly MarkdownTokenizer tokenizer = new();
    private readonly OrRule rule = new(TokenType.WORD, TokenType.NUMBER);

    [TestCase("ThisPartMatch but this dont")]
    [TestCase("1234567890 and no match after this")]
    public void OrRule_Match_ShouldMatchOneOfRule(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        var node = rule.Match(tokens) as TextNode;
        node.Should().NotBeNull();
    }

    [Test]
    public void OrRule_Match_ShouldMatchFirstAppearance()
    {
        const string text = "BothMatch12345 but first returned";
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens) as TextNode;
        
        node.Should().NotBeNull();
        node.ToText(tokens).Should().Be("BothMatch");
    }

    [TestCase("_No match att all_")]
    [TestCase(" 123456 Even with number doesnt work")]
    public void OrRule_Match_ShouldNotMatchWrongPattern(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        var node = rule.Match(tokens) as TextNode;
        node.Should().BeNull();
    }
}