using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    static class MdTags
    {
        private static List<TagSpecification> Tags = new List<TagSpecification>
        {
            new TagSpecification("_", "_", TagType.italics),
            new TagSpecification("__", "__", TagType.bold)
        };

        public static List<TagSpecification> GetAllTags()
        {
            return new List<TagSpecification>(Tags);
        }
    }
}
