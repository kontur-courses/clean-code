using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;
using Markdown.Tags.BoldTag;
using Markdown.Tags.ItalicTag;

namespace Markdown
{
    public static class MarkdownFilter
    {
        public static Dictionary<int, Tag> FilterTags(Tag[] tags, int paragraphLength)
        {
            var tagDictionary = tags.ToDictionary(tag => tag.Index);
            FilterIntersection(tagDictionary, paragraphLength);
            FilterBoldTagsInItalic(tagDictionary, paragraphLength);
            return tagDictionary;
        }

        private static void FilterIntersection(Dictionary<int, Tag> tags, int paragraphLength)
        {
            if (tags.Count < 4)
            {
                return;
            }

            var tagsWindow = CollectTagsWindow(tags, 0, paragraphLength);

            for(var j = tagsWindow.Peek().Index; j < paragraphLength; j++)
            {
                if (TryRemoveIntersection(tags, tagsWindow))
                {
                    tagsWindow = CollectTagsWindow(tags, tagsWindow.Peek().Index, paragraphLength);
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

        private static Queue<Tag> CollectTagsWindow(Dictionary<int, Tag> tags, int index, int paragraphLength)
        {
            var currentTags = new Queue<Tag>();
            while (index < paragraphLength && currentTags.Count < 4)
            {
                if (tags.TryGetValue(index, out var tag))
                {
                    currentTags.Enqueue(tag);
                }

                index++;
            }

            return currentTags;
        }

        private static bool TryRemoveIntersection(Dictionary<int, Tag> tags, Queue<Tag> currentTags)
        {
            if (IsIntersection(currentTags.ToArray()))
            {
                foreach (var tag in currentTags)
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
        
        private static void FilterBoldTagsInItalic(Dictionary<int, Tag> tags, int paragraphLength)
        {
            var ignoreStrong = false;
            for (var i = 0; i <= paragraphLength; i++)
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