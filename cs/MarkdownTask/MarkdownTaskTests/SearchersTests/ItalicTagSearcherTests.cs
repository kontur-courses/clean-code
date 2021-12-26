using System;
using System.Collections.Generic;
using FluentAssertions;
using MarkdownTask.Searchers;
using MarkdownTask.Styles;
using MarkdownTask.Tags;
using NUnit.Framework;

namespace MarkdownTaskTests.SearchersTests
{
    public class ItalicTagSearcherTests
    {
        private static readonly StyleInfo StyleInfo = MdStyleKeeper.Styles[TagType.Italic];
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
        [TestCase("__text__")]
        public void Searcher_ShouldNotDefineAnyTags(string mdText)
        {
            var actualResult = searcher.SearchForTags(mdText);
            actualResult.Should().HaveCount(0);
        }

        [TestCase("_ text_")]
        [TestCase("_text _")]
        public void Searcher_ShouldNotDefineTag_WhenSpaceBeforeOrAfterTag(string mdText)
        {
            var actualResult = searcher.SearchForTags(mdText);
            actualResult.Should().HaveCount(0);
        }

        [Test]
        public void Searcher_ShouldNotDefineTag_WhenTagInDifferentWords()
        {
            var mdText = "so_me te_xt";
            var actualResult = searcher.SearchForTags(mdText);
            actualResult.Should().HaveCount(0);
        }

        [TestCase("__")]
        [TestCase("___")]
        [TestCase("____")]
        public void Searcher_ShouldNotDefineTag_IfTagNotWrapSomething(string mdText)
        {
            var actualResult = searcher.SearchForTags(mdText);
            actualResult.Should().HaveCount(0);
        }

        [TestCase("text_1_")]
        [TestCase("text_1_2")]
        public void Searcher_ShouldNotDefineTag_IfTagWrapNumbers(string mdText)
        {
            var actualResult = searcher.SearchForTags(mdText);
            actualResult.Should().HaveCount(0);
        }

        [Test]
        public void Searcher_ShouldCorrectlyDefineTag_InDifferentPartsOfWord(
            [ValueSource(nameof(CasesForItalicTag))]
            Tuple<string, List<Tag>> testCase)
        {
            var mdText = testCase.Item1;
            var expectedResult = testCase.Item2;

            var actualResult = searcher.SearchForTags(mdText);
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void Searcher_ShouldCorrectlyDefineTags_WhenMoreThatOneTag(
            [ValueSource(nameof(CasesForMultipleItalicTag))]
            Tuple<string, List<Tag>> testCase)
        {
            var mdText = testCase.Item1;
            var expectedResult = testCase.Item2;

            var actualResult = searcher.SearchForTags(mdText);
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void Searcher_ShouldNotDefineTag_IfTagIsOpened(
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
                new Tag(0, 6, StyleInfo)
            });

            yield return Tuple.Create("_te_xt", new List<Tag>
            {
                new Tag(0, 4, StyleInfo)
            });

            yield return Tuple.Create("t_ex_t", new List<Tag>
            {
                new Tag(1, 4, StyleInfo)
            });

            yield return Tuple.Create("te_xt_", new List<Tag>
            {
                new Tag(2, 4, StyleInfo)
            });
        }

        private static IEnumerable<Tuple<string, List<Tag>>> CasesForMultipleItalicTag()
        {
            yield return Tuple.Create("_so_me te_xt_", new List<Tag>
            {
                new Tag(0, 4, StyleInfo),
                new Tag(9, 4, StyleInfo)
            });

            yield return Tuple.Create("_some_ _text_", new List<Tag>
            {
                new Tag(0, 6, StyleInfo),
                new Tag(7, 6, StyleInfo)
            });
        }

        private static IEnumerable<Tuple<string, List<Tag>>> CasesForOpenedItalicTag()
        {
            yield return Tuple.Create("_te_x_t", new List<Tag>
            {
                new Tag(0, 4, StyleInfo)
            });

            yield return Tuple.Create("_some_ _text", new List<Tag>
            {
                new Tag(0, 6, StyleInfo)
            });
        }
    }
}