using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parsers
{
    public class ShieldingTagParser : IParser
    {
        public IToken TryGetToken(ref StringBuilder currentBuilder, int i, string line)
        {
            currentBuilder.Append(line[i]);
            return new TokenWord(null);

        }

        public IToken TryGetToken()
        {
            throw new NotImplementedException();
        }
    }
}
