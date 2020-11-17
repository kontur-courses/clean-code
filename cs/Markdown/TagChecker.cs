using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class TagChecker
    {
        public static IEnumerable<Tag> GetCorrectTags(this List<Tag> tags, string text)
        {
            var pairedTags = tags
                .Where(x => x.IsMdPaired)
                .PairTags(text)
                .RemoveIntersectingPairTags()
                .RemoveBoldTagsInItalicTags();
            var notPairedTags = tags.Where(x => !x.IsMdPaired);
            return pairedTags.Concat(notPairedTags);
        }

        public static IEnumerable<Tag> ConfigureUnorderedLists(this List<Tag> tags)
        {
            Tag lastListItem = null;
            foreach (var tag in tags)
                if (tag.Type == TagType.ListItem)
                {
                    if (tag.IsOpening)
                    {
                        if (lastListItem == null)
                        {
                            yield return UnorderedListTagHelper.GetTag(tag.Position, true);
                        }
                        else if (tag.LineNumber - lastListItem.LineNumber != 1)
                        {
                            yield return UnorderedListTagHelper.GetTag(lastListItem.Position, false);
                            yield return UnorderedListTagHelper.GetTag(tag.Position, true);                        
                        }
                    }
                    else
                    {
                        lastListItem = tag;
                    }

                    yield return tag;
                }
                else
                {
                    yield return tag;
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