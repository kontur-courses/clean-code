using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public enum TokenType
    {
        Text,
        WhiteSpace,
        Digits,
        Eol,
        Eof,
        Emphasis,
        Strong,
        Escape
    }
}
