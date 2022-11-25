using System.Collections.Generic;

namespace Markdown
{
    public interface ITagsParser<T> where T : TagWithIndex
    {
        public IEnumerable<T> GetIndexesTags(string text);
    }
}