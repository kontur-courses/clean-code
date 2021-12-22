using System;
using System.Collections.Generic;
using FluentAssertions;
using MarkdownTask;
using NUnit.Framework;

namespace MarkdownTaskTests.SearchersTests
{
    public class ItalicTagSearcherTests
    {
        private ITagSearcher searcher;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            searcher = new ItalicTagSearcher();
        }

        [TestCase("text")]
        [TestCase("")]
        [TestCase("text_")]
        [TestCase("_text")]
        public void Searcher_ShouldNotDefineAnyTags(string mdText)
        {
            var actualResult = searcher.SearchForTags(mdText);
            actualResult.Should().HaveCount(0);
        }

        [Test]
        public void Searcher_ShouldCorrectlyDefineSingleTag(
            [ValueSource(nameof(CasesForItalicTag))]
            Tuple<string, List<Tag>> testCase)
        {
            var mdText = testCase.Item1;
            var expectedResult = testCase.Item2;

            var actualResult = searcher.SearchForTags(mdText);
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void Searcher_ShouldCorrectlyDefineMultipleTag(
            [ValueSource(nameof(CasesForMultipleItalicTag))]
            Tuple<string, List<Tag>> testCase)
        {
            var mdText = testCase.Item1;
            var expectedResult = testCase.Item2;

            var actualResult = searcher.SearchForTags(mdText);
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void Searcher_ShouldNotDefineOpenedTag(
            [ValueSource(nameof(CasesForOpenedItalicTag))]
            Tuple<string, List<Tag>> testCase)
        {
            var mdText = testCase.Item1;
            var expectedResult = testCase.Item2;

            var actualResult = searcher.SearchForTags(mdText);
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        private static IEnumerable<Tuple<string, List<Tag>>> CasesForItalicTag()
        {
            yield return Tuple.Create("_text_", new List<Tag>
            {
                new Tag(0, 6, TagType.Italic)
            });

            yield return Tuple.Create("_te_xt", new List<Tag>
            {
                new Tag(0, 4, TagType.Italic)
            });

            yield return Tuple.Create("t_ex_t", new List<Tag>
            {
                new Tag(1, 4, TagType.Italic)
            });

            yield return Tuple.Create("te_xt_", new List<Tag>
            {
                new Tag(2, 4, TagType.Italic)
            });
        }

        private static IEnumerable<Tuple<string, List<Tag>>> CasesForMultipleItalicTag()
        {
            yield return Tuple.Create("_so_me te_xt_", new List<Tag>
            {
                new Tag(0, 4, TagType.Italic),
                new Tag(9, 4, TagType.Italic)
            });

            yield return Tuple.Create("_some_ _text_", new List<Tag>
            {
                new Tag(0, 6, TagType.Italic),
                new Tag(7, 6, TagType.Italic)
            });
        }

        private static IEnumerable<Tuple<string, List<Tag>>> CasesForOpenedItalicTag()
        {
            yield return Tuple.Create("_te_x_t", new List<Tag>
            {
                new Tag(0, 4, TagType.Italic)
            });

            yield return Tuple.Create("_some_ _text", new List<Tag>
            {
                new Tag(0, 6, TagType.Italic)
            });
        }
    }
}