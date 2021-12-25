using System;
using System.Collections.Generic;
using FluentAssertions;
using MarkdownTask;
using NUnit.Framework;

namespace MarkdownTaskTests.SearchersTests
{
    public class EscapeSearcherTests
    {
        private EscapeSearcher searcher;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            searcher = new EscapeSearcher();
        }

        [Test]
        public void Searcher_ShouldCorrectlyDefineEscapedChars(
            [ValueSource(nameof(EscapeSearcherCases))]
            Tuple<string, List<int>> testCases)
        {
            var mdText = testCases.Item1;
            var expectedResult = testCases.Item2;

            var actualResult = searcher.GetPositionOfEscapingSlashes(mdText);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        private static IEnumerable<Tuple<string, List<int>>> EscapeSearcherCases()
        {
            yield return Tuple.Create("text", new List<int>());
            yield return Tuple.Create(@"\t\e\x\t", new List<int>());
            yield return Tuple.Create(@"\_text\_", new List<int> { 0, 6 });
            yield return Tuple.Create(@"\\_text_\", new List<int> { 0 });
            yield return Tuple.Create(@"some \ text", new List<int>());
        }
    }
}