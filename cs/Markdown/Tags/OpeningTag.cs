using System.Collections.Generic;

namespace Markdown.Tags
{
    public class OpeningTag : Tag
    {
        private readonly Dictionary<string, string> mdToHtml = new Dictionary<string, string>
        {
            {"_", "<em>"},
            {"__", "<strong>"},
            {"#", "<h1>"}
        };


        public OpeningTag(string mdTag, int position, ClosingTag closingTag = null)
        {
            this.mdTag = mdTag;
            this.position = position;
            PairTag = closingTag;
            htmlTag = mdToHtml[mdTag];
        }
    }
}