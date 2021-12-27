using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MarkdownTask.Searchers;
using MarkdownTask.Styles;
using MarkdownTask.Tags;
using NUnit.Framework;

namespace MarkdownTaskTests.SearchersTests
{
    public class SearchersCompositionTests
    {
        private static readonly TagStyleInfo ItalicTagStyleInfo = MdStyleKeeper.Styles[TagType.Italic];
        private static readonly TagStyleInfo StrongTagStyleInfo = MdStyleKeeper.Styles[TagType.Strong];
        private static readonly TagStyleInfo HeaderTagStyleInfo = MdStyleKeeper.Styles[TagType.Header];
        private EscapeSearcher escapeSearcher;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            escapeSearcher = new EscapeSearcher();
        }

        [Test]
        public void CompositionTest(
            [ValueSource(nameof(TestCasesForCompositeTest))]
            Tuple<string, List<Tag>> testCases)
        {
            var mdText = testCases.Item1;
            var expectedResult = testCases.Item2;

            var actualResult = GetTagsComposition(mdText);

            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        private List<Tag> GetTagsComposition(string mdText)
        {
            var escapedChars = escapeSearcher.GetPositionOfEscapingSlashes(mdText);
            var searchers = GetSearchers(mdText);
            return searchers
                .SelectMany(searcher => searcher
                    .SearchForTags(escapedChars))
                .OrderBy(tag => tag.StartsAt)
                .ToList();
        }

        private ITagSearcher[] GetSearchers(string mdText)
        {
            return new ITagSearcher[]
            {
                new HeaderTagSearcher(mdText),
                new ItalicTagSearcher(mdText),
                new StrongTagSearcher(mdText)
            };
        }

        private static IEnumerable<Tuple<string, List<Tag>>> TestCasesForCompositeTest()
        {
            yield return Tuple.Create("_some_ __text__", new List<Tag>
            {
                new Tag(0, 6, ItalicTagStyleInfo),
                new Tag(7, 8, StrongTagStyleInfo)
            });

            yield return Tuple.Create("# __new__ _paragraph_", new List<Tag>
            {
                new Tag(0, 21, HeaderTagStyleInfo),
                new Tag(2, 7, StrongTagStyleInfo),
                new Tag(10, 11, ItalicTagStyleInfo)
            });

            yield return Tuple.Create("# __first__ paragraph\n\n# _second_ paragraph", new List<Tag>
            {
                new Tag(0, 21, HeaderTagStyleInfo),
                new Tag(2, 9, StrongTagStyleInfo),
                new Tag(23, 20, HeaderTagStyleInfo),
                new Tag(25, 8, ItalicTagStyleInfo)
            });
        }
    }
}