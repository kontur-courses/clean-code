using Markdown.Parser.Nodes;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser.Rules.SpecialRules;

public class ContinuesRule(IParsingRule rule, IParsingRule continuesRule) : IParsingRule
{
    public Node? Match(List<Token> tokens, int begin = 0) 
        => new ConditionalRule(rule, HasRightContinues).Match(tokens, begin); 

    private bool HasRightContinues(Node node, List<Token> tokens)
    {
        if (tokens.Count == node.End + 1) return true;
        return continuesRule.Match(tokens, node.End + 1) is not null;
    }
}