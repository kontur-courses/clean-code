namespace Markdown
{
    public class InlineRenderer
    {
        private readonly EscapeFinder escapeFinder = new ();
        private readonly UnderscoreFinder underscoreFinder = new ();
        private readonly PairFinder pairFinder = new ();

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
