using System;
using System.Collections.Generic;

namespace Markdown
{
    public static class TagParser
    {
        private static readonly string NewLine = Environment.NewLine;

        public static readonly Dictionary<TagType, TagHelper> SupportedTags = new Dictionary<TagType, TagHelper>
        {
            [TagType.Italic] = ItalicTagHelper.CreateInstance(),
            [TagType.Header] = HeaderTagHelper.CreateInstance(),
            [TagType.Bold] = BoldTagHelper.CreateInstance(),
            [TagType.Escape] = EscapeTagHelper.CreateInstance()
        };

        public static IEnumerable<Tag> GetTags(string text)
        {
            var position = 0;
            var tags = new List<Tag>();
            while (position < text.Length)
            {
                var tagsFromLine = ReadTagsFromLine(position, text, out var symbolsReadCount);
                position += symbolsReadCount;
                tags.AddRange(tagsFromLine);
            }

            return tags;
        }

        private static IEnumerable<Tag> ReadTagsFromLine(int position, string text, out int symbolsReadCount)
        {
            var startPosition = position;
            var tags = new List<Tag>();
            var hasHeaderInLine = false;
            if (SupportedTags[TagType.Header].TryParse(position, text, out var tag))
            {
                hasHeaderInLine = true;
                position += SupportedTags[TagType.Header].GetSymbolsCountToSkipForParsing();
                tags.Add(tag);
            }

            while (position < text.Length && IsNotNewLine(position, text))
            {
                position += SkipSpaces(position, text);
                tags.AddRange(ReadTags(position, text, out symbolsReadCount));
                position += symbolsReadCount;
                tags.AddRange(ReadTagsFromWord(position, text, out symbolsReadCount));
                position += symbolsReadCount;
            }

            if (hasHeaderInLine)
                tags.Add(new Tag(position, TagType.Header, false, 0, false, true));
            symbolsReadCount = position + NewLine.Length - startPosition;
            return tags.GetCorrectTags(text);
        }

        private static bool IsNotNewLine(int position, string text)
        {
            return position + NewLine.Length >= text.Length || text.Substring(position, NewLine.Length) != NewLine;
        }

        private static int SkipSpaces(int position, string text)
        {
            var spacesCount = 0;
            while (position + spacesCount < text.Length && char.IsWhiteSpace(text[position + spacesCount]))
                spacesCount++;
            return spacesCount;
        }

        private static List<Tag> ReadTagsFromWord(int position, string text, out int symbolsReadCount)
        {
            var startPosition = position;
            var hasNoDigits = true;
            var allTags = new List<Tag>();
            var tagsInWordEnd = new List<Tag>();

            while (position < text.Length && !char.IsWhiteSpace(text[position]))
            {
                if (char.IsDigit(text[position]))
                    hasNoDigits = false;
                var readTags = ReadTags(position, text, out symbolsReadCount, true);
                if (readTags.Count > 0)
                {
                    position += symbolsReadCount;
                    allTags.AddRange(readTags);
                    tagsInWordEnd = readTags;
                }
                else
                {
                    position++;
                    tagsInWordEnd.Clear();
                }
            }

            symbolsReadCount = position - startPosition;
            foreach (var tag in tagsInWordEnd) tag.UnpinFromWord();

            return hasNoDigits ? allTags : tagsInWordEnd;
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