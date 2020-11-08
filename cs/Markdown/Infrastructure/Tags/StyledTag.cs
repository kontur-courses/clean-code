using System.Collections.Generic;
using System.Linq;
using Markdown.Infrastructure.Formatters;

namespace Markdown.Infrastructure.Tags
{
    public class StyledTag : ITag
    {
        private readonly Style style;
        private readonly IEnumerable<ITag> tags;

        public StyledTag(Style style, IEnumerable<ITag> tags)
        {
            this.style = style;
            this.tags = tags;
        }

        public IEnumerable<string> Format(TagFormatter tagFormatter)
        {
            return tagFormatter.Format(style, tags.SelectMany(token => token.Format(tagFormatter)));
        }
    }
}