using System.Collections.Generic;
using System.Linq;
using Markdown.Extensions;
using Markdown.Tag;

namespace Markdown
{
    public class MdTagConverter
    {
        private readonly Dictionary<string, MdType> symbolMdTypeDictionary;
        private int position;
        private string rawText;

        public MdTagConverter(Dictionary<string, MdType> symbolMdTypeDictionary)
        {
            this.symbolMdTypeDictionary = symbolMdTypeDictionary;
        }

        public List<ITag> Parse(string text)
        {
            position = 0;
            rawText = text;
            var pairedTags = new List<ITag>();
            var tagLengths = symbolMdTypeDictionary.Keys.Select(t => t.Length)
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
                textTag.Content = textTag.GetContent(rawText);
                return true;
            }

            return false;
        }

        public bool IsOpenTag(string symbol)
        {
            var prevSymbol = rawText.LookAt(position - 1);
            var nextSymbol = rawText.LookAt(position + symbol.Length);

            return symbolMdTypeDictionary.ContainsKey(symbol) && !char.IsWhiteSpace(nextSymbol)
                                                              && (char.IsWhiteSpace(prevSymbol) || position == 0) ||
                   symbol == "[";
        }

        private bool TryParseOnePairOfTags(string symbol, out ITag tag)
        {
            var type = symbolMdTypeDictionary[symbol];
            tag = TagFactory.Create(type);
            tag.OpenIndex = position;
            tag.CloseIndex = tag.FindCloseIndex(rawText);

            if (tag.CloseIndex != -1)
            {
                tag.Content = tag.GetContent(rawText);
                if (tag.Type == MdType.Link)
                    tag.Attribute = tag.GetLinkTagAttribute(rawText);
                return true;
            }

            return false;
        }
    }
}