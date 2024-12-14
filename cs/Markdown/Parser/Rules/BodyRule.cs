using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class BodyRule : IParsingRule
{
    private static readonly IParsingRule TagRules = new OrRule([
        new EscapeRule([TokenType.Octothorpe, TokenType.Asterisk]),
        new HeaderRule(),
        new UnorderedListRule(),
        new ParagraphRule()
    ]);
    
    private static readonly IParsingRule TokenRules = new PatternRule(TokenType.Newline);

    private static readonly KleeneStarRule ResultRule = new(new OrRule(TagRules, TokenRules));

    public Node? Match(List<Token> tokens, int begin = 0) 
        => ResultRule.Match(tokens, begin) is SpecNode node ? BuildNode(node) : null;

    private static TagNode BuildNode(SpecNode node)
        => new(NodeType.Body, node.Nodes, node.Start, node.Consumed);
}