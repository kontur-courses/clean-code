using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    internal static class TagRegistry
    {

        private static readonly Dictionary<string, TagInfo>  tags = new Dictionary<string, TagInfo>() // появится новый тег - можно написать его сюда
            {
                ["__"] = new TagInfo("__", "strong"),
                ["_"] = new TagInfo("_", "em")
            };

    public static IEnumerable<TagInfo> GetAllTags() => tags.Values.OrderByDescending(tag => tag.Length);
    }
}
