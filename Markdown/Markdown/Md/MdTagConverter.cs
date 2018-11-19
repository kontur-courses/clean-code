using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tag;

namespace Markdown
{
    public class MdTagConverter
    {
        private readonly Dictionary<string, Func<string, ITag>> dictionaryTags;
        private int position;
        private string rawText;

        public MdTagConverter(List<MdType> types)
        {
            dictionaryTags = new Dictionary<string, Func<string, ITag>>();
            foreach (var type in types)
                dictionaryTags.Add(MdTagSymbolDetector.Detect(type), MdTagCreator.Create);
        }

        public List<ITag> Parse(string text)
        {
            position = 0;
            rawText = text;
            var pairedTags = new List<ITag>();
            var tagLengths = dictionaryTags.Keys.Select(t => t.Length)
                .Distinct().OrderByDescending(l => l).ToArray();

            while (position < rawText.Length)
            {
                foreach (var tagLength in tagLengths)
                    AddPairsOfTags(pairedTags, tagLength);

                position++;
            }

            if (TryParseTextTag(pairedTags, text.Length, out var lastTextTag))
                pairedTags.Add(lastTextTag);

            return pairedTags;
        }

        public void AddPairsOfTags(List<ITag> pairedTags, int keyLength)
        {
            if (position + keyLength >= rawText.Length)
                return;
            var symbol = rawText.Substring(position, keyLength);

            if (IsOpenTag(symbol) && TryParseOnePairOfTags(symbol, out var tag))
            {
                if (TryParseTextTag(pairedTags, position, out var textTag))
                    pairedTags.Add(textTag);

                pairedTags.Add(tag);
                position = tag.CloseIndex + tag.Length - 1;
            }
        }

        private bool TryParseTextTag(List<ITag> pairedTags, int closeIndex, out TextTag textTag)
        {
            textTag = new TextTag
            {
                OpenIndex = pairedTags.Count == 0
                    ? 0
                    : pairedTags.Last().CloseIndex + pairedTags.Last().Length,
                CloseIndex = closeIndex - 1
            };
            if (textTag.CloseIndex - textTag.OpenIndex >= 0)
            {
                textTag.Content = textTag.GetTextContent(rawText);
                return true;
            }

            return false;
        }

        public bool IsOpenTag(string symbol)
        {
            var prevSymbol = rawText.LookAt(position - 1);
            var nextSymbol = rawText.LookAt(position + symbol.Length);

            return dictionaryTags.ContainsKey(symbol) && !char.IsWhiteSpace(nextSymbol)
                                                      && (char.IsWhiteSpace(prevSymbol) || position == 0);
        }

        private bool TryParseOnePairOfTags(string symbol, out ITag tag)
        {
            tag = MdTagCreator.Create(symbol);
            tag.OpenIndex = position;
            tag.CloseIndex = rawText.FindCloseTagIndex(tag);

            if (tag.CloseIndex != -1)
            {
                tag.Content = tag.GetTagContent(rawText);
                return true;
            }

            return false;
        }
    }
}