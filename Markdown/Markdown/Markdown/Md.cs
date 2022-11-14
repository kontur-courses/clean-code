using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    internal class Md
    {
        public Md()
        {
        }

        public string Render(string MarkdownString)
        {
            var tokenList = TokenParser.ParseLine(MarkdownString);
            
            return MarkdownString;
        }
    }
}
