using System;
using System.Linq;
using Markdown.Parsers;
using Markdown.Tags;

namespace Markdown
{
    public static class MarkdownParser
    {
        public static Tag[] ParseAllTags(string paragraph)
        {
            return HeaderParser.ParseTags(paragraph)
                .Concat(ItalicParser.ParseTags(paragraph))
                .Concat(BoldParser.ParseTags(paragraph))
                .Concat(UnOrderedListParser.ParseTags(paragraph))
                .ToArray();
        }

        public static int GetEndOfTag(string paragraph, int index, Func<string, int, bool> isTagEnd, int tagLength)
        {
            index += tagLength;
            while (index < paragraph.Length)
            {
                if (paragraph[index] == ' ')
                {
                    return -1;
                }
                if (isTagEnd(paragraph, index))
                {
                    return index;
                }

                index++;
            }

            return -1;
        }
    }
}