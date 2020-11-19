using System;
using System.Collections.Generic;

namespace Markdown
{
    public static class TagParser
    {
        private static readonly string NewLine = Environment.NewLine;

        public static readonly Dictionary<TagType, TagHelper> SupportedTags = new Dictionary<TagType, TagHelper>
        {
            [TagType.Italic] = new ItalicTagHelper(),
            [TagType.Header] = new HeaderTagHelper(),
            [TagType.Bold] = new BoldTagHelper(),
            [TagType.Escape] = new EscapeTagHelper(),
            [TagType.UnorderedList] = new UnorderedListTagHelper(),
            [TagType.ListItem] = new ListItemTagHelper()
        };

        public static IEnumerable<Tag> GetTags(string text)
        {
            var position = 0;
            var lineNumber = 0;
            var tags = new List<Tag>();
            while (position < text.Length)
            {
                var tagsFromLine = ReadTagsFromLine(position, text, lineNumber, out var symbolsReadCount);
                position += symbolsReadCount;
                tags.AddRange(tagsFromLine);
                lineNumber++;
            }

            return tags.ConfigureUnorderedLists();
        }

        private static IEnumerable<Tag> ReadTagsFromLine(
            int position,
            string text,
            int lineNumber,
            out int symbolsReadCount)
        {
            var startPosition = position;
            var tags = new List<Tag>();
            var isHeader = false;
            var isListItem = false;
            if (SupportedTags[TagType.Header].TryParse(position, text, out var tag))
            {
                isHeader = true;
                position += SupportedTags[TagType.Header].GetSymbolsCountToSkipForParsing();
                tags.Add(tag);
            }

            if (SupportedTags[TagType.ListItem].TryParse(position, text, out tag, false, lineNumber))
            {
                isListItem = true;
                position += SupportedTags[TagType.ListItem].GetSymbolsCountToSkipForParsing();
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

            if (IsNeedToAddCloseTag(isHeader, isListItem, position, out tag))
                tags.Add(tag);
            symbolsReadCount = position + NewLine.Length - startPosition;
            return tags.GetCorrectTags(text);
        }

        private static bool IsNeedToAddCloseTag(bool isLineHeader, bool isLineListItem, int position, out Tag closeTag)
        {
            if (isLineHeader)
            {
                closeTag = HeaderTagHelper.GetCloseTag(position);
                return true;
            }

            if (isLineListItem)
            {
                closeTag = ListItemTagHelper.GetCloseTag(position);
                return true;
            }

            closeTag = null;
            return false;
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
            var hasDigits = false;
            var allTags = new List<Tag>();
            var tagsInWordEnd = new List<Tag>();

            while (position < text.Length && !char.IsWhiteSpace(text[position]))
            {
                if (char.IsDigit(text[position]))
                    hasDigits = true;
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

            return hasDigits ? tagsInWordEnd : allTags;
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