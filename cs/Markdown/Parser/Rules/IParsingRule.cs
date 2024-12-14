using Markdown.Parser.Nodes;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public interface IParsingRule
{
    public Node? Match(List<Token> tokens, int begin);
}