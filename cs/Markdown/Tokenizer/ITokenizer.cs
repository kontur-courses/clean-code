using System.Collections.Generic;

namespace Markdown
{
    public interface ITokenizer
    {
        public IEnumerable<IToken> Tokenize();
    }
}
