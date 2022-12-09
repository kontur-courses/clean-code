using System.Collections.Generic;
using Markdown.TokenNamespace;

namespace Markdown.TokenizerNamespace
{
    public interface ITokenizer
    {
        public IEnumerable<IToken> Tokenize();
    }
}
