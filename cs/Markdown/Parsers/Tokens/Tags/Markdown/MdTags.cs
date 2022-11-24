using Markdown.Parsers.Tokens.Tags.Enum;
using System;
using System.Collections.Generic;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdTags
    {
        private static MdTags instance;
        private readonly Dictionary<string, Func<TagPosition, Tag>> mdTagDictionary = new Dictionary<string, Func<TagPosition, Tag>>();
        private readonly HashSet<char> serviceSymbols = new HashSet<char>();

        private MdTags()
        {
            mdTagDictionary.Add(new MdBoldTag(TagPosition.Any).ToString(), p => new MdBoldTag(p));
            mdTagDictionary.Add(new MdItalicTag(TagPosition.Any).ToString(), p => new MdItalicTag(p));

            mdTagDictionary.Add(new MdHeaderTag().ToString(), p => new MdHeaderTag());

            foreach (var tag in mdTagDictionary)
            {
                foreach (var symbol in tag.Key)
                {
                    serviceSymbols.Add(symbol);
                }
            }
        }

        public static MdTags GetInstance()
        {
            return instance ??= new MdTags();
        }

        public Tag CreateTagFor(string text, TagPosition tagPosition = TagPosition.Any) =>
            mdTagDictionary[text].Invoke(tagPosition);
        

        public bool IsTag(string text) => mdTagDictionary.ContainsKey(text);

        public bool IsServiceSymbol(char symbol) =>
            serviceSymbols.Contains(symbol);
    }
}
