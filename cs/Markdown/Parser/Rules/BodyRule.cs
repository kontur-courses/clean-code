using Markdown.Parser.Nodes;
using Markdown.Tokens;

namespace Markdown.Parser.Rules;

public class BodyRule : IParsingRule
{
    public Node? Match(List<Token> tokens, int begin = 0)
    {
        throw new NotImplementedException();
    }
}