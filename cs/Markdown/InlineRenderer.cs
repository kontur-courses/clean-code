namespace Markdown
{
    public class InlineRenderer
    {
        private readonly EscapeFinder escapeFinder = new EscapeFinder();
        private readonly UnderscoreFinder underscoreFinder = new UnderscoreFinder();
        private readonly PairFinder pairFinder = new PairFinder();

        public string Render(string text)
        {
            throw new NotImplementedException();
        }

        private static string BuildHtml(string text, IReadOnlyList<UnderscorePair> pairs, ISet<int> escapePositions)
        {
            throw new NotImplementedException();
        }

        private static Dictionary<int, UnderscorePair> GetPairsByPosition(IReadOnlyList<UnderscorePair> pairs)
        {
            throw new NotImplementedException();
        }
    }
}
