using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    internal class Token
    {
        public readonly int OpenIndex;
        public readonly int CloseIndex;
        public readonly TokenType TokenType;
        public readonly string Content;
        public bool IsOpened => CloseIndex == 0;

        public Token(int openIndex, TokenType tokenType)
        {
            OpenIndex = openIndex;
            TokenType = tokenType;
        }
    }
}
