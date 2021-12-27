using System.Collections.Generic;
using MarkdownTask.Tags;

namespace MarkdownTask.Searchers
{
    public interface ITagSearcher
    {
        List<Tag> SearchForTags(List<int> escapedChars);
    }
}