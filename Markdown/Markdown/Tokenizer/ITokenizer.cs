using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Tokenizer
{
    public interface ITokenizer<out T> where T : IToken
    {
        public IEnumerable<T> Tokenize(IEnumerable<string> rawTokens);
    }
}