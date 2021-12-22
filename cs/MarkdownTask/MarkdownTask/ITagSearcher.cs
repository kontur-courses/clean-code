using System.Collections.Generic;

namespace MarkdownTask
{
    public interface ITagSearcher
    {
        List<Tag> SearchForTags(string mdText);
    }
}