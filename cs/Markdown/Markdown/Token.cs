using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Token
    {
        public readonly string Text;
        public readonly Mark Mark;

        public Token(string text, Mark mark)
        {
            Text = text;
            Mark = mark;
        }
    }
}
