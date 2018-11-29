using System.Collections.Generic;

namespace MarkdownNew
{
    static class TagsPairsDictionary
    {
        public static Dictionary<Tag, Tag> GetTagsPairs()
        {
            var pairs = new Dictionary<Tag, Tag>
            {
                {new Tag("_", "_"), new Tag("<em>", "</em>")},
                {new Tag("__", "__"), new Tag("<strong>", "</strong>")}
            };
            return pairs;
        }
    }
}
