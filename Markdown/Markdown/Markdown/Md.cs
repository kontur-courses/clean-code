using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    static class Md
    { 

        public static string Render(string MarkdownString)
        {
            
            TokenParser.AddText(MarkdownString);
            return MarkdownString;
        }
    }
}
