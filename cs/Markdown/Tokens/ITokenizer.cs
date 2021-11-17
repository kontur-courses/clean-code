using System.Collections.Generic;

namespace Markdown.Tokens
{
    public interface ITokenizer
    {
        public IEnumerable<Token> Tokenize(string text);
    }
}