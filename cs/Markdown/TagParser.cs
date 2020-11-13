using System;
using System.Collections.Generic;

namespace Markdown
{
    public class TagParser
    {
        private static readonly string newLine = Environment.NewLine;

        public static readonly List<Tag> SupportedTags = new List<Tag>
        {
            ItalicTag.CreateInstance(), HeaderTag.CreateInstance(),
            BoldTag.CreateInstance(), EscapeTag.CreateInstance()
        };

        private readonly List<Tag> tags;
        private readonly string text;
        private bool hasHeaderInLine;
        private int position;

        public TagParser(string text)
        {
            this.text = text;
            tags = new List<Tag>();
        }

        public List<Tag> GetTags()
        {
            while (position < text.Length)
            {
                ReadLine();
                hasHeaderInLine = false;
            }

            return tags;
        }

        private void ReadLine()
        {
            while (position < text.Length && text.Substring(position, newLine.Length) != newLine)
            {
                SkipSpaces();
                ReadWord();
            }

            if (hasHeaderInLine)
                tags.Add(HeaderTag.GetCloseTag(position));
            position += Environment.NewLine.Length;
        }

        private void SkipSpaces()
        {
            while (position < text.Length &&
                   char.IsWhiteSpace(text[position])) position++;
        }

        private void ReadWord()
        {
            var hasDigits = false;
            var tagsInWord = new List<Tag>();

            while (position < text.Length && !char.IsWhiteSpace(text[position]))
            {
                if (char.IsDigit(text[position]))
                    hasDigits = true;
                if (!hasDigits && TryReadTag(out var tag))
                {
                    position += tag.GetMdTagLengthToSkip();
                    tagsInWord.Add(tag);
                    if (tag is HeaderTag)
                    {
                        hasHeaderInLine = true;
                        break;
                    }
                }
                else
                {
                    position++;
                }
            }

            if (!hasDigits)
                tags.AddRange(tagsInWord);
        }

        private bool TryReadTag(out Tag tag)
        {
            foreach (var supportedTag in SupportedTags)
                if (supportedTag.TryParse(position, text, out tag))
                    return true;

            tag = null;
            return false;
        }
    }
}