using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class PairFinderTests
    {
        [TestCase("_a_", new[] { 0, 2 })]
        [TestCase("__a__", new[] { 0, 3 })]
        [TestCase("_a_ _b_", new[] { 0, 2, 4, 6 })]
        [TestCase("__a _b_ c__", new[] { 0, 9, 4, 6 })]
        public void Find_ShouldPairUnderscores_WhenTheyHaveSameLength_Test(string text, int[] expectedPositions)
        {
            var pairs = FindPairs(text);

            ToPositions(pairs)
                .Should()
                .Equal(expectedPositions);
        }

        [TestCase("_a")]
        [TestCase("_a__")]
        [TestCase("__a_")]
        [TestCase("a_ b_")]
        public void Find_ShouldReturnEmpty_WhenUnderscoresHaveNoPair_Test(string text)
        {
            var pairs = FindPairs(text);

            pairs
                .Should()
                .BeEmpty();
        }

        [TestCase("_ a_")]
        [TestCase("_a _b")]
        public void Find_ShouldReturnEmpty_WhenWhitespaceIsInsideUnderscores_Test(string text)
        {
            var pairs = FindPairs(text);

            pairs
                .Should()
                .BeEmpty();
        }

        [TestCase("_нач_але", new[] { 0, 4 })]
        [TestCase("сер_еди_не", new[] { 3, 7 })]
        public void Find_ShouldPairUnderscores_WhenTheyAreInSameWord_Test(string text, int[] expectedPositions)
        {
            var pairs = FindPairs(text);

            ToPositions(pairs)
                .Should()
                .Equal(expectedPositions);
        }

        [TestCase("ра_зных сл_овах")]
        [TestCase("a_b c_d")]
        [TestCase("_a b_c")]
        public void Find_ShouldReturnEmpty_WhenUnderscoresInsideWordsAreInDifferentWords_Test(string text)
        {
            var pairs = FindPairs(text);

            pairs
                .Should()
                .BeEmpty();
        }

        [Test]
        public void Find_ShouldPairNearestUnderscores_WhenSameLengthUnderscoresAreNested_Test()
        {
            var pairs = FindPairs("_a _b_ c_");

            ToPositions(pairs)
                .Should()
                .Equal(0, 8, 3, 5);
        }

        [TestCase("__a _b__ c_")]
        [TestCase("_a __b_ c__")]
        [TestCase("__пересечения _двойных__ и одинарных_")]
        public void Find_ShouldReturnEmpty_WhenSingleAndDoublePairsIntersect_Test(string text)
        {
            var pairs = FindPairs(text);

            pairs
                .Should()
                .BeEmpty();
        }

        [TestCase("_a __b__ c_", new[] { 0, 10 })]
        [TestCase("_одинарного __двойное__ не_", new[] { 0, 26 })]
        public void Find_ShouldSkipDoublePair_WhenItIsInsideSinglePair_Test(string text, int[] expectedPositions)
        {
            var pairs = FindPairs(text);

            ToPositions(pairs)
                .Should()
                .Equal(expectedPositions);
        }

        private static IReadOnlyList<UnderscorePair> FindPairs(string text)
        {
            var underscores = UnderscoreFinder.Find(text, new HashSet<int>());
            return PairFinder.Find(underscores);
        }

        private static IEnumerable<int> ToPositions(IEnumerable<UnderscorePair> pairs)
        {
            return pairs
                .OrderBy(pair => pair.Start)
                .SelectMany(pair => new[] { pair.Start, pair.End });
        }
    }
}
