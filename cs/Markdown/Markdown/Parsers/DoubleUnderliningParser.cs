using System;
using Markdown.Tags;

namespace Markdown.Parsers
{
    public class DoubleUnderliningParser : IParser
    {
        public IToken TryGetToken(ref int i)
        {
            i++;
            return new TagBold();
        }

        public IToken TryGetToken()
        {
            throw new NotImplementedException();
        }
    }
}
