using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.TagConverters;

namespace Markdown
{
    internal static class TagProcessor
    {
        internal static IEnumerable<TagInfo> GetCorrectTags(this IEnumerable<TagInfo> tags) =>
            tags
            .GetPairTags()
            .GetCorrectPairTags()
            .GetCorrectInside()
            .GetTagsWithoutEmptyInside();
        private static IEnumerable<TagInfo> GetCorrectInside(this IEnumerable<TagInfo> pairTags)
        {
            var result = new Stack<TagInfo>();
            if (!pairTags.Any())
                return result;
            var NotCorrectInsidePosition = new HashSet<int>();
            TagInfo peekTag;
            TagInfo peekOpenTag;
            var correctOpenTag = new Stack<TagInfo>();
            foreach (var tag in pairTags)
            {
                peekTag = result.Any() ? result.Peek() : null;
                peekOpenTag = correctOpenTag.Any() ? correctOpenTag.Peek() : null;
                if (peekTag == null)
                {
                    result.Push(tag);
                    correctOpenTag.Push(tag);
                    continue;
                }
                if (peekTag.tagConverter.TagName == tag.tagConverter.TagName)
                {
                    result.Pop();
                    if (NotCorrectInsidePosition.Contains(peekTag.Pos))
                        NotCorrectInsidePosition.Add(tag.Pos);
                    else
                        correctOpenTag.Pop();
                    continue;
                }
                if (peekOpenTag != null && !peekOpenTag.tagConverter.CanProcessTag(tag.tagConverter.TagName))
                    NotCorrectInsidePosition.Add(tag.Pos);
                else
                    correctOpenTag.Push(tag);
                result.Push(tag);
            }
            return pairTags.Where(t => t.tagConverter.IsSingleTag || !NotCorrectInsidePosition.Contains(t.Pos));
        }

        private static IEnumerable<TagInfo> GetTagsWithoutEmptyInside(this IEnumerable<TagInfo> pairTags)
        {
            var result = new Stack<TagInfo>();
            var positionsEmptyTag = new HashSet<int>();
            TagInfo peek;
            foreach(var tag in pairTags)
            {
                peek = result.Any() ? result.Peek() : null;
                if(peek == null)
                {
                    result.Push(tag);
                    continue;
                }
                if (tag.tagConverter.TagName == peek.tagConverter.TagName)
                {
                    result.Pop();
                    if (tag.Pos == peek.Pos + peek.tagConverter.TagName.Length)
                    {
                        positionsEmptyTag.Add(tag.Pos);
                        positionsEmptyTag.Add(peek.Pos);
                    }
                }
                else
                    result.Push(tag);
            }
            return pairTags.Where(t => t.tagConverter.IsSingleTag || !positionsEmptyTag.Contains(t.Pos));
        }
        private static IEnumerable<TagInfo> GetCorrectPairTags(this IEnumerable<TagInfo> pairTags)
        {
            var result = new Stack<TagInfo>();
            if (!pairTags.Any())
                return result;
            var OpenTag = TagsAssociation.tags.ToDictionary(t => t, t => false);
            foreach (var tag in pairTags)
            {
                if (OpenTag[tag.tagConverter.TagName])
                {
                    OpenTag[tag.tagConverter.TagName] = false;
                    if (result.Peek().tagConverter.TagName == tag.tagConverter.TagName)
                        result.Pop();
                    else
                        result.Push(tag);
                }
                else 
                {
                    OpenTag[tag.tagConverter.TagName] = true;
                    result.Push(tag);
                }
            }
            var TagsPositionNotCorrectTag = 
                result
                .Where(t => !t.tagConverter.IsSingleTag)
                .Select(t => t.Pos);
            return pairTags.Where(t => !TagsPositionNotCorrectTag.Contains(t.Pos));
        }

        private static IEnumerable<TagInfo> GetPairTags(this IEnumerable<TagInfo> tags)
        {
            var stackTag = new Stack<TagInfo>();
            var tagPair = new List<TagInfo>();
            var tagOpen = TagsAssociation.tags.ToDictionary(t => t, t => false);
            var tagPos = new Dictionary<string, int>();
            foreach (var tag in tags)
            {
                PushTagCorrect(tag);
            }
            var result = new List<TagInfo>();
            foreach (var tag in tagPair)
            {
                if (tag.tagConverter.IsSingleTag || 
                    !tagOpen[tag.tagConverter.TagName] || 
                    tag.Pos != tagPos[tag.tagConverter.TagName])
                    result.Add(tag);
            }
            return result;

            void PushTagCorrect(TagInfo tag)
            {
                if (tagOpen[tag.tagConverter.TagName] && tag.CanClose)
                {
                    tagPair.Add(tag);
                    tagOpen[tag.tagConverter.TagName] = false;
                    return;
                }
                if (!tagOpen[tag.tagConverter.TagName] && tag.CanOpen)
                {
                    tagPos[tag.tagConverter.TagName] = tag.Pos;
                    tagPair.Add(tag);
                    tagOpen[tag.tagConverter.TagName] = true;
                }
            }
        }
    }
}
