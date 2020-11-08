using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Token
    {
        public readonly int StartPosition;
        public readonly int EndPosition;

        public Token(int start, int end)
        {
            StartPosition = start;
            EndPosition = end;
        }
    }
}
