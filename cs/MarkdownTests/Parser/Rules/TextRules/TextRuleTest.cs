using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.TextRules;
using Markdown.Tokenizer;

namespace MarkdownTests.Parser.Rules.TextRules;

[TestFixture]
public class TextRuleTest
{
    private readonly TextRule rule = new();
    private readonly MarkdownTokenizer tokenizer = new();
    
    [TestCase("Word")]
    [TestCase("Something")]
    [TestCase("BigButSolidString")]
    public void TextRule_Match_SimpleMatch(string markdown)
    {
        var tokens = tokenizer.Tokenize(markdown);

        var node = rule.Match(tokens) as TextNode;

        node.Should().NotBeNull();
        node.Consumed.Should().Be(1);
        node.ToText(tokens).Should().Be(markdown);
    }

    [TestCase("_")]
    [TestCase("\n")]
    public void TextRule_Match_NoTextShouldReturnNull(string markdown)
    {
        var tokens = tokenizer.Tokenize(markdown);
        var node = rule.Match(tokens) as TextNode;
        node.Should().BeNull();
    }

    [TestCase("Hello world!")]
    [TestCase("I am glad to see u")]
    [TestCase("    Some space from both sides    ")]
    public void TextRule_Match_SequenceOfWordAndSpaceShouldBeText(string markdown)
    {
        var tokens = tokenizer.Tokenize(markdown);
        
        var node = rule.Match(tokens) as TextNode;
        
        node.Should().NotBeNull();
        node.ToText(tokens).Should().BeEquivalentTo(markdown);
    }

    [TestCase("Hello _world_", ExpectedResult = "Hello ")]
    [TestCase("_No text at start_ but some in the end", 9, ExpectedResult = " but some in the end")]
    public string? TextRule_Match_SequenceShouldBeInterruptedWithNoSpaceOrWordType(string markdown, int begin = 0)
    {
        var tokens = tokenizer.Tokenize(markdown);
        var node = rule.Match(tokens, begin) as TextNode;
        return node?.ToText(tokens);
    }
}