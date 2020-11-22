using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class TagChecker
    {
        public static List<Tag> GetCorrectTags(this List<Tag> tags, string text)
        {
            var pairedTags = tags
                .Where(x => x.IsMdPaired)
                .PairTags(text)
                .RemoveIntersectingPairTags()
                .RemoveBoldTagsInItalicTags();
            var notPairedTags = tags.Where(x => !x.IsMdPaired);
            return pairedTags.Concat(notPairedTags).OrderBy(x => x.Position).ToList();
        }

        public static bool IsNeedToAddUnorderedListTag(this List<Tag> tags, bool inUnorderedList)
        {
            var tagsIsEmpty = tags.Count == 0;
            if (!inUnorderedList)
                return !tagsIsEmpty && tags[0].Type == TagType.ListItem;
            return !tagsIsEmpty && tags[0].Type != TagType.ListItem || tagsIsEmpty;
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
                    if (openTag.TryPairCloseTag(tag, text))
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
            {
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
        }

        private static IEnumerable<Tag> RemoveBoldTagsInItalicTags(this IEnumerable<Tag> tags)
        {
            var betweenItalicTags = false;
            foreach (var tag in tags.OrderBy(x => x.Position))
            {
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
}