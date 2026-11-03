namespace Markdown
{
    public class PairFinder
    {
        public IReadOnlyList<UnderscorePair> Find(IReadOnlyList<Underscore> underscores)
        {
            throw new NotImplementedException();
        }

        private static List<UnderscorePair> MatchPairs(IReadOnlyList<Underscore> underscores)
        {
            throw new NotImplementedException();
        }

        private static bool CanBePaired(Underscore opening, Underscore closing)
        {
            throw new NotImplementedException();
        }

        private static List<UnderscorePair> RemoveIntersecting(List<UnderscorePair> pairs)
        {
            throw new NotImplementedException();
        }

        private static bool AreIntersecting(UnderscorePair first, UnderscorePair second)
        {
            throw new NotImplementedException();
        }

        private static List<UnderscorePair> RemoveDoubleInsideSingle(List<UnderscorePair> pairs)
        {
            throw new NotImplementedException();
        }

        private static bool IsInside(UnderscorePair inner, UnderscorePair outer)
        {
            throw new NotImplementedException();
        }
    }
}
