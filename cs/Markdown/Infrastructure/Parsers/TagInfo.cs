using Markdown.Infrastructure.Blocks;

namespace Markdown.Infrastructure.Parsers
{
    public class TagInfo
    {
        public readonly int Offset;
        public readonly int Length;
        public readonly Style Style;
        public readonly bool IsClosing;

        public TagInfo(int offset, int length, Style style, bool isClosing = true)
        {
            Offset = offset;
            Length = length;
            Style = style;
            IsClosing = isClosing;
        }
        
                
        public bool Closes(TagInfo other)
        {
            return (IsClosing && Style == other.Style)
                || Style == Blocks.Style.Enter;
        }
    }
}