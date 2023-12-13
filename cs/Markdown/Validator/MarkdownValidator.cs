using Markdown.Tokens;

namespace Markdown.Validator;

public class MarkdownValidator : ITokenValidator
{
    public List<Token> RemoveInvalidTokens(List<Token> tokens)
    {
        return tokens;
    }
}