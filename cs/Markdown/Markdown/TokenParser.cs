using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class TokenParser
    {
        public readonly char Delimiter;

        public TokenParser(char delimiter)
        {
            Delimiter = delimiter;
        }

        public IEnumerable<Token> Parse(string text)
        {
            throw new NotImplementedException();
        }
    }
}
