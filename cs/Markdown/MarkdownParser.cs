using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;
using Markdown.Tags.BoldTag;
using Markdown.Tags.HeaderTag;
using Markdown.Tags.ItalicTag;
using Markdown.Tags.UnorderedListTag;

namespace Markdown
{
    public static class MarkdownParser
    {
        public static bool IsUnOrderedListStart = false;
        public static Tag[] ParseAllTags(string paragraph)
        {
            return ParseHeaderTag(paragraph)
                .Concat(ParseAllItalicTags(paragraph))
                .Concat(ParseAllBoldTags(paragraph))
                .Concat(ParseUnOrderedListTag(paragraph))
                .ToArray();
        }

        public static Tag[] ParseUnOrderedListTag(string paragraph)
        {
            if (paragraph[0] == '*' && paragraph[1] == ' ')
            {
                if (IsUnOrderedListStart)
                {
                    return new Tag[0];
                }
                IsUnOrderedListStart = true;
                return new Tag[]{new OpenUnOrderedListTag(0) };
            }
            if (IsUnOrderedListStart)
            {
                IsUnOrderedListStart = false;
                return new Tag[]{new CloseUnOrderedListTag(0) };
            }

            return new Tag[0];
        }

        public static Tag[] ParseHeaderTag(string paragraph)
        {
            return paragraph[0] == '#' 
                ? new Tag[] {new OpenHeaderTag(0), new CloseHeaderTag(paragraph.Length)} 
                : new Tag[0];
        }
        
        public static Tag[] ParseAllItalicTags(string paragraph)
        {
            var result = new List<Tag>();
            for (var index = 0; index < paragraph.Length; index++)
            {
                if (ItalicTag.IsTagStart(paragraph, index))
                {
                    var endOfTag = GetEndOfItalicTag(paragraph, index);
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
        
        public static Tag[] ParseAllBoldTags(string paragraph)
        {
            var result = new List<Tag>();
            for (var index = 0; index < paragraph.Length; index++)
            {
                if (BoldTag.IsTagStart(paragraph, index))
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

        private static int GetEndOfTag(string paragraph, int index, Func<string, int, bool> isTagEnd, int length)
        {
            index += length;
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

        private static int GetEndOfBoldTag(string paragraph, int index)
        {
            return GetEndOfTag(paragraph, index, BoldTag.IsTagEnd, BoldTag.TagLength);
        }

        private static int GetEndOfItalicTag(string paragraph, int index)
        {
            return GetEndOfTag(paragraph, index, ItalicTag.IsTagEnd, ItalicTag.TagLength);
        }
    }
}