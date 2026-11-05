namespace Markdown
{
    public class EscapeFinder
    {
        public const char EscapeSymbol = '\\';

        private const int EscapeSequenceLength = 2;

        private static readonly HashSet<char> MarkupSymbols = [EscapeSymbol, UnderscoreFinder.UnderscoreSymbol];

        public ISet<int> FindEscapePositions(string text)
        {
            var escapePositions = new HashSet<int>();
            var position = 0;
            while (position < text.Length)
            {
                if (IsEscapingNextSymbol(text, position))
                {
                    escapePositions.Add(position);
                    position += EscapeSequenceLength;
                }
                else
                {
                    position++;
                }
            }

            return escapePositions;
        }

        private static bool IsEscapingNextSymbol(string text, int position)
        {
            var nextPosition = position + 1;
            var isLastSymbol = nextPosition == text.Length;
            if (isLastSymbol)
                return false;

            return text[position] == EscapeSymbol && MarkupSymbols.Contains(text[nextPosition]);
        }
    }
}
