using System.Collections.Generic;
using System.Linq;
using Markdown.Core.Rules;
using Markdown.Core.Tags;

namespace Markdown.Core.Parsers
{
    class MainParser : IParser
    {
        private readonly IEnumerable<ITag> tags;

        public MainParser(IEnumerable<ITag> tags)
        {
            this.tags = tags;
        }


        public List<TagToken> ParseLine(string line)
        {
            var singleTagTokens = new SingleTagParser(tags).ParseLine(line);
            var doubleTagTokens = new DoubleTagParser(tags).ParseLine(line);
            return singleTagTokens.Concat(doubleTagTokens).ToList();
        }
    }
}