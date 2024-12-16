using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.SpecialRules;
using Markdown.Parser.Rules.TextRules;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser.Rules.TagRules;

public class BodyRule : IParsingRule
{
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var tagRules = new OrRule([
            new EscapeRule(TokenType.HASH_TAG),
            new HeadlineRule(), new ParagraphRule(),
        ]);
        var tokenRules = new PatternRule(TokenType.NEW_LINE);
        
        var resultRule = new KleenStarRule(new OrRule(tagRules, tokenRules));
        return resultRule.Match(tokens, begin) is SpecNode node ? BuildNode(node) : null;
    }

    private static TagNode BuildNode(SpecNode node)
        => new(NodeType.BODY, node.Nodes, node.Start, node.Consumed);
}