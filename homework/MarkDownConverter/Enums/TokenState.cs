using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter.Enums
{
    public enum TokenState
    {
        Space,
        NewLine,
        Numbers,
        Symbols,
        Italic,
        Bold,
        Heading,
    }
}
