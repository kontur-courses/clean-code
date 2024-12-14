using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokenizer;

namespace MarkdownTests.Parser.Rules;

[TestFixture]
[TestOf(typeof(UnorderedListRule))]
public class UnorderedListRuleTest
{
    
    private readonly MdTokenizer tokenizer = new();
    private readonly UnorderedListRule rule = new();
    
    [Test]
    public void Match_ShouldMatchCorrectly_WhenSimpleList()
    {
        var text = 
            """
            * abc def ghi
            * jkl mno pqrs
            * ter uvw xyz
            """;
        var tokens = tokenizer.Tokenize($"{text.Replace("\r", "")}\n");
        
        var node = rule.Match(tokens) as TagNode;
        node.Should().NotBeNull();
        node.ToText(tokens).Should().Be(text
            .Replace("\r\n", "")
            .Replace("* ", ""));
        node.Children.Should().OnlyContain(n => n.NodeType == NodeType.ListItem);
    }
    
    [Test]
    public void Match_ShouldMatchCorrectly_WhenTextInItemsIsWithTags()
    {
        var text = 
            """
            * abc _def_ ghi
            * jkl mno __pqrs__
            * __ter__ uvw xyz
            """;
        var tokens = tokenizer.Tokenize($"{text.Replace("\r", "")}\n");
        
        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.Children.Count.Should().Be(3);
        node.Children.Should().OnlyContain(n => n.NodeType == NodeType.ListItem);
    }

    [TestCase("* abc\n\n* def")]
    [TestCase("* abc\ndef\n* ghi")]
    public void Match_ShouldBeInterruptedByEmptyLineOrParagraph(string text)
    {
        var tokens = tokenizer.Tokenize(text);

        var node = rule.Match(tokens) as TagNode;
        
        node.Should().NotBeNull();
        node.Children.Count.Should().Be(1);
    }
}