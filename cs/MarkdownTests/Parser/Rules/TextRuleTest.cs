using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules;
using Markdown.Tokenizer;

namespace MarkdownTests.Parser.Rules;

[TestFixture]
[TestOf(typeof(TextRule))]
public class TextRuleTest
{
    private readonly TextRule rule = new();
    private readonly MdTokenizer tokenizer = new();

    [Test]
    public void Match_ShouldMatch_SimpleText()
    {
        const string text = "abc";
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens) as TextNode;
        
        node.Should().NotBeNull();
        node.Consumed.Should().Be(1);
        node.ToText(tokens).Should().Be(text);
    }

    [TestCase("_")]
    [TestCase("\n")]
    public void Match_ShouldReturnNull_WhenNotText(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens) as TextNode;
        
        node.Should().BeNull();
    }

    [Test]
    public void Match_ShouldMatchSequenceOfWordsAndSpaces()
    {
        const string text = "abc def ghi";
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens) as TextNode;
        
        node.Should().NotBeNull();
        node.ToText(tokens).Should().BeEquivalentTo(text);
    }

    [TestCase("abc _def_", ExpectedResult = "abc ")]
    [TestCase("abc \ndef", ExpectedResult = "abc ")]
    public string? Match_ShouldBeInterrupted_ByNonSpaceOrWordType(string text, int begin = 0)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens, begin) as TextNode;

        return node?.ToText(tokens);
    }
}