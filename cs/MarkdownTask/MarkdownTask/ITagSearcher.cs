using System.Collections.Generic;

namespace MarkdownTask
{
    public interface ITagSearcher
    {
        string TagPrefix { get; }
        List<Tag> SearchForTags(string mdText);
    }
}