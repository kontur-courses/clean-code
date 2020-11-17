using System.Collections.Generic;

namespace Markdown.Tags
{
    public class ClosingTag : Tag
    {
        private readonly Dictionary<string, string> mdToHtml = new Dictionary<string, string>
        {
            {"_", "</em>"},
            {"__", "</strong>"},
            {"#", "</h1>"}
        };

        private string parametrs;


        public ClosingTag(string mdTag, int position)
        {
            this.mdTag = mdTag;
            this.position = position;
            htmlTag = mdToHtml[mdTag];
        }
    }
}