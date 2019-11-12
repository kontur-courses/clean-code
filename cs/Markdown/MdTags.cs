using System.Collections.Generic;

namespace Markdown
{
    static class MdTags
    {
        private static List<TagSpecification> Tags = new List<TagSpecification>
        {
            new TagSpecification("__", "__", TagType.bold),
            new TagSpecification("_", "_", TagType.italics, new List<TagType>(){TagType.bold})
        };

        public static List<TagSpecification> GetAllTags()
        {
            return new List<TagSpecification>(Tags);
        }
    }
}
