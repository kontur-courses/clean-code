using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Parsers
{
    interface IMorphemeParser
    {
        bool CanHaveInnerMorphemes { get; }
        IEnumerable<string> GetMorphemes(string inputString);
        string OpenTag { get; }
        string CloseTag { get; }
    }
}
