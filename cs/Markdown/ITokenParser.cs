using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public interface ITokenParser
    {
        public Token ParseToken(string text, int position);
    }
}
