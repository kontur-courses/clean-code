using System.Globalization;
using System.Linq;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Infrastructure.Tokenization
{
    public static class TokenHelpers
    {
        public static bool InsideWord(this TokenPosition source) =>
            source.HasFlags(TokenPosition.BeforeWord, TokenPosition.AfterWord);

        public static bool InsideDigit(this TokenPosition source) =>
            source.HasFlags(TokenPosition.AfterDigit, TokenPosition.BeforeDigit);
        
        public static bool AtDigitBegin(this TokenPosition source) =>
            source.HasFlags(TokenPosition.BeforeDigit);

        public static bool AtDigitEnd(this TokenPosition source) =>
            source.HasFlags(TokenPosition.AfterDigit);

        public static bool OnDigitBorder(this TokenPosition source) =>
            source.AtDigitBegin() || source.AtDigitEnd();

        public static bool WhitespaceFramed(this TokenPosition source) =>
            source.HasFlags(TokenPosition.AfterWhitespace, TokenPosition.BeforeWhitespace);

        public static bool AtWordBegin(this TokenPosition source) =>
            source.HasFlags(TokenPosition.BeforeWord, TokenPosition.AfterWhitespace);

        public static bool AtWordEnd(this TokenPosition source) =>
            source.HasFlags(TokenPosition.AfterWord, TokenPosition.BeforeWhitespace);

        public static bool OnWordBorder(this TokenPosition source) =>
            source.AtWordBegin() || source.AtWordEnd();

        public static bool HasFlags(this TokenPosition source, params TokenPosition[] flags) =>
            flags.All(f => source.HasFlag(f));

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

        private static TokenPosition GetNextCharFlags(char next)
        {
            TokenPosition result = default;
            if (char.IsWhiteSpace(next))
                result |= TokenPosition.BeforeWhitespace;
            if (char.IsDigit(next))
                result |= TokenPosition.BeforeDigit;
            if (char.IsLetter(next))
                result |= TokenPosition.BeforeWord;

            var nextCategory = char.GetUnicodeCategory(next);
            if (nextCategory == UnicodeCategory.LineSeparator || nextCategory == UnicodeCategory.ParagraphSeparator)
                result |= TokenPosition.ParagraphEnd;

            return result;
        }

        private static TokenPosition GetPreviousCharFlags(char previous)
        {
            TokenPosition result = default;
            if (char.IsWhiteSpace(previous))
                result |= TokenPosition.AfterWhitespace;
            if (char.IsDigit(previous))
                result |= TokenPosition.AfterDigit;
            if (char.IsLetter(previous))
                result |= TokenPosition.AfterWord;

            var nextCategory = char.GetUnicodeCategory(previous);
            if (nextCategory == UnicodeCategory.LineSeparator || nextCategory == UnicodeCategory.ParagraphSeparator)
                result |= TokenPosition.ParagraphStart;

            return result;
        }
    }
}