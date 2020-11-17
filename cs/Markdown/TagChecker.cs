using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class TagChecker
    {
        public static IEnumerable<Tag> GetCorrectTags(this List<Tag> tags, string text)
        {
            var pairedTags = tags
                .Where(x => x.IsPaired)
                .PairTags(text)
                .RemoveIntersectingPairTags()
                .RemoveBoldTagsInItalicTags();
            var singleTags = tags.Where(x => !x.IsPaired);
            return pairedTags.Concat(singleTags);
        }

        private static IEnumerable<Tag> PairTags(this IEnumerable<Tag> tags, string text)
        {
            var openTags = new Dictionary<TagType, Tag>();
            foreach (var tag in tags)
            {
                openTags.TryGetValue(tag.Type, out var openTag);
                if (openTag == null)
                {
                    if (tag.CanBeOpen(text))
                        openTags[tag.Type] = tag;
                }
                else
                {
                    if (openTag.TryPairCloseTag(tag, text) || openTag.Type == TagType.Header)
                    {
                        yield return openTag;
                        yield return tag;
                        openTags[tag.Type] = null;
                    }
                    else if (openTag.InWord && openTag.NotInOneWordWith(tag, text))
                    {
                        openTags[tag.Type] = tag;
                    }
                }
            }
        }

        private static IEnumerable<Tag> RemoveIntersectingPairTags(this IEnumerable<Tag> tags)
        {
            var openTags = new Stack<Tag>();
            foreach (var tag in tags.OrderBy(x => x.Position))
                if (!openTags.TryPeek(out var openTag) || openTag.Type != tag.Type)
                {
                    openTags.Push(tag);
                }
                else
                {
                    yield return openTags.Pop();
                    yield return tag;
                }
        }

        private static IEnumerable<Tag> RemoveBoldTagsInItalicTags(this IEnumerable<Tag> tags)
        {
            var betweenItalicTags = false;
            foreach (var tag in tags.OrderBy(x => x.Position))
                if (tag.Type == TagType.Italic)
                {
                    betweenItalicTags = !betweenItalicTags;
                    yield return tag;
                }
                else if (!betweenItalicTags)
                {
                    yield return tag;
                }
        }
    }
}