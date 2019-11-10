using System;
using System.Collections.Generic;
using Markdown.MdTag;

namespace Markdown.Parser
{
    class MdTagParser: IParser<Tag>
    {
        private Dictionary<string, string> MdToHtmlMatches;

        public List<Tag> Parse(string textToParse)
        {
            throw new NotImplementedException();
        }
    }
}
