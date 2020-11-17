using System.Collections.Generic;
using Markdown.Tags;
using Markdown.Tags.BoldTag;

namespace Markdown.Parsers
{
    public static class BoldParser
    {
        public static Tag[] ParseTags(string paragraph)
        {
            var result = new List<Tag>();
            for (var index = 0; index < paragraph.Length; index++)
            {
                if (IsTagStart(paragraph, index))
                {
                    var endOfTag = GetEndOfBoldTag(paragraph, index);
                    if (endOfTag == -1)
                    {
                        return result.ToArray();
                    }
                    result.Add(new OpenBoldTag(index));
                    result.Add(new CloseBoldTag(endOfTag));
                    index = endOfTag;
                }
            }

            return result.ToArray();
        }
        
        private static int GetEndOfBoldTag(string paragraph, int index)
        {
            return MarkdownParser.GetEndOfTag(paragraph, index, IsTagEnd, BoldTag.TagLength);
        }
        
        private static bool IsTagStart(string paragraph, int startIndex)
        {
            return paragraph[startIndex] == '_'
                   && startIndex <= paragraph.Length - 3
                   && paragraph[startIndex + 1] == '_'
                   && !char.IsDigit(paragraph[startIndex + 2])
                   && (paragraph[startIndex + 2] != ' ')
                   && (startIndex == 0 || paragraph[startIndex - 1] != '\\');
        }

        private static bool IsTagEnd(string paragraph, int endIndex)
        {
            return paragraph[endIndex] == '_'
                   && endIndex < paragraph.Length - 1
                   && paragraph[endIndex + 1] == '_'
                   && paragraph[endIndex - 1] != ' '
                   && paragraph[endIndex - 1 ] != '\\';
        }
    }
}