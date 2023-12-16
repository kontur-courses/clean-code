using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownTask
{
    public class Token
    {
        public string HtmlTag;
        public string MDTag;
        public int start, length;

        public Token(string html, string mdTag, int start, int length)
        {

        }
    }
}
