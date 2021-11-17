using System.Globalization;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.Parser
{
    internal static class StyleTokenExtensions
    {
        internal static bool IsTokenPlacedCorrectly(this StyleToken token, string text)
        {
            return !(token.IsInsideTextWithDigits(text) || token.IsInsideDifferentWords(text));
        }

        private static bool IsInsideDifferentWords(this StyleToken token, string text)
        {
            var openInsideWord = IsSeparatorInsideWord(token.OpenIndex, token.Separator.Length, text);
            var closeInsideWord = IsSeparatorInsideWord(token.CloseIndex, token.Separator.Length, text);

            var tokenContent = text
                .Substring(token.OpenIndex + token.Separator.Length, token.CloseIndex - token.OpenIndex - 1);

            return openInsideWord || closeInsideWord && tokenContent.Any(x => x == ' ');
        }

        private static bool IsInsideTextWithDigits(this StyleToken token, string text)
        {
            var openInsideWord = IsSeparatorInsideTextWithDigits(token.OpenIndex, token.Separator.Length, text);
            var closeInsideWord = IsSeparatorInsideTextWithDigits(token.CloseIndex, token.Separator.Length, text);

            var tokenContent = text
                .Substring(token.OpenIndex + token.Separator.Length, token.CloseIndex - token.OpenIndex - 1);

            return openInsideWord || closeInsideWord && tokenContent.Any(x => x == ' ');
        }

        private static bool IsSeparatorInsideWord(int index, int separatorLength, string text)
        {
            var isLeftLetter = index > 0 && char.IsLetter(text[index - 1]);

            var isRightLetter = index + separatorLength < text.Length - 1 &&
                                char.IsLetter(text[index + separatorLength]);

            return isLeftLetter && isRightLetter;
        }

        private static bool IsSeparatorInsideTextWithDigits(int index, int separatorLength, string text)
        {
            var isLeftLetter = index > 0 && char.IsLetterOrDigit(text[index - 1]);

            var isRightLetter = index + separatorLength < text.Length - 1 &&
                                char.IsLetterOrDigit(text[index + separatorLength]);

            return isLeftLetter && isRightLetter;
        }
    }
}