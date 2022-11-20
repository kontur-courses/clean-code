using System;

namespace Markdown
{
    public class DefaultTokenizer<TTag> : ITokenizer<TTag>
    {
        public IToken<TTag>[] Tokenize()
        {
            throw new NotImplementedException();
        }
    }
}
