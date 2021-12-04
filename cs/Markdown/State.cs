using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public enum State
    {
        TagBeginning = 1,
        TagEnding = 2,
        Whitespace = 4,
        Digit = 8,
        Escape = 16,
        Other = 32
    }
}
