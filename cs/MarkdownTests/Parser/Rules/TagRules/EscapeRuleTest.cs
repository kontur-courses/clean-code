using FluentAssertions;
using Markdown.Parser.Rules.TagRules;
using Markdown.Parser.Tools;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tokens;

namespace MarkdownTests.Parser.Rules.TagRules;

[TestFixture]
public class EscapeRuleTest
{
    private readonly MarkdownTokenizer tokenizer = new();
    private readonly EscapeRule rule = new([TokenType.UNDERSCORE, TokenType.HASH_TAG]);

    [TestCase(@"\_", ExpectedResult = "_")]
    [TestCase(@"\#", ExpectedResult = "#")]
    public string? EscapeRule_Match_ShouldEscapeTagsSymbols(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        var match = rule.Match(tokens);
        return match?.ToText(tokens);
    }
    
    [TestCase(@"\321")]
    [TestCase(@"\Text that not escaping")]
    [TestCase(@"\ Text that not escaping")]
    public void EscapeRule_Match_ShouldNotEscapeNonTagSymbols(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        var match = rule.Match(tokens);
        match.Should().BeNull();
    }
}