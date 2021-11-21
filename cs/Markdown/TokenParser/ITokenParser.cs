using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.TokenParser
{
    public interface ITokenParser
    {
        IEnumerable<TokenNode> Parse(IEnumerable<Token> tokens);
    }
}