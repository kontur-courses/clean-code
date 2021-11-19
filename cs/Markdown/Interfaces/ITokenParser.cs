using System.Collections.Generic;

namespace Markdown.Interfaces
{
    public interface ITokenParser
    {
        IEnumerable<TokenNode> Parse(IEnumerable<Token> tokens);
    }
}