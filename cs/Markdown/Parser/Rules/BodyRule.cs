using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class BodyRule : IParsingRule
{
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var tagRules = new OrRule([
            new EscapeRule([TokenType.Octothorpe, TokenType.Asterisk]),
            new HeaderRule(),
            new UnorderedListRule(),
            new ParagraphRule()
        ]);
        var tokenRules = new PatternRule(TokenType.Newline);
        
        var resultRule = new KleeneStarRule(new OrRule(tagRules, tokenRules));
        return resultRule.Match(tokens, begin) is SpecNode node ? BuildNode(node) : null;
    }
    private static TagNode BuildNode(SpecNode node)
        => new(NodeType.Body, node.Nodes, node.Start, node.Consumed);
}