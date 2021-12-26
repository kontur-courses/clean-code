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
        private static readonly StyleInfo ItalicStyleInfo = MdStyleKeeper.Styles[TagType.Italic];
        private static readonly StyleInfo StrongStyleInfo = MdStyleKeeper.Styles[TagType.Strong];
        private static readonly StyleInfo HeaderStyleInfo = MdStyleKeeper.Styles[TagType.Header];
        private EscapeSearcher escapeSearcher;

        private List<ITagSearcher> searchers;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            searchers = new List<ITagSearcher>
            {
                new HeaderTagSearcher(),
                new ItalicTagSearcher(),
                new StrongTagSearcher()
            };
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
            return searchers
                .SelectMany(searcher => searcher
                    .SearchForTags(mdText, escapedChars))
                .OrderBy(tag => tag.StartsAt)
                .ToList();
        }

        private static IEnumerable<Tuple<string, List<Tag>>> TestCasesForCompositeTest()
        {
            yield return Tuple.Create("_some_ __text__", new List<Tag>
            {
                new Tag(0, 6, ItalicStyleInfo),
                new Tag(7, 8, StrongStyleInfo)
            });

            yield return Tuple.Create("# __new__ _paragraph_", new List<Tag>
            {
                new Tag(0, 21, HeaderStyleInfo),
                new Tag(2, 7, StrongStyleInfo),
                new Tag(10, 11, ItalicStyleInfo)
            });

            yield return Tuple.Create("# __first__ paragraph\n\n# _second_ paragraph", new List<Tag>
            {
                new Tag(0, 21, HeaderStyleInfo),
                new Tag(2, 9, StrongStyleInfo),
                new Tag(23, 20, HeaderStyleInfo),
                new Tag(25, 8, ItalicStyleInfo)
            });
        }
    }
}