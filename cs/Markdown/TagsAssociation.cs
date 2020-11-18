using Markdown.TagConverters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal static class TagsAssociation
    {
        private static readonly Dictionary<string, ITagConverter> tagConverters = 
            new Dictionary<string, ITagConverter>()
            {
                [new EmTagConverter().TagName] = new EmTagConverter(),
                [new StrongTagConverter().TagName] = new StrongTagConverter(),
                [new H1TagConverter().TagName] = new H1TagConverter(),
                [new UlTagConverter().TagName] = new UlTagConverter()
            };

        internal static readonly HashSet<string> tags = tagConverters.Keys.ToHashSet();

        private static readonly int maxLengthTag = tags.Any() ? tags.Select(t => t.Length).Max() : 0;
        internal static ITagConverter GetTagConverter(string tagMd)
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
