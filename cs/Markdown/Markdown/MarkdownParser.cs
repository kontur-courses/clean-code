using System.Collections.Generic;

namespace Markdown
{
    public class MarkdownParser : ITagsParser<MdTag>
    {
        public List<MdTag> GetIndexesTags(string text)
        {
            return new List<MdTag>();
        }
    }
}