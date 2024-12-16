using Markdown.Parser.Nodes;
using Markdown.Parser.Nodes.TagNodes;
using Markdown.Parser.Rules.SpecialRules;
using Markdown.Parser.Rules.TextRules;
using Markdown.Parser.Tools;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser.Rules.TagRules;

public class HeadlineRule : IParsingRule
{
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var hashtagRule = new KleenStarRule(new PatternRule(TokenType.HASH_TAG));
        
        var resultRule = new AndRule([
            new ConditionalRule(hashtagRule, IsPossibleHeader),
            new PatternRule(TokenType.SPACE), 
            new ParagraphRule(),
        ]);
        return resultRule.Match(tokens, begin) is SpecNode node ? BuildNode(node) : null;
    }

    private static HeadlineNode BuildNode(SpecNode specNode)
    {
        var valueNode = (specNode.Nodes.Third() as TagNode)!;
        var (_, _, headlineLevel) = (specNode.Nodes.First() as SpecNode)!;

        return new HeadlineNode(headlineLevel, valueNode.Children, specNode.Start, specNode.Consumed);
    }

    private static bool IsPossibleHeader(Node node, List<Token> tokens) => node.Consumed <= 6;
}