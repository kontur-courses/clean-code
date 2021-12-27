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
        private static readonly TagStyleInfo TagStyleInfo = MdStyleKeeper.Styles[TagType.Header];
        private EscapeSearcher escapeSearcher;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            escapeSearcher = new EscapeSearcher();
        }

        [TestCase("")]
        [TestCase("#text")]
        [TestCase("text# ")]
        [TestCase("some # text")]
        public void Searcher_ShouldNotDefineAnyTags(string mdText)
        {
            var searcher = new HeaderTagSearcher(mdText);
            var escapedChars = escapeSearcher.GetPositionOfEscapingSlashes(mdText);

            var actualResult = searcher.SearchForTags(escapedChars);
            actualResult.Should().HaveCount(0);
        }

        [Test]
        public void Searcher_ShouldCorrectlyDefineTags(
            [ValueSource(nameof(CasesForHeaderTag))]
            Tuple<string, List<Tag>> testCase)
        {
            var mdText = testCase.Item1;
            var expectedResult = testCase.Item2;
            var searcher = new HeaderTagSearcher(mdText);
            var escapedChars = escapeSearcher.GetPositionOfEscapingSlashes(mdText);

            var actualResult = searcher.SearchForTags(escapedChars);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        private static IEnumerable<Tuple<string, List<Tag>>> CasesForHeaderTag()
        {
            yield return Tuple.Create("# text", new List<Tag>
            {
                new Tag(0, 6, TagStyleInfo)
            });

            yield return Tuple.Create("# some\n\n# text", new List<Tag>
            {
                new Tag(0, 6, TagStyleInfo),
                new Tag(8, 6, TagStyleInfo)
            });
        }
    }
}