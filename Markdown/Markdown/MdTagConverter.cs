using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tag;

namespace Markdown
{
    public class MdTagConverter
    {
        public readonly Dictionary<string, ITag> dictionaryTags;
        private int position;

        public MdTagConverter(Dictionary<string, ITag> dictionaryTags)
        {
            this.dictionaryTags = dictionaryTags;
        }

        public List<ITag> Parse(string text)
        {
            var pairedTags = new List<ITag>();
            position = 0;

            while (position < text.Length - 1)
            {
                var twoSymbol = text.Substring(position, 2);
                var symbol = text[position].ToString();

                if (position == text.Length - 2)
                {
                    if (TryParseTextTag(text, pairedTags, text.Length, out var textTag))
                        pairedTags.Add(textTag);
                }

                if (IsOpenTag(twoSymbol, text) && TryParseOnePairOfTags(twoSymbol, text, out var tag))
                {
                    if (TryParseTextTag(text, pairedTags, position, out var textTag))
                        pairedTags.Add(textTag);


                    pairedTags.Add(tag);
                    position = tag.CloseIndex + tag.Length - 1;

                }

                if (IsOpenTag(symbol, text) && TryParseOnePairOfTags(symbol, text, out tag))
                {
                    if (TryParseTextTag(text, pairedTags, position, out var textTag))
                        pairedTags.Add(textTag);

                    pairedTags.Add(tag);
                    position = tag.CloseIndex + tag.Length - 1;
                }



                position++;
            }

            return pairedTags;
        }

        private bool TryParseTextTag(string text, List<ITag> pairedTags, int closeIndex, out TextTag textTag)
        {
            textTag = new TextTag
            {
                OpenIndex = pairedTags.Count == 0
                    ? 0
                    : pairedTags.Last().CloseIndex + pairedTags.Last().Length,
                CloseIndex = closeIndex-1
            };
            if (textTag.CloseIndex - textTag.OpenIndex >= 0)
            {
                textTag.Content = text.Substring(textTag.OpenIndex, textTag.CloseIndex - textTag.OpenIndex + 1);
                return true;
            }

            return false;
        }

        public bool IsOpenTag(string symbol, string text)
        {
            var prevSymbol = text.LookAt(position - 1);
            var nextSymbol = text.LookAt(position + symbol.Length);

            return dictionaryTags.ContainsKey(symbol) && !char.IsWhiteSpace(nextSymbol)
                                                      && (char.IsWhiteSpace(prevSymbol) || position == 0);
        }

        private bool TryParseOnePairOfTags(string symbol, string text, out ITag tag)
        {
            var tagType = dictionaryTags[symbol].GetType();
            tag = (ITag) Activator.CreateInstance(tagType);
            tag.OpenIndex = position;
            tag.CloseIndex = text.FindCloseTagIndex(tag);

            if (tag.CloseIndex != -1)
                tag.Content = text.Substring(tag.OpenIndex + tag.Length, tag.CloseIndex - tag.OpenIndex - tag.Length);

            return tag.CloseIndex != -1;
        }
    }
}