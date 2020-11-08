using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Tag
    {
        public static readonly Dictionary<string, Tuple<string, string>> MatchingSymbolsToTags =
            new Dictionary<string, Tuple<string, string>>
            {
                ["_"] = Tuple.Create("<em>", "</em>")
            };

        public Tag(string value, int index)
        {
            Value = value;
            Index = index;
        }

        public string Value { get; }
        public int Index { get; }

        public static bool IsTag(string symbol)
        {
            return MatchingSymbolsToTags.ContainsKey(symbol);
        }
    }
}