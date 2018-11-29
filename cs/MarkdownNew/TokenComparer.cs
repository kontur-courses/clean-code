using System.Collections.Generic;

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
