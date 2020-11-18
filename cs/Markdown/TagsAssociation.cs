using Markdown.TagConverters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal static class TagsAssociation
    {
        private static readonly ITagConverter[] tagConverters = new ITagConverter[]
        {
            new EmTagConverter(),
            new StrongTagConverter(),
            new H1TagConverter(),
            new UlTagConverter()
        };

        internal static readonly HashSet<string> tags = 
            tagConverters
            .Select(t => t.TagName)
            .ToHashSet();

        private static readonly int maxLengthTag = tags.Any() ? tags.Select(t => t.Length).Max() : 0;
        internal static ITagConverter GetTagConverter(string tagMd) => 
            tagConverters
            .FirstOrDefault(t => t.TagName == tagMd);

        internal static string GetTagMd(string text, int position, HashSet<string> tags)
        {
            string currentKey = null; 
            for(var count = Math.Max(maxLengthTag, text.Length - position); currentKey == null && count > 0; count--)
                currentKey = GetTagMd(text, position, count, tags);
            return currentKey;
        }

        private static string GetTagMd(string text, int position, int countSymbols, HashSet<string> tags) 
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
