using Markdown.TagConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Markdown
{
    internal static class TagsAssociation
    {
        internal static readonly Dictionary<string, ITagConverter> tagConverters = 
            new Dictionary<string, ITagConverter>()
        {
            [new TagEm().StringMd] = new TagEm(),
            [new TagStrong().StringMd] = new TagStrong(),
            [@"\"] = new TagShield()
        };

        internal static readonly HashSet<string> tags = tagConverters.Keys.ToHashSet();
        internal static ConverterInfo GetTagConverter(string text, int position)
        {
            var tagMd = GetTagMd(text, position, tags);
            if(tagMd == null)
                return new ConverterInfo(new EmptyTagConverter(), text, position);
            return new ConverterInfo(tagConverters[tagMd], text, position);
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
            if (position >= text.Length - countSymbols)
                return null;
            var substring = text.Substring(position, countSymbols);
            if (tags.Contains(substring))
                return substring;
            return null;
        }
    }
}
