using System.Collections.Generic;

namespace MarkdownNew
{
    static class TagsPairsDictionary
    {
        public static Dictionary<Tag, Tag> GetTagsPairs()
        {
            var pairs = new Dictionary<Tag, Tag>();
            pairs.Add(new Tag("_", "_"), new Tag("<em>", "</em>"));
            pairs.Add(new Tag("__", "__"), new Tag("<strong>", "</strong>"));
            return pairs;
        }
    }
}
