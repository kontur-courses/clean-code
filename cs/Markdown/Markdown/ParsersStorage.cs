using System.Collections.Generic;
using Markdown.Parsers;

namespace Markdown
{
    public static class ParsersStorage
    {
        public static List<IParser> ParserList => MakeParserList();

        private static List<IParser> MakeParserList()
        {
            return new List<IParser>()
            {
                new DoubleUnderliningParser(),
                new SingleUnderliningParser(),
                new ShieldingTagParser(),
                new HeaderParser(),
                new SpaceParser(),
                new StartLinkParser(),
                new EndLinkParser(),
            };
        }
    }
}
