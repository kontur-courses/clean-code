using System.Collections.Generic;

namespace Markdown
{
    public interface ITagsParser<T> where T: Tag
    {
        public List<T> GetIndexesTags(string text);
    }
}
