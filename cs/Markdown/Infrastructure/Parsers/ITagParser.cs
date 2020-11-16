using System.Collections.Generic;

namespace Markdown.Infrastructure.Parsers
{
    public interface ITagParser
    {
        public IEnumerable<TagInfo> GetTags();
    }
}