using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class HTMLTags
    {
        private static List<TagSpecification> TagsByPriority = new List<TagSpecification>
        {
            new TagSpecification("<em>", "</em>", TagType.italics),
            new TagSpecification("<strong>", "</strong>", TagType.bold)
        };

        public static List<TagSpecification> GetAllTags()
        {
            return new List<TagSpecification>(TagsByPriority);
        }
    }
}
