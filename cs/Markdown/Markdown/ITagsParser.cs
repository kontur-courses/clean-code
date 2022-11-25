using System.Collections.Generic;

namespace Markdown
{
    public interface ITagsParser<T> where T : Tag
    {
        public IEnumerable<T> GetIndexesTags(string text);
    }
}