using Markdown.TagConverters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Markdown
{
    internal static class TagsAssociation
    {
        private static readonly Dictionary<string, ITagConverter> tagConverters = new Dictionary<string, ITagConverter>()
        {
            [new TagEm().StringMd] = new TagEm(),
            [new TagStrong().StringMd] = new TagStrong()
        };
        internal static ITagConverter GetTagConverter(string text, int position)
        {
            var tagMd = GetTagMd(text, position);
            if(tagMd == null)
                return new EmptyTagConverter();
            return tagConverters[tagMd];
        }

        private static string GetTagMd(string text, int position)
        {
            string previousKey;
            string currentKey = null;
            var count = 0;
            do
            {
                previousKey = currentKey;
                count++;
                currentKey = GetTagMd(text, position, count);
            }
            while (currentKey != null);
            return previousKey;
        }

        private static string GetTagMd(string text, int position, int countSymbols) 
        {
            var substring = text.Substring(position, countSymbols);
            if (tagConverters.ContainsKey(substring))
                return substring;
            return null;
        }
    }
}
