using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.TagRules;
using Markdown.Parser.Tools;
using Markdown.Tokenizer;

namespace MarkdownTests.Parser.Rules.TagRules;

[TestFixture]
public class InWordBoldRuleTest
{
    private readonly InWordBoldRule rule = new();
    private readonly MarkdownTokenizer tokenizer = new();
    
    [TestCase("Bold tag in wo__rd__ end", 7, ExpectedResult = "rd")]
    [TestCase("Bold tag in __wo__rd begin", 6, ExpectedResult = "wo")]
    [TestCase("Bold tag in w__or__d center", 7, ExpectedResult = "or")]
    public string InWordBoldRule_Match_ShouldMatchTagInWord(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens, begin) as TagNode;
        
        node.Should().NotBeNull();
        node.Children.Should().ContainSingle(n => n.NodeType == NodeType.TEXT);
        return node.Children.ToText(tokens);
    }
    
    [TestCase("Italic tag tha__t in different wor__ds", 5)]
    [TestCase("Italic tag t__hat in different words__", 5)]
    [TestCase("Italic tag that__ in different wo__rds", 5)]
    public void InWordItalicRule_Match_ShouldNotMatchTagInDifferentWords(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        var node = rule.Match(tokens, begin) as TagNode;
        node.Should().BeNull();
    }
}