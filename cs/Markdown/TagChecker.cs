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
            return pairedTags.Concat(notPairedTags).OrderBy(x=>x.Position).ToList();
        }

        public static IEnumerable<Tag> ConfigureUnorderedLists(this List<Tag> tags)
        {
            Tag lastListItem = null;
            var openTags = tags.Where(x => x.IsOpening);
            var closeTags = tags.Where(x => !x.IsOpening);
            var tagPairs = openTags.Zip(closeTags, (x, y) => new
            {
                OpenTag = x,
                CloseTag = y
            });
            foreach (var tagPair in tagPairs)
            {
                if (lastListItem == null)
                {
                    yield return UnorderedListTagHelper.GetTag(tagPair.OpenTag.Position, true);
                }
                else if (tagPair.OpenTag.LineNumber - lastListItem.LineNumber != 1)
                {
                    yield return UnorderedListTagHelper.GetTag(lastListItem.Position, false);
                    yield return UnorderedListTagHelper.GetTag(tagPair.OpenTag.Position, true);
                }

                lastListItem = tagPair.CloseTag;
                yield return tagPair.OpenTag;
                yield return tagPair.CloseTag;
            }
            if (lastListItem != null)
                yield return UnorderedListTagHelper.GetTag(lastListItem.Position, false);
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