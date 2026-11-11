using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class UnderscoreFinderTests
    {
        [TestCase("")]
        [TestCase("abc")]
        [TestCase("a b")]
        public void Find_ShouldReturnEmpty_WhenTextHasNoUnderscores_Test(string text)
        {
            var underscores = FindWithoutEscapes(text);

            underscores
                .Should()
                .BeEmpty();
        }

        [TestCase("_a_", new[] { 0, 2 })]
        [TestCase("a _b_ c", new[] { 2, 4 })]
        [TestCase("__a__", new[] { 0, 3 })]
        public void Find_ShouldReturnPositions_WhenTextHasUnderscores_Test(string text, int[] expectedPositions)
        {
            var underscores = FindWithoutEscapes(text);

            underscores
                .Select(underscore => underscore.Position)
                .Should()
                .Equal(expectedPositions);
        }

        [TestCase("_a_", new[] { 1, 1 })]
        [TestCase("__a__", new[] { 2, 2 })]
        [TestCase("__a_", new[] { 2, 1 })]
        public void Find_ShouldJoinUnderscores_WhenTheyGoInRow_Test(string text, int[] expectedLengths)
        {
            var underscores = FindWithoutEscapes(text);

            underscores
                .Select(underscore => underscore.Length)
                .Should()
                .Equal(expectedLengths);
        }

        [TestCase("___")]
        [TestCase("____")]
        [TestCase("___a___")]
        public void Find_ShouldSkipUnderscores_WhenMoreThanTwoGoInRow_Test(string text)
        {
            var underscores = FindWithoutEscapes(text);

            underscores
                .Should()
                .BeEmpty();
        }

        [TestCase(@"\_a_", new[] { 0 }, new[] { 3 })]
        [TestCase(@"\__a_", new[] { 0 }, new[] { 2, 4 })]
        public void Find_ShouldSkipUnderscore_WhenItIsEscaped_Test(
            string text,
            int[] escapePositions,
            int[] expectedPositions)
        {
            var underscores = UnderscoreFinder.Find(text, escapePositions.ToHashSet());

            underscores
                .Select(underscore => underscore.Position)
                .Should()
                .Equal(expectedPositions);
        }

        [TestCase("цифрами_12_3")]
        [TestCase("a_1")]
        [TestCase("1_a")]
        public void Find_ShouldSkipUnderscore_WhenItIsInsideWordWithDigits_Test(string text)
        {
            var underscores = FindWithoutEscapes(text);

            underscores
                .Should()
                .BeEmpty();
        }

        [TestCase("_a", true)]
        [TestCase("a_b", true)]
        [TestCase("a_ b", false)]
        [TestCase("a_", false)]
        public void Find_ShouldAllowOpening_OnlyWhenNextSymbolIsNotWhitespace_Test(string text, bool expectedCanOpen)
        {
            var underscore = FindSingleWithoutEscapes(text);

            underscore.CanOpen
                .Should()
                .Be(expectedCanOpen);
        }

        [TestCase("a_", true)]
        [TestCase("a_b", true)]
        [TestCase("a _b", false)]
        [TestCase("_a", false)]
        public void Find_ShouldAllowClosing_OnlyWhenPreviousSymbolIsNotWhitespace_Test(
            string text,
            bool expectedCanClose)
        {
            var underscore = FindSingleWithoutEscapes(text);

            underscore.CanClose
                .Should()
                .Be(expectedCanClose);
        }

        [TestCase("a_b", true)]
        [TestCase("_a", false)]
        [TestCase("a_", false)]
        [TestCase("._a", false)]
        public void Find_ShouldMarkInsideWord_OnlyWhenLettersAreOnBothSides_Test(string text, bool expectedIsInsideWord)
        {
            var underscore = FindSingleWithoutEscapes(text);

            underscore.IsInsideWord
                .Should()
                .Be(expectedIsInsideWord);
        }

        [TestCase("_a_ _b_", new[] { 0, 0, 1, 1 })]
        [TestCase("a_b c_d", new[] { 0, 1 })]
        [TestCase("_a  b_", new[] { 0, 1 })]
        public void Find_ShouldNumberWords_WhenTheyAreSeparatedByWhitespace_Test(string text, int[] expectedWordIndexes)
        {
            var underscores = FindWithoutEscapes(text);

            underscores
                .Select(underscore => underscore.WordIndex)
                .Should()
                .Equal(expectedWordIndexes);
        }

        private static IReadOnlyList<Underscore> FindWithoutEscapes(string text)
        {
            return UnderscoreFinder.Find(text, new HashSet<int>());
        }

        private static Underscore FindSingleWithoutEscapes(string text)
        {
            return FindWithoutEscapes(text)
                .Single();
        }
    }
}
