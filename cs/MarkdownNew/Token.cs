using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownNew
{
    class Token
    {
        public int Start { get; private set; }
        public Tag  StartTag { get; private set; }
        public int Lenght { get; private set; }
        public int End => Start + Lenght - 1;
        public Token(int start, Tag startTag)
        {
            this.Start = start;
            this.StartTag = startTag;
        }

        public void SetEndOfToken(int position)
        {
            Lenght = position - Start + 1;
        }
    }
}
