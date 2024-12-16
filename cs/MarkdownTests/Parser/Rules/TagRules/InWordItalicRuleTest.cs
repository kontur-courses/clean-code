using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.TagRules;
using Markdown.Parser.Tools;
using Markdown.Tokenizer;

namespace MarkdownTests.Parser.Rules.TagRules;

[TestFixture]
public class InWordItalicRuleTest
{
    private readonly InWordItalicRule rule = new();
    private readonly MarkdownTokenizer tokenizer = new();
    
    [TestCase("Italic tag in wo_rd_ end", 7, ExpectedResult = "rd")]
    [TestCase("Italic tag in _wo_rd begin", 6, ExpectedResult = "wo")]
    [TestCase("Italic tag in w_or_d center", 7, ExpectedResult = "or")]
    public string InWordItalicRule_Match_ShouldMatchTagInWord(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        
        var node = rule.Match(tokens, begin) as TagNode;
        
        node.Should().NotBeNull();
        node.Children.Should().ContainSingle(n => n.NodeType == NodeType.TEXT);
        return node.ToText(tokens);
    }
    
    [TestCase("Italic tag tha_t in different wor_ds", 5)]
    [TestCase("Italic tag t_hat in different words_", 5)]
    [TestCase("Italic tag that_ in different wo_rds", 5)]
    public void InWordItalicRule_Match_ShouldNotMatchTagInDifferentWords(string text, int begin)
    {
        var tokens = tokenizer.Tokenize(text);
        var node = rule.Match(tokens, begin) as TagNode;
        node.Should().BeNull();
    }
}