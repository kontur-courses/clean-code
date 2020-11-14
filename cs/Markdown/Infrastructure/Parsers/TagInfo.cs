using System;
using Markdown.Infrastructure.Blocks;

namespace Markdown.Infrastructure.Parsers
{
    public class TagInfo
    {
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
            return CanClose
                   && CanOpen
                   && toClose.CanClose
                   && toClose.CanOpen
                   && MarkdownParser.WhiteSpaceCharBetweenTags(ref text, toClose, this);
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