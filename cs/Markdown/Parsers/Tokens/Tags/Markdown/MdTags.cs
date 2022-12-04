using System;
using System.Collections.Generic;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdTags
    {
        private static MdTags instance;
        private readonly Dictionary<string, Func<MdPairedTag, Tag>> mdTagDictionary = new Dictionary<string, Func<MdPairedTag, Tag>>();
        private readonly HashSet<char> serviceSymbols = new HashSet<char>();
        private readonly HashSet<char> tagStartSymbols = new HashSet<char>();

        private MdTags()
        {
            mdTagDictionary.Add(new MdBoldTag(null).ToString(), startTag => new MdBoldTag(startTag));
            mdTagDictionary.Add(new MdItalicTag(null).ToString(), startTag => new MdItalicTag(startTag));

            mdTagDictionary.Add(new MdHeaderTag().ToString(), p => new MdHeaderTag());
            mdTagDictionary.Add(new MdCommentTag().ToString(), p => new MdCommentTag());
            
            foreach (var tag in mdTagDictionary)
            {
                tagStartSymbols.Add(tag.Key[0]);
                foreach (var symbol in tag.Key)
                {
                    serviceSymbols.Add(symbol);
                }
            }
        }

        public static MdTags GetInstance()
        {
            return instance ?? (instance = new MdTags());
        }

        public Tag CreateTagFor(string text, MdPairedTag startTag = null) =>
            mdTagDictionary[text].Invoke(startTag);

        public bool IsTag(string text) => mdTagDictionary.ContainsKey(text);

        public bool IsServiceSymbol(char symbol) =>
            serviceSymbols.Contains(symbol);

        public bool IsTagStart(char symbol) =>
            tagStartSymbols.Contains(symbol);

    }
}
