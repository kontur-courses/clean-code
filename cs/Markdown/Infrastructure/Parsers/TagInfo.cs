using Markdown.Infrastructure.Blocks;
using Markdown.Infrastructure.Parsers.Tags;

namespace Markdown.Infrastructure.Parsers
{
    public class TagInfo
    {
        private readonly bool canClose;
        private readonly bool canOpen;
        public readonly int Length;
        public readonly int Offset;
        public readonly Tag Tag;

        public TagInfo(int offset, int length, Tag tag, bool canClose = true, bool canOpen = true)
        {
            Offset = offset;
            Length = length;
            Tag = tag;
            this.canClose = canClose;
            this.canOpen = canOpen;
        }

        public TagInfo(int offset, int length, Style style, bool canClose = true, bool canOpen = true)
            : this(offset, length, new Tag(style), canClose, canOpen)
        {
        }


        public bool Closes(TagInfo toClose, ITextHelper textHelper) =>
            ClosesSameType(toClose, textHelper)
            || ClosesByNewLine(toClose);

        private bool ClosesSameType(TagInfo toClose, ITextHelper textHelper) =>
            canClose
            && toClose.canOpen
            && IsSameType(toClose)
            && !IsDifferentWords(toClose, textHelper);

        private bool IsDifferentWords(TagInfo toClose, ITextHelper textHelper) =>
            canClose
            && canOpen
            && toClose.canClose
            && toClose.canOpen
            && textHelper.WhiteSpaceCharBetweenTags(toClose, this);

        private bool IsSameType(TagInfo toClose) => 
            Tag.Style == toClose.Tag.Style;

        private bool ClosesByNewLine(TagInfo toClose) => 
            Style.Header == toClose.Tag.Style && Tag.Style == Style.NewLine;

        public bool Follows(TagInfo previous) => 
            previous.Offset + previous.Length == Offset;
    }
}