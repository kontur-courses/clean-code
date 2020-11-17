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
                [new EmITagConverter().StringMd] = new EmITagConverter(),
                [new StrongITagConverter().StringMd] = new StrongITagConverter(),
                [new H1ITagConverter().StringMd] = new H1ITagConverter(),
                [new UlITagConverter().StringMd] = new UlITagConverter()
            };

        internal static readonly HashSet<string> tags = tagConverters.Keys.ToHashSet();

        private static readonly int maxLengthTag = tags.Select(t => t.Length).Max();
        internal static TagConverterBase GetTagConverter(string tagMd)
        {
            if(!tagConverters.ContainsKey(tagMd))
                return null;
            return tagConverters[tagMd];
        }

        internal static string GetTagMd(string text, int position, IEnumerable<string> tags)
        {
            string currentKey = null; 
            for(var count = Math.Max(maxLengthTag, text.Length - position); currentKey == null && count > 0; count--)
                currentKey = GetTagMd(text, position, count, tags);
            return currentKey;
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
