using System.Globalization;
using System.Linq;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Infrastructure.Tokenization.Workers
{
    public static class TokenPositionHelpers
    {
        public static bool InsideWord(this TokenPosition source) =>
            source.HasAllFlags(TokenPosition.BeforeWord, TokenPosition.AfterWord);

        public static bool InsideDigit(this TokenPosition source) =>
            source.HasAllFlags(TokenPosition.AfterDigit, TokenPosition.BeforeDigit);

        public static bool AtDigitBegin(this TokenPosition source) =>
            source.HasAllFlags(TokenPosition.BeforeDigit);

        public static bool AtDigitEnd(this TokenPosition source) =>
            source.HasAllFlags(TokenPosition.AfterDigit);

        public static bool OnDigitBorder(this TokenPosition source) =>
            source.AtDigitBegin() || source.AtDigitEnd();

        public static bool WhitespaceFramed(this TokenPosition source) =>
            source.AfterWhitespace() && source.BeforeWhitespace();

        public static bool AtWordBegin(this TokenPosition source) =>
            source.HasAllFlags(TokenPosition.BeforeWord, TokenPosition.AfterWhitespace);

        public static bool AtWordEnd(this TokenPosition source) =>
            source.HasAllFlags(TokenPosition.AfterWord, TokenPosition.BeforeWhitespace);

        public static bool OnWordBorder(this TokenPosition source) =>
            source.AtWordBegin() || source.AtWordEnd();

        public static bool AfterWhitespace(this TokenPosition source) =>
            source.HasFlag(TokenPosition.AfterWhitespace);

        public static bool BeforeWhitespace(this TokenPosition source) =>
            source.HasFlag(TokenPosition.BeforeWhitespace);

        public static bool OnParagraphStart(this TokenPosition source) =>
            source.HasFlag(TokenPosition.ParagraphStart);

        public static bool HasAllFlags(this TokenPosition source, params TokenPosition[] flags) =>
            flags.All(f => source.HasFlag(f));
        
        public static bool HasAnyFlag(this TokenPosition source, params TokenPosition[] flags) =>
            flags.Any(f => source.HasFlag(f));

        public static TokenPosition GetPosition(string raw, int startIndex, string tokenSymbol) =>
            GetPosition(raw, startIndex, tokenSymbol.Length);

        public static TokenPosition GetPosition(string rawInput, int currentIndex, int symbolLength)
        {
            var nextIndex = currentIndex + symbolLength;
            var next = nextIndex >= rawInput.Length
                ? (char?) null
                : rawInput[nextIndex];

            var previous = currentIndex == 0
                ? (char?) null
                : rawInput[currentIndex - 1];

            return GetPosition(previous, next);
        }

        public static TokenPosition GetPosition(char? previous, char? next)
        {
            TokenPosition position = default;
            position |= next.HasValue
                ? GetNextCharFlags(next.Value)
                : TokenPosition.ParagraphEnd;

            position |= previous.HasValue
                ? GetPreviousCharFlags(previous.Value)
                : TokenPosition.ParagraphStart;
            return position;
        }

        public static TokenPosition GetNextCharFlags(char next)
        {
            TokenPosition result = default;
            if (char.IsWhiteSpace(next))
                result |= TokenPosition.BeforeWhitespace;
            if (char.IsDigit(next))
                result |= TokenPosition.BeforeDigit;
            if (char.IsLetter(next))
                result |= TokenPosition.BeforeWord;

            var category = char.GetUnicodeCategory(next);
            if (category == UnicodeCategory.LineSeparator || category == UnicodeCategory.ParagraphSeparator)
                result |= TokenPosition.ParagraphEnd;

            return result;
        }

        public static TokenPosition GetPreviousCharFlags(char previous)
        {
            TokenPosition result = default;
            if (char.IsWhiteSpace(previous))
                result |= TokenPosition.AfterWhitespace;
            if (char.IsDigit(previous))
                result |= TokenPosition.AfterDigit;
            if (char.IsLetter(previous))
                result |= TokenPosition.AfterWord;

            var category = char.GetUnicodeCategory(previous);
            if (category == UnicodeCategory.LineSeparator || category == UnicodeCategory.ParagraphSeparator)
                result |= TokenPosition.ParagraphStart;

            return result;
        }
    }
}