namespace Markdown
{
    public class UnderscoreFinder
    {
        public const char UnderscoreSymbol = '_';

        private const int MaxUnderscoreLength = 2;
        private const char TextBoundarySymbol = ' ';

        public IReadOnlyList<Underscore> Find(string text, ISet<int> escapePositions)
        {
            var underscores = new List<Underscore>();
            var wordIndex = 0;
            var position = 0;
            while (position < text.Length)
            {
                if (IsWhitespaceAfterWord(text, position))
                    wordIndex++;

                var length = CountUnescapedUnderscores(text, position, escapePositions);
                if (TryCreateUnderscore(text, position, length, wordIndex, out var underscore))
                    underscores.Add(underscore);

                position += GetStepLength(length);
            }

            return underscores;
        }

        private static bool IsWhitespaceAfterWord(string text, int position)
        {
            var previousSymbol = GetSymbol(text, position - 1);
            return char.IsWhiteSpace(text[position]) && !char.IsWhiteSpace(previousSymbol);
        }

        private static int CountUnescapedUnderscores(string text, int start, ISet<int> escapePositions)
        {
            var isEscaped = escapePositions.Contains(start - 1);
            return isEscaped ? 0 : CountUnderscores(text, start);
        }

        private static int GetStepLength(int underscoresLength)
        {
            return Math.Max(underscoresLength, 1);
        }

        private static int CountUnderscores(string text, int start)
        {
            var end = start;
            while (end < text.Length && text[end] == UnderscoreSymbol)
            {
                end++;
            }

            return end - start;
        }

        private static bool TryCreateUnderscore(
            string text,
            int position,
            int length,
            int wordIndex,
            out Underscore underscore)
        {
            underscore = null;
            if (IsNotMarkup(text, position, length))
                return false;

            underscore = new Underscore(
                position,
                length,
                CanOpen(text, position, length),
                CanClose(text, position),
                IsInsideWord(text, position, length),
                wordIndex);

            return true;
        }

        private static bool IsNotMarkup(string text, int position, int length)
        {
            var isMissing = length == 0;
            var isTooLong = length > MaxUnderscoreLength;
            return isMissing || isTooLong || IsInsideNumber(text, position, length);
        }

        private static bool IsInsideNumber(string text, int position, int length)
        {
            return IsNextToDigit(text, position, length);
        }

        private static bool CanOpen(string text, int position, int length)
        {
            var nextSymbol = GetSymbol(text, position + length);
            return !char.IsWhiteSpace(nextSymbol);
        }

        private static bool CanClose(string text, int position)
        {
            var previousSymbol = GetSymbol(text, position - 1);
            return !char.IsWhiteSpace(previousSymbol);
        }

        private static bool IsInsideWord(string text, int position, int length)
        {
            var previousSymbol = GetSymbol(text, position - 1);
            var nextSymbol = GetSymbol(text, position + length);
            return char.IsLetterOrDigit(previousSymbol) && char.IsLetterOrDigit(nextSymbol);
        }

        private static bool IsNextToDigit(string text, int position, int length)
        {
            var previousSymbol = GetSymbol(text, position - 1);
            var nextSymbol = GetSymbol(text, position + length);
            return char.IsDigit(previousSymbol) || char.IsDigit(nextSymbol);
        }

        private static char GetSymbol(string text, int position)
        {
            var isInsideText = position >= 0 && position < text.Length;
            return isInsideText ? text[position] : TextBoundarySymbol;
        }
    }
}
