using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Tokenizers
{
    public interface ITokenizer
    {
        public IEnumerable<Token> Tokenize(IEnumerable<string> lexemes);
    }
}