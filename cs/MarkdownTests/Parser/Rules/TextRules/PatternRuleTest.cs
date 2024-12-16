using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.TextRules;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tokens;

namespace MarkdownTests.Parser.Rules.TextRules;

[TestFixture]
public class PatternRuleTest
{
    private readonly MarkdownTokenizer tokenizer = new();
    
    [Test]
    public void PatternRule_Match_SinglePattern()
    {
        var tokens = tokenizer.Tokenize("_");
        var rule = new PatternRule([TokenType.UNDERSCORE]);
        
        var node = rule.Match(tokens) as TextNode;
        
        node.Should().NotBeNull();
        node.ToText(tokens).Should().BeEquivalentTo("_");
    }

    [Test]
    public void PatternRule_Match_ContinuesPattern()
    {
        var tokens = tokenizer.Tokenize("_\n ");
        var rule = new PatternRule([TokenType.UNDERSCORE, TokenType.NEW_LINE, TokenType.SPACE]);
        
        var node = rule.Match(tokens) as TextNode;
        
        node.Should().NotBeNull();
        node.ToText(tokens).Should().BeEquivalentTo("_\n ");
    }
}