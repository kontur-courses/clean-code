using System.Collections.Generic;
using Markdown.Tags;
using Markdown.Tags.ItalicTag;

namespace Markdown.Parsers
{
    public static class ItalicParser
    {
        public static Tag[] ParseTags(string paragraph)
        {
            var result = new List<Tag>();
            for (var index = 0; index < paragraph.Length; index++)
            {
                if (IsTagStart(paragraph, index))
                {
                    var endOfTag = GetEndOfTag(paragraph, index);
                    if (endOfTag == -1)
                    {
                        return result.ToArray();
                    }
                    result.Add(new OpenItalicTag(index));
                    result.Add(new CloseItalicTag(endOfTag));
                    index = endOfTag;
                }
            }

            return result.ToArray();
        }
        
        private static int GetEndOfTag(string paragraph, int index)
        {
            return MarkdownParser.GetEndOfTag(paragraph, index, IsTagEnd, ItalicTag.TagLength);
        }
        
        private static bool IsTagStart(string paragraph, int startIndex)
        {
            return (paragraph[startIndex] == '_'
                    && startIndex < paragraph.Length - 1
                    && !char.IsDigit(paragraph[startIndex + 1])
                    && paragraph[startIndex + 1] != ' '
                    && (startIndex == 0 || paragraph[startIndex - 1] != '_' 
                        && paragraph[startIndex - 1] != '\\')
                    && paragraph[startIndex + 1] != '_')
                   || (paragraph[startIndex] == '_' 
                       && startIndex >= 2 
                       && paragraph[startIndex - 1] == '_' 
                       && paragraph[startIndex - 2] == '\\');
        }
        
        private static bool IsTagEnd(string paragraph, int index)
        {
            return (paragraph[index] == '_'
                    && (index == paragraph.Length - 1 || paragraph[index + 1] != '_')
                    && paragraph[index - 1] != ' '
                    && paragraph[index - 1] != '_'
                    && paragraph[index - 1] != '\\')
                   || (paragraph[index] == '_' 
                       && index >= 2 
                       && paragraph[index - 1] == '_' 
                       && paragraph[index - 2] == '\\');
        }
    }
}