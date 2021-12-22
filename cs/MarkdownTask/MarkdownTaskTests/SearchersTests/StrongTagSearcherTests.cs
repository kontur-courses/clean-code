using System;
using System.Collections.Generic;
using FluentAssertions;
using MarkdownTask;
using NUnit.Framework;

namespace MarkdownTaskTests.SearchersTests
{
    public class StrongTagSearcherTests
    {
        private ITagSearcher searcher;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            searcher = new StrongTagSearcher();
        }

        [Test]
        public void Searcher_ShouldCorrectlyDefineSingleTag(
            [ValueSource(nameof(CasesForStrongTag))]
            Tuple<string, List<Tag>> testCase)
        {
            var mdText = testCase.Item1;
            var expectedResult = testCase.Item2;

            var actualResult = searcher.SearchForTags(mdText);
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void Searcher_ShouldCorrectlyDefineMultipleTag(
            [ValueSource(nameof(CasesForMultipleStrongTag))]
            Tuple<string, List<Tag>> testCase)
        {
            var mdText = testCase.Item1;
            var expectedResult = testCase.Item2;

            var actualResult = searcher.SearchForTags(mdText);
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        private static IEnumerable<Tuple<string, List<Tag>>> CasesForStrongTag()
        {
            yield return Tuple.Create("__text__", new List<Tag>
            {
                new Tag(0, 8, TagType.Strong)
            });

            yield return Tuple.Create("__te__xt", new List<Tag>
            {
                new Tag(0, 6, TagType.Strong)
            });

            yield return Tuple.Create("t__ex__t", new List<Tag>
            {
                new Tag(1, 6, TagType.Strong)
            });

            yield return Tuple.Create("te__xt__", new List<Tag>
            {
                new Tag(2, 6, TagType.Strong)
            });
        }

        private static IEnumerable<Tuple<string, List<Tag>>> CasesForMultipleStrongTag()
        {
            yield return Tuple.Create("__so__me te__xt__", new List<Tag>
            {
                new Tag(0, 6, TagType.Strong),
                new Tag(11, 6, TagType.Strong)
            });

            yield return Tuple.Create("__some__ __text__", new List<Tag>
            {
                new Tag(0, 8, TagType.Strong),
                new Tag(9, 8, TagType.Strong)
            });
        }
    }
}