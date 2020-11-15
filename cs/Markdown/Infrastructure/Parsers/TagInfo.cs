using Markdown.Infrastructure.Blocks;
using Markdown.Infrastructure.Parsers.Tags;

namespace Markdown.Infrastructure.Parsers
{
    public class TagInfo
    {
        private readonly bool CanClose;
        private readonly bool CanOpen;
        public readonly int Length;
        public readonly int Offset;
        public readonly Tag Tag;

        public TagInfo(int offset, int length, Tag tag, bool canClose = true, bool canOpen = true)
        {
            Offset = offset;
            Length = length;
            Tag = tag;
            CanClose = canClose;
            CanOpen = canOpen;
        }

        public TagInfo(int offset, int length, Style style, bool canClose = true, bool canOpen = true)
            : this(offset, length, new Tag(style), canClose, canOpen)
        {
        }


        public bool Closes(TagInfo toClose, string text)
        {
            return ClosesSameType(toClose, text)
                   || ClosesByNewLine(toClose);
        }

        private bool ClosesSameType(TagInfo toClose, string text = "")
        {
            return CanClose
                   && toClose.CanOpen
                   && IsSameType(toClose)
                   && !IsDifferentWords(toClose, text)
                ;
        }

        public bool IsDifferentWords(TagInfo toClose, string text = "")
        {
            return CanClose
                   && CanOpen
                   && toClose.CanClose
                   && toClose.CanOpen
                   && MarkdownParser.WhiteSpaceCharBetweenTags(ref text, toClose, this);
        }

        private bool IsSameType(TagInfo toClose)
        {
            return Tag.Style == toClose.Tag.Style;
        }

        private bool ClosesByNewLine(TagInfo toClose)
        {
            return Style.Header == toClose.Tag.Style && Tag.Style == Style.NewLine;
        }

        public bool Follows(TagInfo previous)
        {
            return previous.Offset + previous.Length == Offset;
        }
    }
}