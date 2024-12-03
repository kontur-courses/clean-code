using FluentAssertions;
using Markdown;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules;

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
        node.Text.Should().Be(text);
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
        node.Tokens.Should().BeEquivalentTo(tokens);
    }

    [TestCase("abc _def_")]
    [TestCase("abc \ndef")]
    public void Match_ShouldBeInterrupted_ByNonSpaceOrWordType(string text, int begin = 0)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens, begin) as TextNode;
        
        node.Should().NotBeNull();
        node.Tokens.Should().NotBeEquivalentTo(tokens);
        node.Tokens.Should().BeEquivalentTo(tokens
            .Skip(begin)
            .Take(node.Consumed)
            .ToList());
    }
}