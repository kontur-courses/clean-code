using FluentAssertions;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokenizer;
using Markdown.Tokens;

namespace MarkdownTests.Parser.Rules.BoolRules;

[TestFixture]
[TestOf(typeof(KleeneStarRule))]
public class KleeneStarRuleTest
{
    private readonly MdTokenizer tokenizer = new();
    private readonly OrRule primaryRule = new(TokenType.Word, TokenType.Space);


    [TestCase("abc")]
    [TestCase("abc def ghi jkl")]
    public void Match_ShouldReturnManyMatches(string text)
    {
        var rule = new KleeneStarRule(primaryRule);
        var tokens = tokenizer.Tokenize(text);
        
        var match = rule.Match(tokens);
        
        match.Should().NotBeNull();
        match.ToText(tokens).Should().Be(text);
    }
    [TestCase("abc def ghi _ jkl", ExpectedResult = "abc def ghi ")]
    [TestCase("abc def ghi _jkl_ mno", ExpectedResult = "abc def ghi ")]
    public string Match_ShouldStopWhenNotMatched(string text)
    {
        var rule = new KleeneStarRule(primaryRule);
        var tokens = tokenizer.Tokenize(text);
        
        var match = rule.Match(tokens);
        
        match.Should().NotBeNull();
        return match.ToText(tokens);
    } 
}