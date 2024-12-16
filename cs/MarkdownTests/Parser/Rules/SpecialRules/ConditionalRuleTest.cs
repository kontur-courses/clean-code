using System.Collections;
using FluentAssertions;
using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.SpecialRules;
using Markdown.Parser.Rules.TextRules;
using Markdown.Parser.Tools;
using Markdown.Tokenizer;

namespace MarkdownTests.Parser.Rules.SpecialRules;

[TestFixture]
public class ConditionalRuleTest
{
    private readonly TextRule primaryRule = new();
    private readonly MarkdownTokenizer tokenizer = new();

    private static IEnumerable CasesThatMatchesPrimaryRule
    {
        get
        {
            yield return new TestCaseData("Common text");
            yield return new TestCaseData(" Strange text with spaces ");
            yield return new TestCaseData("Bigger common text with a lot of words");
        }
    }

    [Test, TestCaseSource(nameof(CasesThatMatchesPrimaryRule))]
    public void ConditionalRule_Match_ShouldMatchNodeWithRightCondition(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        var rule = new ConditionalRule(primaryRule, (node, _) => node.NodeType == NodeType.TEXT);
        
        var node = rule.Match(tokens);

        node.Should().NotBeNull();
        node.ToText(tokens).Should().Be(text);
        node.NodeType.Should().Be(NodeType.TEXT);
    }
    
    [Test, TestCaseSource(nameof(CasesThatMatchesPrimaryRule))]
    public void ConditionalRule_Match_ShouldNotMatchWithWrongCondition(string text)
    {
        var tokens = tokenizer.Tokenize(text);
        var rule = new ConditionalRule(primaryRule, (_, _) => false);
        
        var node = rule.Match(tokens);

        node.Should().BeNull();
    }
}