using System;

namespace MarkdownParser.Infrastructure.Tokenization.Models
{
    [Flags]
    public enum TokenPosition
    {
        BeforeWord = 1,
        AfterWord = 2,
        BeforeDigit = 4,
        AfterDigit = 8,
        ParagraphStart = 16,
        ParagraphEnd = 32,
        BeforeWhitespace = 64,
        AfterWhitespace = 128
    }
}