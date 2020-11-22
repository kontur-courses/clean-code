using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Markdown
{
    public static class TagParser
    {
        private static readonly Dictionary<TagType, TagHelper> supportedTags = new Dictionary<TagType, TagHelper>
        {
            [TagType.Italic] = new ItalicTagHelper(),
            [TagType.Header] = new HeaderTagHelper(),
            [TagType.Bold] = new BoldTagHelper(),
            [TagType.Escape] = new EscapeTagHelper(),
            [TagType.UnorderedList] = new UnorderedListTagHelper(),
            [TagType.ListItem] = new ListItemTagHelper()
        };

        public static readonly IReadOnlyDictionary<TagType, TagHelper> SupportedTags =
            new ReadOnlyDictionary<TagType, TagHelper>(supportedTags);

        public static List<Tag> GetTagsFromParagraph(string paragraph)
        {
            var position = 0;
            var tags = new List<Tag>();
            while (position < paragraph.Length)
            {
                position += SkipSpaces(position, paragraph);
                tags.AddRange(ReadTagsFromWord(position, paragraph, out var symbolsReadCount));
                position += symbolsReadCount;
            }

            AddCloseParagraphTag(tags, position);
            return tags.GetCorrectTags(paragraph);
        }

        private static void AddCloseParagraphTag(List<Tag> tags, int position)
        {
            if (tags.Count == 0)
                return;
            if (tags[0].Type == TagType.Header)
                tags.Add(HeaderTagHelper.GetCloseTag(position));
            else if (tags[0].Type == TagType.ListItem)
                tags.Add(ListItemTagHelper.GetCloseTag(position));
        }

        private static int SkipSpaces(int position, string text)
        {
            var spacesCount = 0;
            while (position + spacesCount < text.Length && char.IsWhiteSpace(text[position + spacesCount]))
                spacesCount++;
            return spacesCount;
        }

        private static IEnumerable<Tag> ReadTagsFromWord(int position, string text, out int symbolsReadCount)
        {
            var startPosition = position;
            var hasDigits = false;
            var tagsInWord = new List<Tag>();
            var tagsInWordEnd = new List<Tag>();
            var tagsInWordBegin = ReadTags(position, text, out symbolsReadCount);
            position += symbolsReadCount;
            while (position < text.Length && !char.IsWhiteSpace(text[position]))
            {
                if (char.IsDigit(text[position]))
                    hasDigits = true;
                var readTags = ReadTags(position, text, out symbolsReadCount, true);
                tagsInWord.AddRange(readTags);
                tagsInWordEnd = readTags;
                position += Math.Max(1, symbolsReadCount);
            }

            symbolsReadCount = position - startPosition;
            foreach (var tag in tagsInWordEnd)
                tag.UnpinFromWord();

            return hasDigits ? tagsInWordBegin.Concat(tagsInWordEnd) : tagsInWordBegin.Concat(tagsInWord);
        }

        private static bool TryReadTag(int position, string text, out Tag tag, bool inWord = false)
        {
            foreach (var tagHelper in SupportedTags.Values)
                if (tagHelper.TryParse(position, text, out tag, inWord))
                    return true;

            tag = null;
            return false;
        }

        private static List<Tag> ReadTags(int position, string text, out int symbolsReadCount, bool inWord = false)
        {
            var startPosition = position;
            var tags = new List<Tag>();
            while (TryReadTag(position, text, out var tag, inWord))
            {
                tags.Add(tag);
                position += SupportedTags[tag.Type].GetSymbolsCountToSkipForParsing();
            }

            symbolsReadCount = position - startPosition;
            return tags;
        }
    }
}