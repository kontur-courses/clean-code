using System;

namespace Markdown
{
    public enum TokenType
    {
        Undefined = 1 << 0,
        Space = 1 << 1,
        Word = 1 << 2,
        BreakLine = 1 << 3,
        Number = 1 << 4,
        OtherSymbol = 1 << 5,
        Tag = 1 << 6,
    }
}