namespace Markdown
{
    public class EscapeFinder
    {
        public const char EscapeSymbol = '\\';

        private static readonly HashSet<char> MarkupSymbols = [EscapeSymbol, UnderscoreFinder.UnderscoreSymbol];

        public ISet<int> FindEscapePositions(string text)
        {
            throw new NotImplementedException();
        }

        private static bool IsEscapingNextSymbol(string text, int position)
        {
            throw new NotImplementedException();
        }
    }
}
