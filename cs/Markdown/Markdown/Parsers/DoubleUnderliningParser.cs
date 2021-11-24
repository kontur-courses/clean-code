using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
