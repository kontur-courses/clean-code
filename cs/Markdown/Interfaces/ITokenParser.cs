using System.Collections.Generic;

namespace Markdown.Interfaces
{
    public interface ITokenParser
    {
        IEnumerable<Token> Parse(IEnumerable<Token> tokens);
    }
}