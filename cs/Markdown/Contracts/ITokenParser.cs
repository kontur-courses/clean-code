using Markdown.Tokens;

namespace Markdown.Contracts;

public interface ITokenParser
{
    string Parse(Token token);
}