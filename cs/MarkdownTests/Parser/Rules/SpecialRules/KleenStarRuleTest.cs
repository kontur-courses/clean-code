using FluentAssertions;
using Markdown.Parser.Rules.SpecialRules;
using Markdown.Parser.Tools;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tokens;

namespace MarkdownTests.Parser.Rules.SpecialRules;

[TestFixture]
public class KleenStarRuleTest
{
    private readonly MarkdownTokenizer tokenizer = new();
    private readonly OrRule primaryRule = new(TokenType.WORD, TokenType.SPACE);
    
    [TestCase("OneWord")]
    [TestCase("Default text with words and spaces")]
    public void KleenStarRule_Match_ShouldReturnManyMatches(string text)
    {
        var rule = new KleenStarRule(primaryRule);
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens);
        
        node.Should().NotBeNull();
        node.ToText(tokens).Should().Be(text);
    }

    [TestCase("Just a part _ will match", ExpectedResult = "Just a part ")]
    [TestCase("Not all text _will match_ here", ExpectedResult = "Not all text ")]
    public string KleenStarRule_Match_ShouldStopWhenNotMatched(string text)
    {
        var rule = new KleenStarRule(primaryRule);
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens);
        
        node.Should().NotBeNull();
        return node.ToText(tokens);
    } 
}