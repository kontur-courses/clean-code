using System;
using System.Collections.Generic;
using FluentAssertions;
using MarkdownTask.Searchers;
using MarkdownTask.Styles;
using MarkdownTask.Tags;
using NUnit.Framework;

namespace MarkdownTaskTests.SearchersTests
{
    public class HeaderTagSearcherTests
    {
        private static readonly StyleInfo StyleInfo = MdStyleKeeper.Styles[TagType.Header];
        private ITagSearcher searcher;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            searcher = new HeaderTagSearcher();
        }

        [TestCase("")]
        [TestCase("#text")]
        [TestCase("text# ")]
        [TestCase("some # text")]
        public void Searcher_ShouldNotDefineAnyTags(string mdText)
        {
            var actualResult = searcher.SearchForTags(mdText);
            actualResult.Should().HaveCount(0);
        }

        [Test]
        public void Searcher_ShouldCorrectlyDefineTags(
            [ValueSource(nameof(CasesForHeaderTag))]
            Tuple<string, List<Tag>> testCase)
        {
            var mdText = testCase.Item1;
            var expectedResult = testCase.Item2;

            var actualResult = searcher.SearchForTags(mdText);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        private static IEnumerable<Tuple<string, List<Tag>>> CasesForHeaderTag()
        {
            yield return Tuple.Create("# text", new List<Tag>
            {
                new Tag(0, 6, StyleInfo)
            });

            yield return Tuple.Create("# some\n\n# text", new List<Tag>
            {
                new Tag(0, 6, StyleInfo),
                new Tag(8, 6, StyleInfo)
            });
        }
    }
}