using Markdown.Parser.Nodes;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser.Rules;

public interface IParsingRule
{
    public Node? Match(List<Token> tokens, int begin = 0);
}