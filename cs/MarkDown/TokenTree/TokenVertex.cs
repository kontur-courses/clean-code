using System;
using System.Collections.Generic;
using System.Text;

namespace MarkDown.TokenTree
{
    class TokenVertex
    {
        public TokenVertex parent;
        public TokenVertex child;
        public TokenVertex value;

        public TokenVertex()
        {

        }
    }
}
