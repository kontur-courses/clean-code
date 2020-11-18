using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.TagConverters;

namespace Markdown
{
    internal static class TagProcessor
    {
        internal static List<TagInfo> GetCorrectTags(this List<TagInfo> tags) =>
            tags
            .GetPairTags()
            .GetCorrectPairTags()
            .GetCorrectInside()
            .GetTagsWithoutEmptyInside();
        private static List<TagInfo> GetCorrectInside(this List<TagInfo> pairTags)
        {
            var result = new Stack<TagInfo>();
            if (!pairTags.Any())
                return new List<TagInfo>();
            var NotCorrectInsidePosition = new HashSet<int>();
            var correctOpenTag = new Stack<TagInfo>();
            foreach (var tag in pairTags)
            {
                if (!result.TryPeek(out var peekTag))
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
                if (correctOpenTag.TryPeek(out var peekOpenTag) && 
                    !peekOpenTag.tagConverter.CanProcessTag(tag.tagConverter.TagName))
                    NotCorrectInsidePosition.Add(tag.Pos);
                else
                    correctOpenTag.Push(tag);
                result.Push(tag);
            }
            return pairTags
                .Where(t => t.tagConverter.IsSingleTag || !NotCorrectInsidePosition.Contains(t.Pos))
                .ToList();
        }

        private static List<TagInfo> GetTagsWithoutEmptyInside(this List<TagInfo> pairTags)
        {
            var result = new Stack<TagInfo>();
            var positionsEmptyTag = new HashSet<int>();
            foreach(var tag in pairTags)
            {
                if(!result.TryPeek(out var peek))
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
            return pairTags
                .Where(t => t.tagConverter.IsSingleTag || !positionsEmptyTag.Contains(t.Pos))
                .ToList();
        }
        private static List<TagInfo> GetCorrectPairTags(this List<TagInfo> pairTags)
        {
            var result = new Stack<TagInfo>();
            if (!pairTags.Any())
                return new List<TagInfo>();
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
            return pairTags
                .Where(t => !TagsPositionNotCorrectTag.Contains(t.Pos))
                .ToList();
        }

        private static List<TagInfo> GetPairTags(this List<TagInfo> tags)
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
