using System.Collections.Generic;

namespace Markdown.Tags
{
    public class Tag
    {
        public static readonly IReadOnlyList<string> MdTagValues = new List<string> {"_", "#", "__"};
        public Tag PairTag { get; set; }
        public string mdTag { get; protected set; }
        public string htmlTag { get; set; }
        public int position { get; protected set; }
    }
}