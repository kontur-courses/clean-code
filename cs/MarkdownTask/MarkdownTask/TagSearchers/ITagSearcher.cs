using System.Collections.Generic;
using MarkdownTask.Tags;

namespace MarkdownTask.TagSearchers
{
    public interface ITagSearcher
    {
        string TagPrefix { get; }
        List<Tag> SearchForTags(string mdText);
    }
}