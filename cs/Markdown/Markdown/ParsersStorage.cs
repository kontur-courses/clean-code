using System.Collections.Generic;
using Markdown.Parsers;

namespace Markdown
{
    public static class ParsersStorage
    {
        public static Dictionary<string, IParser> ParsersTable => MakeDictionary();

        private static Dictionary<string, IParser> MakeDictionary()
        {
            return new Dictionary<string, IParser>
            {
                ["\\"] = new ShieldingTagParser(),
                ["#"] = new HeaderParser(),
                [" "] = new SpaceParser(),
                ["["] = new StartLinkParser(),
                ["]"] = new EndLinkParser(),
                ["__"] = new DoubleUnderliningParser(),
                ["_"] = new SingleUnderliningParser(),
            };
        }
    }
}
