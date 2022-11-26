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
        private readonly HashSet<char> tagStartSymbols = new HashSet<char>();

        private MdTags()
        {
            mdTagDictionary.Add(new MdBoldTag(TagPosition.Any).ToString(), p => new MdBoldTag(p));
            mdTagDictionary.Add(new MdItalicTag(TagPosition.Any).ToString(), p => new MdItalicTag(p));

            mdTagDictionary.Add(new MdHeaderTag().ToString(), p => new MdHeaderTag());
            mdTagDictionary.Add(new MdCommentTag().ToString(), p => new MdCommentTag());
            
            foreach (var tag in mdTagDictionary)
            {
                tagStartSymbols.Add(tag.Key[0]);
                foreach (var symbol in tag.Key)
                {
                    //TODO: delete? if(symbol != ' ')
                        serviceSymbols.Add(symbol);
                }
            }
        }

        public static MdTags GetInstance()
        {
            return instance ?? (instance = new MdTags());
        }

        public Tag CreateTagFor(string text, TagPosition tagPosition = TagPosition.Any) =>
            mdTagDictionary[text].Invoke(tagPosition);
        

        public bool IsTag(string text) => mdTagDictionary.ContainsKey(text);

        public bool IsServiceSymbol(char symbol) =>
            serviceSymbols.Contains(symbol);

        public bool IsTagStart(char symbol) =>
            tagStartSymbols.Contains(symbol);

    }
}
