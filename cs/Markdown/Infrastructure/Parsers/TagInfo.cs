using System;
using Markdown.Infrastructure.Blocks;

namespace Markdown.Infrastructure.Parsers
{
    public class TagInfo
    {
        protected bool Equals(TagInfo other)
        {
            return Offset == other.Offset && Length == other.Length && Style == other.Style;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TagInfo) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Offset, Length, (int) Style);
        }

        public readonly int Offset;
        public readonly int Length;
        public readonly Style Style;
        private readonly bool CanClose;
        private readonly bool CanOpen;

        public TagInfo(int offset, int length, Style style, bool canClose = true, bool canOpen = true)
        {
            Offset = offset;
            Length = length;
            Style = style;
            CanClose = canClose;
            CanOpen = canOpen;
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
            return 
                CanClose
                   && CanOpen
                   && toClose.CanClose
                   && toClose.CanOpen
                   &&
                   MarkdownParser.WhiteSpaceCharBetweenTags(ref text, toClose, this);
        }


        private bool IsSameType(TagInfo toClose)
        {
            return Style == toClose.Style;
        }

        private bool ClosesByNewLine(TagInfo toClose)
        {
            return Style.Header == toClose.Style && Style == Style.NewLine;
        }

        public bool Follows(TagInfo previous)
        {
            return previous.Offset + previous.Length == Offset;
        }
    }
}