using System.Collections.Generic;

namespace Markdown
{
    public class MarkdownParser : ITagsParser<MdTag>
    {
        public Dictionary<MdTag, (int startTagIndex, int closeTagIndex)> GetIndexesTags(string text)
        {
            return new Dictionary<MdTag, (int, int)>();
        }
    }
}