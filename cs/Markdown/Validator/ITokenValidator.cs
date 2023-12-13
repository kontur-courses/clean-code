using Markdown.Tokens;

namespace Markdown.Validator;

public interface ITokenValidator
{
    public List<Token> RemoveInvalidTokens(List<Token> tokens);
}