using System;
using System.Collections.Generic;
using FluentAssertions;
using MarkdownTask;
using NUnit.Framework;

namespace MarkdownTaskTests.SerarchersTests
{
    public class SingleHighlightTagSearcherTests
    {
        private ITagSearcher searcher;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            searcher = new SingleHighlightTagSearcher();
        }

        [Test]
        public void Searcher_ShouldCorrectlyDefineSingleTag_Em(
            [ValueSource(nameof(CasesForSingleHighlightTag))]
            Tuple<string, List<Tag>> testCase)
        {
            var mdText = testCase.Item1;
            var expectedResult = testCase.Item2;

            var actualResult = searcher.SearchForTags(mdText);
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void Searcher_ShouldCorrectlyDefineMultipleTag_Em(
            [ValueSource(nameof(CasesForMultipleSingleHighlightTag))]
            Tuple<string, List<Tag>> testCase)
        {
            var mdText = testCase.Item1;
            var expectedResult = testCase.Item2;

            var actualResult = searcher.SearchForTags(mdText);
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        private static IEnumerable<Tuple<string, List<Tag>>> CasesForSingleHighlightTag()
        {
            yield return Tuple.Create("_text_", new List<Tag>
            {
                new Tag(0, 6, TagType.SingleHighlight)
            });

            yield return Tuple.Create("_te_xt", new List<Tag>
            {
                new Tag(0, 4, TagType.SingleHighlight)
            });

            yield return Tuple.Create("t_ex_t", new List<Tag>
            {
                new Tag(1, 4, TagType.SingleHighlight)
            });

            yield return Tuple.Create("te_xt_", new List<Tag>
            {
                new Tag(2, 4, TagType.SingleHighlight)
            });
        }

        private static IEnumerable<Tuple<string, List<Tag>>> CasesForMultipleSingleHighlightTag()
        {
            yield return Tuple.Create("_so_me te_xt_", new List<Tag>
            {
                new Tag(0, 4, TagType.SingleHighlight),
                new Tag(9, 4, TagType.SingleHighlight)
            });

            yield return Tuple.Create("_some_ _text_", new List<Tag>
            {
                new Tag(0, 6, TagType.SingleHighlight),
                new Tag(7, 6, TagType.SingleHighlight)
            });
        }
    }
}