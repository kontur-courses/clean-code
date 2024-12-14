using System.Collections;
using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokenizer;

namespace MarkdownTests.Parser.Rules.BoolRules;

[TestFixture]
[TestOf(typeof(ConditionalRule))]
public class ConditionalRuleTest
{
    private readonly TextRule primaryRule = new();
    private readonly MdTokenizer tokenizer = new();
    private static IEnumerable CasesThatMatchesPrimaryRule
    {
        get
        {
            yield return new TestCaseData("abc def");
            yield return new TestCaseData(" abc def ghi ");
        }
    }
    [Test, TestCaseSource(nameof(CasesThatMatchesPrimaryRule))]
    public void ConditionalRule_Match_ShouldMatchNodeWithRightCondition(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        var rule = new ConditionalRule(primaryRule, (node, _) => node.NodeType == NodeType.Text);
        
        var match = rule.Match(tokens);
        match.Should().NotBeNull();
        match.ToText(tokens).Should().Be(text);
        match.NodeType.Should().Be(NodeType.Text);
    }
    
    [Test, TestCaseSource(nameof(CasesThatMatchesPrimaryRule))]
    public void ConditionalRule_Match_ShouldNotMatchWithWrongCondition(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        var rule = new ConditionalRule(primaryRule, (_,_) => false);
        
        var match = rule.Match(tokens);
        match.Should().BeNull();
    }
}