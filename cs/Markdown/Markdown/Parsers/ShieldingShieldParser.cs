using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parsers
{
    public class ShieldingShieldParser : IParser
    {
        public IToken TryGetToken(ref int i, ref StringBuilder currentBuilder, string line)
        {
            currentBuilder.Append(line[i + 1]);
            i++;
            return new TokenWord(null);
        }

        public IToken TryGetToken()
        {
            throw new NotImplementedException();
        }
    }
}
