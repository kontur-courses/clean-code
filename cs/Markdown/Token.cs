using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Token
    {
        public int Start { get; private set; }
        public int Lenght { get; private set; }
        public int End { get => Start + Lenght; }
        public Tag Tag { get; private set; }
        public Token(int start, int lenght, Tag tag)
        {
            this.Start = start;
            this.Lenght = lenght;
            this.Tag = tag;
        }
    }
}
