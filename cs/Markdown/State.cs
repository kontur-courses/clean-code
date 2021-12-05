using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public enum State
    {
        UnderlineBeginnig,
        UnderlineEnding,
        Whitespace,
        Digit,
        Escape,
        Other
    }
}
