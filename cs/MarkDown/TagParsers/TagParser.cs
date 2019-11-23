using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkDown.TagParsers
{
    public abstract class TagParser
    {
        public abstract Stack<TagType> EscapingTokens { get; }
        public abstract TagType Type { get; }
        public abstract (string md, string html) OpeningTags { get; }
        public abstract (string md, string html) ClosingTags { get; }


        public bool TagIgnorable(IEnumerable<TagType> currentTokens)
        {
            return currentTokens.Any(t => EscapingTokens.Contains(t));
        }

        public bool TagIsClosing(string line, int startIndex)
        {
            return startIndex + ClosingTags.md.Length <= line.Length
                   && line.Substring(startIndex, ClosingTags.md.Length) == ClosingTags.md
                   && TagIsValid(line, startIndex, false);
        }

        public bool TagIsOpening(string line, int startIndex)
        {
            return startIndex + OpeningTags.md.Length <= line.Length
                   && line.Substring(startIndex, OpeningTags.md.Length) == OpeningTags.md
                   && TagIsValid(line, startIndex, true);
        }

        private bool TagIsValid(string line, int startIndex, bool tagOpened)
        {
            if (startIndex < 0 || startIndex > line.Length) throw new ArgumentException("start index is out of range");

            var leftIndex = startIndex - 1;
            var rightIndex = startIndex + (tagOpened ? OpeningTags.md.Length : ClosingTags.md.Length);

            if (tagOpened)
                return (leftIndex < 0 || char.IsWhiteSpace(line[leftIndex]))
                       && (line.Length <= rightIndex ||
                           !char.IsWhiteSpace(line[rightIndex]) &&
                           line[rightIndex] != '_');

            return (line.Length <= rightIndex || char.IsWhiteSpace(line[rightIndex]))
                   && (leftIndex < 0 ||
                       !char.IsWhiteSpace(line[leftIndex]) &&
                       line[leftIndex] != '_');
        }
    }
}