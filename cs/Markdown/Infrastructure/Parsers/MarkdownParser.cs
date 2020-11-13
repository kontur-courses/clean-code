using System;
using System.Collections.Generic;
using Markdown.Infrastructure.Blocks;

namespace Markdown.Infrastructure.Parsers
{
    public class MarkdownParser : BlockParser
    {
        // [gfg](https://asdasd)
        /// <summary>
        /// Validate interaction of tags according to rules
        /// </summary>
        protected override IEnumerable<TagInfo> GetValidTags(IEnumerable<TagInfo> tagPositions)
        {
            // delete escaped 
            // remove intersections
            // remove strong in em 

            return tagPositions;
        }

        /// <summary>
        /// Parse tags according to documented rules
        /// </summary>
        protected override IEnumerable<TagInfo> ParseTags(string text)
        {
            var processed = 0;
            while (processed < text.Length)
            {
                var tagInfo = ParseTag(ref text, processed);
                if (tagInfo != null)
                {
                    processed += tagInfo.Length;
                    yield return tagInfo;
                }
                else
                {
                    processed += 1;
                }
            }
        }

        private TagInfo ParseTag(ref string text, int offset)
        {
            switch (text[offset])
            {
                case '_':
                    return ParseUnderscore(ref text, offset);
                case '\\':
                    return ParseEscapeSymbol(ref text, offset);
                case '#':
                    return ParseHeader(ref text, offset);
                default:
                    return TryParseNewLine(ref text, offset, out var tagInfo) 
                        ? tagInfo 
                        : null;
            }
        }

        private bool NextCharIs(char possibleChar, ref string text, int offset)
        {
            return offset + 1 < text.Length && text[offset + 1] == possibleChar;
        }

        private TagInfo ParseUnderscore(ref string text, int offset)
        {
            return NextCharIs('_', ref text, offset)
                ? new TagInfo(offset, 2, Style.Bold)
                : new TagInfo(offset, 1, Style.Angled);
        }
        
        private TagInfo ParseEscapeSymbol(ref string text, int offset)
        {
            return NextCharIs('\\', ref text, offset)
                ? null
                : new TagInfo(offset, 1, Style.Escape);
        }
        
        private TagInfo ParseHeader(ref string text, int offset)
        {
            return NextCharIs(' ', ref text, offset)
                ? new TagInfo(offset, 2, Style.Header)
                : null;
        }

        private bool TryParseNewLine(ref string text, int offset, out TagInfo tagInfo)
        {
            var newLine = Environment.NewLine;
            var substring = text.Substring(offset, newLine.Length);
            if (substring == newLine)
            {
                tagInfo = new TagInfo(offset, 2, Style.Enter);
                return true;
            }
            tagInfo = null;
            return false;
        }
    }
}