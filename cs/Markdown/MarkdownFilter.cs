using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;
using Markdown.Tags.BoldTag;
using Markdown.Tags.ItalicTag;

namespace Markdown
{
    public static class MarkdownFilter
    {
        public static Dictionary<int, Tag> FilterTags(Tag[] tags, int length)
        {
            var tagDictionary = tags.ToDictionary(tag => tag.Index);
            FilterIntersection(tagDictionary, length);
            FilterBoldTagsInItalic(tagDictionary, length);
            return tagDictionary;
        }

        private static void FilterIntersection(Dictionary<int, Tag> tags, int length)
        {
            if (tags.Count < 4)
            {
                return;
            }

            var tagsWindow = CollectTagsWindow(tags, 0, length);

            for(var j = tagsWindow.Peek().Index; j < length; j++)
            {
                if (TryRemoveIntersection(tags, tagsWindow))
                {
                    tagsWindow = CollectTagsWindow(tags, tagsWindow.Peek().Index, length);
                    if (tagsWindow.Count < 4)
                    {
                        return;
                    }
                }
                else if (tags.TryGetValue(j, out var tag))
                {
                    tagsWindow.Enqueue(tag);
                    tagsWindow.Dequeue();
                }
                
            }
        }

        private static Queue<Tag> CollectTagsWindow(Dictionary<int, Tag> tags, int index, int length)
        {
            var list = new Queue<Tag>();
            while (index < length && list.Count < 4)
            {
                if (tags.TryGetValue(index, out var tag))
                {
                    list.Enqueue(tag);
                }

                index++;
            }

            return list;
        }

        private static bool TryRemoveIntersection(Dictionary<int, Tag> tags, Queue<Tag> list)
        {
            if (IsIntersection(list.ToArray()))
            {
                foreach (var tag in list)
                {
                    tags.Remove(tag.Index);
                }

                return true;
            }

            return false;
        }

        private static bool IsIntersection(Tag[] window)
        {
            return (window[0] is OpenBoldTag 
                   && window[1] is OpenItalicTag
                   && window[2] is CloseBoldTag
                   && window[3] is CloseItalicTag)
                   || (window[0] is OpenItalicTag
                       && window[1] is OpenBoldTag
                       && window[2] is CloseItalicTag
                       && window[3] is CloseBoldTag);
        }
        
        private static void FilterBoldTagsInItalic(Dictionary<int, Tag> tags, int length)
        {
            var ignoreStrong = false;
            for (var i = 0; i <= length; i++)
            {
                if (tags.TryGetValue(i, out var tag))
                {
                    switch (tag)
                    {
                        case OpenItalicTag _:
                            ignoreStrong = true;
                            break;
                        case CloseItalicTag _:
                            ignoreStrong = false;
                            break;
                        case BoldTag _:
                            if (ignoreStrong)
                            {
                                tags.Remove(i);
                            }
                            break;
                    }
                }
            }
        }
    }
}