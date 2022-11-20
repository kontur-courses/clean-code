using System.Collections.Generic;

namespace Markdown
{
    public interface IParser<T> where T: Tag
    {
        public Dictionary<T, (int startTagIndex, int closeTagIndex)> GetIndexesTags(string text);
    }
}
