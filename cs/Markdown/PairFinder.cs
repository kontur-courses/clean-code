namespace Markdown
{
    public class PairFinder
    {
        public IReadOnlyList<UnderscorePair> Find(IReadOnlyList<Underscore> underscores)
        {
            var pairs = MatchPairs(underscores);
            var pairsWithoutIntersections = RemoveIntersecting(pairs);
            return RemoveDoubleInsideSingle(pairsWithoutIntersections);
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
            var sortedPairs = SortByOpening(pairs);
            var intersectingPairs = new HashSet<UnderscorePair>();
            var openPairs = new Stack<UnderscorePair>();
            foreach (var pair in sortedPairs)
            {
                while (openPairs.Count > 0 && openPairs.Peek().Closing.Position < pair.Opening.Position)
                {
                    openPairs.Pop();
                }

                if (openPairs.Count > 0 && AreIntersecting(openPairs.Peek(), pair))
                {
                    intersectingPairs.Add(openPairs.Peek());
                    intersectingPairs.Add(pair);
                }

                openPairs.Push(pair);
            }

            return sortedPairs
                .Where(pair => !intersectingPairs.Contains(pair))
                .ToList();
        }

        private static bool AreIntersecting(UnderscorePair first, UnderscorePair second)
        {
            var startsInsideFirst = first.Opening.Position < second.Opening.Position
                                    && second.Opening.Position < first.Closing.Position;
            var endsAfterFirst = first.Closing.Position < second.Closing.Position;
            return startsInsideFirst && endsAfterFirst;
        }

        private static List<UnderscorePair> RemoveDoubleInsideSingle(List<UnderscorePair> pairs)
        {
            var allowedPairs = new List<UnderscorePair>();
            var openSinglePairs = new Stack<UnderscorePair>();
            foreach (var pair in SortByOpening(pairs))
            {
                while (openSinglePairs.Count > 0 && openSinglePairs.Peek().Closing.Position < pair.Opening.Position)
                {
                    openSinglePairs.Pop();
                }

                var isInsideSingle = openSinglePairs.Count > 0 && IsInside(pair, openSinglePairs.Peek());
                var isDoubleInsideSingle = isInsideSingle && !pair.Opening.IsSingle;
                if (isDoubleInsideSingle)
                    continue;

                if (pair.Opening.IsSingle)
                    openSinglePairs.Push(pair);

                allowedPairs.Add(pair);
            }

            return allowedPairs;
        }

        private static bool IsInside(UnderscorePair inner, UnderscorePair outer)
        {
            return outer.Opening.Position < inner.Opening.Position
                   && inner.Closing.Position < outer.Closing.Position;
        }

        private static List<UnderscorePair> SortByOpening(IEnumerable<UnderscorePair> pairs)
        {
            return pairs
                .OrderBy(pair => pair.Opening.Position)
                .ToList();
        }
    }
}
