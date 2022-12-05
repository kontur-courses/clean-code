using System;
using System.Collections.Generic;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdTags
    {
        private static MdTags instance;
        private readonly Dictionary<string, Func<MarkdownParsingLine, Tag>> mdTagDictionary = new Dictionary<string, Func<MarkdownParsingLine, Tag>>();
        private readonly HashSet<char> serviceSymbols = new HashSet<char>();
        private readonly HashSet<char> tagStartSymbols = new HashSet<char>();

        private MdTags()
        {
            mdTagDictionary.Add(new MdBoldTag().ToString(), context => new MdBoldTag(context));
            mdTagDictionary.Add(new MdItalicTag().ToString(), context => new MdItalicTag(context));

            mdTagDictionary.Add(new MdHeaderTag().ToString(), context => new MdHeaderTag());
            
            foreach (var tag in mdTagDictionary)
            {
                tagStartSymbols.Add(tag.Key[0]);
                foreach (var symbol in tag.Key)
                    serviceSymbols.Add(symbol);
            }
        }

        public static MdTags GetInstance() => 
            instance ??= new MdTags();

        public IToken TryToCreateTagFor(string text, MarkdownParsingLine context)
        {
            var tag = mdTagDictionary[text].Invoke(context);

            return tag.TryToValidate(context) 
                ? tag 
                : tag.ToText();
        }

        public bool IsTag(string text) => 
            mdTagDictionary.ContainsKey(text);

        public bool IsServiceSymbol(char symbol) =>
            serviceSymbols.Contains(symbol);

        public bool IsTagStart(char symbol) =>
            tagStartSymbols.Contains(symbol);
    }
}
