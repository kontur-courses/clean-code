using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public enum Sign
    {
        Whitespace = 32,
        TagSign = 64,
        Escape = 128,
        Digit = 256,
        Other = 512
    }
}
