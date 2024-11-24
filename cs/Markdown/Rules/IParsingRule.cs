using Markdown.Tokens;

namespace Markdown.Rules;

public interface IParsingRule
{
    public Node Match(List<Token> tokens);
}