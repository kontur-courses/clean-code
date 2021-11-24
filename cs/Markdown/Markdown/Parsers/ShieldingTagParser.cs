using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parsers
{
    public class ShieldingTagParser : IParser
    {
        private readonly HashSet<char> _specialSymbols = new HashSet<char> { '_', '#', '[', ']' };

        public IToken TryGetToken(ref int i, ref StringBuilder currentBuilder, string line)
        {
            if (_specialSymbols.Contains(line[i + 1]) ||
                line[i + 1] == '\\')
            {
                currentBuilder.Append(line[i + 1]);
                i++;
            }
            else
            {
                currentBuilder.Append(line[i]);
            }
                
            return new TokenWord(null);
        }

        public IToken TryGetToken()
        {
            throw new NotImplementedException();
        }
    }
}
