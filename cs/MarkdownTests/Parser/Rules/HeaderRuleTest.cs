using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules;
using Markdown.Tokenizer;

namespace MarkdownTests.Parser.Rules;

[TestFixture]
[TestOf(typeof(HeaderRule))]
public class HeaderRuleTest
{
    private readonly MdTokenizer tokenizer = new();
    private readonly HeaderRule rule = new();

    [TestCase("abc")]
    [TestCase("abc def ghi")]
    public void Match_ShouldMatch_SimpleHeader(string text)
    {
        var tokens = tokenizer.Tokenize($"# {text}\n");

        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
    }

    [TestCase("_abc_")]
    [TestCase("__abc__")]
    [TestCase("abc _def_")]
    [TestCase("abc __def__")]
    [TestCase("abc _def_ ghi")]
    [TestCase("abc __def__ ghi")]
    [TestCase("abc __d_e_f__ ghi")]
    public void Match_ShouldMatch_HeaderWithInnerTags(string text)
    {
        var tokens = tokenizer.Tokenize($"# {text}\n");
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
    }

    [Test]
    public void Match_ShouldNotMatch_IfNoSpaceAfterOctothorpe()
    {
        const string text = "#abc\n";
        
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().BeNull();
    }
    
    [Test]
    public void Match_ShouldNotMatch_IfNoEndOfLine()
    {
        const string text = "# abc";
        
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().BeNull();
    }
    
}