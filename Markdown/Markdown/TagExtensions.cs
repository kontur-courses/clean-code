using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tag;

namespace Markdown
{
    public static class TagExtensions
    {
        public static TextTag ToTextTag(this ITag tag)
        {
            return new TextTag
            {
                Content = $"{tag.Symbol}{tag.Content}{tag.Symbol}"
            };
        }
    }
}
