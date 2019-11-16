using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Token
    {
        public char Value;
        public TokenType Type;
        public MdElement MdType;
        public MdPosition MdPosition;
        public bool IsClosed;

        public Token(char value, TokenType type)
        {
            Type = type;
            Value = value;
            MdPosition = MdPosition.None;
            IsClosed = false;
        }
    }
}
