using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class HTMLTags
    {
        private static List<TagSpecification> Tags = new List<TagSpecification>
        {
            new TagSpecification("<em>", "<\\em>", TagType.italics),
            new TagSpecification("<strong>", "<\\strong>", TagType.bold)
        };

        public static List<TagSpecification> GetAllSpecifications()
        {
            return new List<TagSpecification>(Tags);
        }
    }
}
