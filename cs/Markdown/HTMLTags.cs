using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class HTMLTags
    {
        private static List<TagSpecification> Tags = new List<TagSpecification>
        {
            new TagSpecification("<strong>", "</strong>", TagType.bold),
            new TagSpecification("<em>", "</em>", TagType.italics)
        };

        public static List<TagSpecification> GetAllSpecifications()
        {
            return new List<TagSpecification>(Tags);
        }
    }
}
