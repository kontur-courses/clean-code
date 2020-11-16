using System.Collections.Generic;
using Markdown.Infrastructure.Parsers.Tags;

namespace Markdown.Infrastructure.Formatters
{
    public interface IBlockFormatter
    {
        public IEnumerable<string> Format(Tag tag, IEnumerable<string> words);
    }
}