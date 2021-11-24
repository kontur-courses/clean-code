using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parsers
{
    public class StartLinkParser : IParser
    {
        public IToken TryGetToken()
        {
            return new TagLink(null);
        }
    }
}
