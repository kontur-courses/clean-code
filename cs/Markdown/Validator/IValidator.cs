using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Validator
{
    public interface IValidator
    {
        List<IToken> ValidateTokens(List<IToken> tokens, string text);
    }
}