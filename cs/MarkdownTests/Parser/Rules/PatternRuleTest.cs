using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules;
using Markdown.Tokenizer;
using Markdown.Tokens;

namespace MarkdownTests.Parser.Rules;

[TestFixture]
[TestOf(typeof(PatternRule))]
public class PatternRuleTest
{
    private readonly MdTokenizer tokenizer = new();

    [Test]
    public void Match_ShouldMatchSinglePattern()
    {
        var tokens = tokenizer.Tokenize("_");
        var rule = new PatternRule([TokenType.Underscore]);

        var node = rule.Match(tokens) as TextNode;
        
        node.Should().NotBeNull();
        node.ToText(tokens).Should().BeEquivalentTo("_");
    }

    [Test]
    public void Match_ContinuesPattern()
    {
        var tokens = tokenizer.Tokenize("_\n ");
        var rule = new PatternRule([
            TokenType.Underscore, TokenType.Newline, TokenType.Space]);
        
        var node = rule.Match(tokens) as TextNode;
        
        node.Should().NotBeNull();
        node.ToText(tokens).Should().BeEquivalentTo("_\n ");
    }
}