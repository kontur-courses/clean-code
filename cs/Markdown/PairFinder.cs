namespace Markdown
{
    public class PairFinder
    {
        public IReadOnlyList<UnderscorePair> Find(IReadOnlyList<Underscore> underscores)
        {
            return MatchPairs(underscores);
        }

        private static List<UnderscorePair> MatchPairs(IReadOnlyList<Underscore> underscores)
        {
            var pairs = new List<UnderscorePair>();
            var underscoresByLength = underscores.GroupBy(underscore => underscore.Length);
            foreach (var sameLengthUnderscores in underscoresByLength)
            {
                pairs.AddRange(MatchSameLength(sameLengthUnderscores));
            }

            return pairs;
        }

        private static List<UnderscorePair> MatchSameLength(IEnumerable<Underscore> underscores)
        {
            var pairs = new List<UnderscorePair>();
            var openings = new Stack<Underscore>();
            foreach (var underscore in underscores)
            {
                if (openings.Count > 0 && CanBePaired(openings.Peek(), underscore))
                    pairs.Add(new UnderscorePair(openings.Pop(), underscore));
                else if (underscore.CanOpen)
                    openings.Push(underscore);
            }

            return pairs;
        }

        private static bool CanBePaired(Underscore opening, Underscore closing)
        {
            var isInSameWord = opening.WordIndex == closing.WordIndex;
            return closing.CanClose && (isInSameWord || !opening.IsInsideWord);
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
