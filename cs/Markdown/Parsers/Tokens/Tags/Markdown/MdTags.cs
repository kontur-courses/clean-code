using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Parsers.Tokens.Tags.Markdown
{
    public class MdTags
    {
        private static MdTags instance;
        private readonly Dictionary<string, Func<MdParsingLine, Tag>> mdTagDictionary = new Dictionary<string, Func<MdParsingLine, Tag>>();
        private readonly HashSet<char> serviceSymbols = new HashSet<char>();
        private readonly HashSet<char> tagStartSymbols = new HashSet<char>();

        private MdTags()
        {
            mdTagDictionary.Add(new MdBoldTag().ToString(), context => new MdBoldTag(context));
            mdTagDictionary.Add(new MdItalicTag().ToString(), context => new MdItalicTag(context));

            mdTagDictionary.Add(new MdHeaderTag().ToString(), context => new MdHeaderTag());
            mdTagDictionary.Add(new MdCommentTag().ToString(), context => new MdCommentTag());
            
            foreach (var tag in mdTagDictionary)
            {
                tagStartSymbols.Add(tag.Key[0]);
                foreach (var symbol in tag.Key)
                    serviceSymbols.Add(symbol);
            }
        }

        public static MdTags GetInstance() =>
            instance ?? (instance = new MdTags());

        public IToken TryToCreateTagFor(string text, MdParsingLine context)
        {
            var tag = mdTagDictionary[text].Invoke(context);

            if (context.Tokens.LastOrDefault() is MdCommentTag)
            {
                context.Tokens.Remove(context.Tokens.Last());
                return tag.ToText();
            }

            if (tag.IsValidTag(context))
                return tag; 
            else
                return tag.ToText();
        }

        public bool IsTag(string text) => 
            mdTagDictionary.ContainsKey(text);

        public bool IsServiceSymbol(char symbol) =>
            serviceSymbols.Contains(symbol);

        public bool IsTagStart(char symbol) =>
            tagStartSymbols.Contains(symbol);
    }
}
