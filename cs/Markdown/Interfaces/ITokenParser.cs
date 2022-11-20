using System.Collections.Generic;

namespace Markdown.Interfaces
{
    public interface ITokenParser
    {
        public IEnumerable<Token> Parse(string data);
    }
}