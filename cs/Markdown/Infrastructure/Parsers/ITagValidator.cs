using System.Collections.Generic;

namespace Markdown.Infrastructure.Parsers
{
    public interface ITagValidator
    {
        public IEnumerable<TagInfo> GetValidTags(IEnumerable<TagInfo> tagInfos);
    }
}