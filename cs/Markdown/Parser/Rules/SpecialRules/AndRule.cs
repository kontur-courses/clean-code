using Markdown.Parser.Nodes;
using Markdown.Parser.Rules.TextRules;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser.Rules.SpecialRules;

public class AndRule(List<IParsingRule> pattern) : IParsingRule
{
    public AndRule(IParsingRule firstRule, IParsingRule secondRule)
        : this([firstRule, secondRule]) 
    { }
    
    public AndRule(TokenType firstType, TokenType secondType)
        : this(new PatternRule(firstType), new PatternRule(secondType)) 
    { }
    
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        var consumed = 0;
        var nodes = new List<Node>();

        foreach (var rule in pattern)
        {
            var newBegin = begin + consumed;
            if (rule.Match(tokens, newBegin) is not { } node) return null;
            
            nodes.Add(node); 
            consumed += node.Consumed; 
        }
        return new SpecNode(nodes, begin, consumed);
    }
}