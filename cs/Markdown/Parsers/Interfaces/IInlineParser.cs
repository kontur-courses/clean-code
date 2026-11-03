using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public interface IInlineParser
    {
        public List<Token> ParseToTokens(string line);
    }
}
