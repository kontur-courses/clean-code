using System;

namespace Markdown
{
    [Flags]
    public enum TokenType
    {
        Undefined = 0,
        Space = 1,
        Word = 2,
        SymbolSet = 4,
        BreakLine = 8,
        Tag = 16,
        Number = 32
    }
}
