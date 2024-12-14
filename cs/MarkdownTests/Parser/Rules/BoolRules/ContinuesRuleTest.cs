using FluentAssertions;
using Markdown.Parser.Rules;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokenizer;
using Markdown.Tokens;

namespace MarkdownTests.Parser.Rules.BoolRules;

[TestFixture]
[TestOf(typeof(ContinuesRule))]
public class ContinuesRuleTest
{
    private readonly MdTokenizer tokenizer = new();
    [Test]
    public void Match_ShouldMatchWhenRightContinues()
    {
        var tokens = tokenizer.Tokenize("abc def ghi_");
        var rule = new ContinuesRule(new TextRule(), new PatternRule(TokenType.Underscore));
        
        var match = rule.Match(tokens);
        match.Should().NotBeNull();
        match.ToText(tokens).Should().Be("abc def ghi");
    }
    [Test]
    public void Match_ShouldNotMatchWhenWrongContinues()
    {
        var tokens = tokenizer.Tokenize("abc def ghi_");
        var rule = new ContinuesRule(new TextRule(), new PatternRule(TokenType.Octothorpe));
        
        var match = rule.Match(tokens);
        match.Should().BeNull();
    }
}