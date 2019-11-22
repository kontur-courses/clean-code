using System.Collections.Generic;
using System.Linq;
using Markdown.Core.Tags;

namespace Markdown.Core.Parsers
{
    class MainParser : IParser
    {
        private readonly SingleTagParser singleTagParser;
        private readonly DoubleTagParser doubleTagParser;

        public MainParser(IEnumerable<ITag> tags)
        {
            singleTagParser = new SingleTagParser(tags);
            doubleTagParser = new DoubleTagParser(tags);
        }


        public List<TagToken> ParseLine(string line)
        {
            var singleTagTokens = singleTagParser.ParseLine(line);
            var doubleTagTokens = doubleTagParser.ParseLine(line);
            return singleTagTokens.Concat(doubleTagTokens).ToList();
        }
    }
}