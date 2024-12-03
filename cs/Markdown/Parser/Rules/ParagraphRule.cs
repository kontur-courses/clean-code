using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.BoolRules;
using Markdown.Parser.Rules.Tools;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class ParagraphRule: IParsingRule
{
    private static readonly OrRule SentenceValues = new([
        new ItalicRule(), new BoldRule(), new TextRule(), 
        PatternRule.DoubleUnderscoreRule(), 
        new OrRule(TokenType.Number, TokenType.Underscore)
    ]);


    private readonly List<IParsingRule> pattern =
    [
        new KleeneStarRule(SentenceValues),
        new PatternRule(TokenType.Newline)
    ];


    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var match = tokens.MatchPattern(pattern, begin);


        if (match.Count != pattern.Count)
        {
            return null;
        }

        if (match.First() is not SpecNode specNode)
        {
            return null;
        }


        return new TagNode(NodeType.Paragraph, specNode.Children, specNode.Consumed + 1);
    }
}