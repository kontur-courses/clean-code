using FluentAssertions;
using Markdown.Parser.Rules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokenizer;
using Markdown.Tokens;

namespace MarkdownTests.Parser.Rules;

[TestFixture]
[TestOf(typeof(EscapeRule))]
public class EscapeRuleTest
{
    private readonly EscapeRule rule = new([TokenType.Underscore, TokenType.Octothorpe]);
    private readonly MdTokenizer tokenizer = new();
    [TestCase(@"\_", ExpectedResult = "_")]
    [TestCase(@"\#", ExpectedResult = "#")]
    public string? EscapeRule_Match_ShouldEscapeTagsSymbols(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        var match = rule.Match(tokens);
        return match?.ToText(tokens);
    }
    [TestCase(@"\abc def")]
    [TestCase(@"\ abc def")]
    public void EscapeRule_Match_ShouldNotEscapeNonTagSymbols(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        var match = rule.Match(tokens);
        match.Should().BeNull();
    }
}