using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownNew
{
    class TokenComparer : IComparer<Token>
    {
        public int Compare(Token x, Token y)
        {
            return y.End.CompareTo(x.End);
        }
    }
}
