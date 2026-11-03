namespace Markdown
{
    public class UnderscoreFinder
    {
        public const char UnderscoreSymbol = '_';

        private const int MaxUnderscoreLength = 2;

        public IReadOnlyList<Underscore> Find(string text, ISet<int> escapePositions)
        {
            throw new NotImplementedException();
        }

        private static bool IsEscaped(int position, ISet<int> escapePositions)
        {
            throw new NotImplementedException();
        }

        private static int CountUnderscores(string text, int start)
        {
            throw new NotImplementedException();
        }

        private static bool CanOpen(string text, int position, int length)
        {
            throw new NotImplementedException();
        }

        private static bool CanClose(string text, int position)
        {
            throw new NotImplementedException();
        }

        private static bool IsInsideWord(string text, int position, int length)
        {
            throw new NotImplementedException();
        }

        private static bool IsNextToDigit(string text, int position, int length)
        {
            throw new NotImplementedException();
        }
    }
}
