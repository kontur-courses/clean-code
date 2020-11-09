using Markdown.TagConverters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    internal static class TagsAssociation
    {
        private static readonly Dictionary<string, ITagConverter> tagConverters = new Dictionary<string, ITagConverter>()
        {
            ["_"] = new TagEm()
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
            if (tagConverters.ContainsKey(text[position].ToString()))
                return text[position].ToString();
            return null;
        }
    }
}
