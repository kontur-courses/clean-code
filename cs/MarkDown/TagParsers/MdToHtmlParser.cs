using System;
using System.Collections.Generic;
using System.Text;

namespace MarkDown.TagParsers
{
    internal class MdToHtmlParser
    {
        private readonly ParserGetter tagParsers;

        public MdToHtmlParser(ParserGetter tagParsers)
        {
            this.tagParsers = tagParsers;
        }

        public StringBuilder GetHtml(string line, Queue<Tag> sequence, Queue<Tag> ignoredTags,
            Queue<int> escapeInd)
        {
            if (sequence == null || ignoredTags == null || escapeInd == null)
                throw new ArgumentException("one of arguments was null");

            var stb = new StringBuilder();
            var indexNextToToken = 0;

            for (var i = 0; i < line.Length; i++)
            {
                if (ignoredTags.Count > 0 && ignoredTags.Peek().StartIndex == i)
                {
                    sequence.Dequeue();
                    stb.Append(GetParsedSubstring(line, ignoredTags.Dequeue(), out indexNextToToken));
                }
                else if (sequence.Count > 0 && sequence.Peek().StartIndex == i)
                {
                    stb.Append(GetParsedSubstring(line, sequence.Dequeue(), false, out indexNextToToken));
                }
                else if (escapeInd.Count > 0 && escapeInd.Peek() == i)
                {
                    var escapedSymbol = FindParsedSubstring(line, escapeInd.Dequeue(), out indexNextToToken);
                    stb.Append(escapedSymbol);
                }
                else
                {
                    stb.Append(line[i]);
                }

                if (indexNextToToken > i)
                    i = indexNextToToken - 1;
            }

            return stb;
        }

        public string GetParsedSubstring(string line, Tag sequence, bool isIgnored, out int indexNextToTag)
        {
            var parser = tagParsers.GetParserFromType(sequence.Type);
            var tagValue = sequence.IsOpening ? parser.OpeningTags.html : parser.ClosingTags.html;
            indexNextToTag = sequence.IndexNextToTag;
            return tagValue;
        }

        public string GetParsedSubstring(string line, Tag sequence, out int indexNextToTag)
        {
            indexNextToTag = sequence.IndexNextToTag;
            var parser = tagParsers.GetParserFromType(sequence.Type);
            var tagValue = sequence.IsOpening ? parser.OpeningTags.md : parser.ClosingTags.md;
            return tagValue;
        }

        public char FindParsedSubstring(string line, int index, out int indexNextToEscapedSymbol)
        {
            indexNextToEscapedSymbol = index + 2;
            return index + 1 < line.Length ? line[index + 1] : '\\';
        }
    }
}