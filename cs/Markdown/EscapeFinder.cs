namespace Markdown
{
    public class EscapeFinder
    {
        private const char EscapeSymbol = '\\';

        private static readonly HashSet<char> EscapableSymbols = [EscapeSymbol, UnderscoreFinder.UnderscoreSymbol];

        public ISet<int> Find(string text)
        {
            throw new NotImplementedException();
        }

        private static bool EscapesNextSymbol(string text, int position)
        {
            throw new NotImplementedException();
        }
    }
}
