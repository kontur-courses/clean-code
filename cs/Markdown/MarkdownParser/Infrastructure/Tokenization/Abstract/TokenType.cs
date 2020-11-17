using System;

namespace MarkdownParser.Infrastructure.Tokenization.Abstract
{
    [Flags]
    public enum TokenType
    {
        Opening = 1,
        Closing = 2,
        Any = Opening | Closing
    }
}