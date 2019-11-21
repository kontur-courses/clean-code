using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class EncloserToken : Token
    {
        public int Start;
        public int End;

        public EncloserToken(string value, TokenType type, MdPosition mdPosition, int start, int end) : base(value, type)
        {
            Start = start;
            End = end;
            MdPosition = mdPosition;
        }
    }
}
