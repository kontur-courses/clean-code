using Markdown.TagConverters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal static class TagsAssociation
    {
        private static readonly Dictionary<string, TagConverterBase> tagConverters = 
            new Dictionary<string, TagConverterBase>()
            {
                [new TagEm().StringMd] = new TagEm(),
                [new TagStrong().StringMd] = new TagStrong(),
                [new TagH1().StringMd] = new TagH1()
            };

        internal static readonly HashSet<string> tags = tagConverters.Keys.ToHashSet();
        internal static TagConverterBase GetTagConverter(string tagMd)
        {
            if(!tagConverters.ContainsKey(tagMd))
                return null;
            return tagConverters[tagMd];
        }

        internal static string GetTagMd(string text, int position, IEnumerable<string> tags)
        {
            string previousKey;
            string currentKey = null;
            var count = 0;
            do
            {
                previousKey = currentKey;
                count++;
                currentKey = GetTagMd(text, position, count, tags);
            }
            while (currentKey != null);
            return previousKey;
        }

        private static string GetTagMd(string text, int position, int countSymbols, IEnumerable<string> tags) 
        {
            if (position > text.Length - countSymbols)
                return null;
            var substring = text.Substring(position, countSymbols);
            if (tags.Contains(substring))
                return substring;
            return null;
        }
    }
}
