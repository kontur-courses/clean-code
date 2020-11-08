using System.Collections.Generic;
using Markdown.Infrastructure.Formatters;

namespace Markdown.Infrastructure.Tags
{
    public interface ITag
    {
        public IEnumerable<string> Format(TagFormatter tagFormatter);
    }
}