using FluentAssertions;
using Markdown.Parser.Rules.SpecialRules;
using Markdown.Parser.Rules.TextRules;
using Markdown.Parser.Tools;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tokens;

namespace MarkdownTests.Parser.Rules.SpecialRules;

[TestFixture]
public class ContinuesRuleTest
{
    private readonly MarkdownTokenizer tokenizer = new();

    [Test]
    public void ContinuesRule_Match_ShouldMatchWhenRightContinues()
    {
        var tokens = tokenizer.Tokenize("Text with concrete continues_");
        var rule = new ContinuesRule(new TextRule(), new PatternRule(TokenType.UNDERSCORE));
        
        var node = rule.Match(tokens);

        node.Should().NotBeNull();
        node.ToText(tokens).Should().Be("Text with concrete continues");
    }

    [Test]
    public void ContinuesRule_Match_ShouldNotMatchWhenWrongContinues()
    {
        var tokens = tokenizer.Tokenize("Text with concrete continues_");
        var rule = new ContinuesRule(new TextRule(), new PatternRule(TokenType.HASH_TAG));
        
        var node = rule.Match(tokens);

        node.Should().BeNull();
    }
}