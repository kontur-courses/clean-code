using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Markdown
{
    public class TokenComparer : IComparer<Token>
    {
        public int Compare([AllowNull] Token x, [AllowNull] Token y)
        {
            if (x.TokenStart.CompareTo(y.TokenStart) != 0)
                return x.TokenStart.CompareTo(y.TokenStart);
            if (x.ContentStart.CompareTo(y.ContentStart) != 0)
                return x.ContentStart.CompareTo(y.ContentStart);
            if (y.ContentLength.CompareTo(x.ContentLength) != 0)
                return y.ContentLength.CompareTo(x.ContentLength);
            if (y.TokenLength.CompareTo(x.TokenLength) != 0)
                return y.TokenLength.CompareTo(x.TokenLength);
            if (y.TokenStyle.Type == StyleType.UnorderedListElement
                && x.TokenStyle.Type == StyleType.UnorderedList)
                return -1;
            if (x.TokenStyle.Type == StyleType.UnorderedListElement
                && y.TokenStyle.Type == StyleType.UnorderedList)
                return 1;

            return 0;
        }
    }
}