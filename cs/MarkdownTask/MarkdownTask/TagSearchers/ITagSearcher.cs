using System.Collections.Generic;
using MarkdownTask.Tags;

namespace MarkdownTask.TagSearchers
{
    public interface ITagSearcher
    {
        List<Tag> SearchForTags(string mdText);
    }
}