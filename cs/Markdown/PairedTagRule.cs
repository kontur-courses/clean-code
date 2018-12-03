using System;

namespace Markdown
{
    internal class PairedTagRule : ITextProcessorRule
    {
        private readonly string fullTag;

        public PairedTagRule(char tag, int length)
        {
            Tag = tag;
            Length = length > 0 ? length : throw new ArgumentException("Length must be positive");
            fullTag = new string(tag, length);
        }

        public int Length { get; set; }

        public char Tag { get; set; }

        public bool Check(int position, string text)
        {
            if (position + Length < text.Length && text[position + Length] == Tag)
                return false;
            if (position + Length <= text.Length)
                return text.Substring(position, Length) == fullTag;
            return false;
        }

        public bool Check(Delimiter delimiter) => delimiter.Value == fullTag;

        public Delimiter Escape(Delimiter delimiter, string text)
        {
            if (delimiter.Position == 0)
                return delimiter;
            return text[delimiter.Position - 1] == '\\'
                       ? Length != 1 ? new Delimiter(fullTag.Substring(1), delimiter.Position + 1) : null
                       : delimiter;
        }

        public Token GetToken(Delimiter delimiter, string text)
        {
            if (delimiter.IsClosing)
                return null;
            var second = delimiter.Partner;
            var length = second.Position - delimiter.Position + Length;

            return new PairedTagToken(delimiter.Position, length, text.Substring(delimiter.Position, length), fullTag);
        }

        public bool IsValid(Delimiter delimiter, string text)
        {
            var position = delimiter.Position;
            var isLast = position == text.Length - Length;
            var isFirst = position == 0;
            if (isLast || isFirst)
                return true;
            var next = text[position + Length];
            var previous = text[position - 1];
            return !(next.IsLetterOrDigitOrSpecifiedChar(Tag) && previous.IsLetterOrDigitOrSpecifiedChar(Tag));
        }

        public bool IsValidAsOpening(Delimiter delimiter, string text)
        {
            var position = delimiter.Position;
            if (position == 0)
                return !char.IsWhiteSpace(text[position + Length]);
            if (position + Length == text.Length)
                return false;
            return char.IsWhiteSpace(text[position - 1]) && !char.IsWhiteSpace(text[position + Length]);
        }

        public bool IsValidClosing(Delimiter delimiter, string text)
        {
            var position = delimiter.Position;
            if (position == 0)
                return false;
            if (position + Length == text.Length)
                return !char.IsWhiteSpace(text[position - 1]);
            return !char.IsWhiteSpace(text[position - 1]) && char.IsWhiteSpace(text[position + Length]);
        }

        public Delimiter ProcessIncomingChar(int position, string text, out int amountOfSymbolsToSkip)
        {
            amountOfSymbolsToSkip = Length - 1;
            return new Delimiter(fullTag, position);
        }
    }
}
