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

        public ClosingTag ClosingTag;

        private string parametrs;


        public OpeningTag(string mdTag, int position, ClosingTag closingTag)
        {
            this.mdTag = mdTag;
            this.position = position;
            ClosingTag = closingTag;
            PairTag = closingTag;
            htmlTag = mdToHtml[mdTag];
        }
    }
}