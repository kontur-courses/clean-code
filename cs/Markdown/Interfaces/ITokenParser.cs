using System.Collections.Generic;

namespace Markdown.Interfaces
{
    public interface ITokenParser
    {
        Component[] Parse(IEnumerable<Token> tokens);
    }
}