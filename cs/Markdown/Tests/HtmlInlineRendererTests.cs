using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class HtmlInlineRendererTests
    {
        [TestCase("")]
        [TestCase("abc")]
        [TestCase("a b")]
        public void Render_ShouldReturnSameText_WhenTextHasNoMarkup_Test(string text)
        {
            var html = HtmlInlineRenderer.Render(text);

            html
                .Should()
                .Be(text);
        }

        [TestCaseSource(nameof(TagCases))]
        public void Render_ShouldWrapInTag_WhenTextIsInsideUnderscores_Test(string text, string expectedHtml)
        {
            var html = HtmlInlineRenderer.Render(text);

            html
                .Should()
                .Be(expectedHtml);
        }

        [TestCaseSource(nameof(PartOfWordCases))]
        public void Render_ShouldWrapPartOfWord_WhenUnderscoresAreInSameWord_Test(string text, string expectedHtml)
        {
            var html = HtmlInlineRenderer.Render(text);

            html
                .Should()
                .Be(expectedHtml);
        }

        [TestCaseSource(nameof(NestedTagCases))]
        public void Render_ShouldNestTags_OnlyWhenSingleIsInsideDouble_Test(string text, string expectedHtml)
        {
            var html = HtmlInlineRenderer.Render(text);

            html
                .Should()
                .Be(expectedHtml);
        }

        [TestCaseSource(nameof(EscapeCases))]
        public void Render_ShouldRemoveBackslash_OnlyWhenItEscapesMarkup_Test(string text, string expectedHtml)
        {
            var html = HtmlInlineRenderer.Render(text);

            html
                .Should()
                .Be(expectedHtml);
        }

        [TestCase("_a")]
        [TestCase("_a__")]
        [TestCase("__a_")]
        [TestCase("_ a_")]
        [TestCase("_a _")]
        [TestCase("a_ b_")]
        public void Render_ShouldKeepUnderscores_WhenTheyHaveNoPair_Test(string text)
        {
            var html = HtmlInlineRenderer.Render(text);

            html
                .Should()
                .Be(text);
        }

        [TestCase("___")]
        [TestCase("____")]
        [TestCase("___a___")]
        [TestCase("цифрами_12_3")]
        [TestCase("a_1 b_2")]
        public void Render_ShouldKeepUnderscores_WhenTheyCanNotBeMarkup_Test(string text)
        {
            var html = HtmlInlineRenderer.Render(text);

            html
                .Should()
                .Be(text);
        }

        [TestCase("ра_зных сл_овах")]
        [TestCase("a_b c_d")]
        [TestCase("_a b_c")]
        public void Render_ShouldKeepUnderscores_WhenTheyAreInsideDifferentWords_Test(string text)
        {
            var html = HtmlInlineRenderer.Render(text);

            html
                .Should()
                .Be(text);
        }

        [TestCase("__a _b__ c_")]
        [TestCase("_a __b_ c__")]
        [TestCase("__пересечения _двойных__ и одинарных_")]
        [TestCase("__a _b__ c")]
        public void Render_ShouldKeepUnderscores_WhenTagsIntersect_Test(string text)
        {
            var html = HtmlInlineRenderer.Render(text);

            html
                .Should()
                .Be(text);
        }

        [TestCase("_a __b__ c")]
        public void Render_ShouldKeepDoubleUnderscores_WhenTheyAreAfterOpenedSingle_Test(string text)
        {
            var html = HtmlInlineRenderer.Render(text);

            html
                .Should()
                .Be(text);
        }

        private static IEnumerable<TestCaseData> TagCases()
        {
            yield return new TestCaseData("_a_", "<em>a</em>");
            yield return new TestCaseData("__a__", "<strong>a</strong>");
            yield return new TestCaseData("_a_ _b_", "<em>a</em> <em>b</em>");
            yield return new TestCaseData("_a  b_", "<em>a  b</em>");
            yield return new TestCaseData("_12_", "<em>12</em>");
            yield return new TestCaseData(
                "Текст, _окруженный с двух сторон_ одинарными",
                "Текст, <em>окруженный с двух сторон</em> одинарными");
            yield return new TestCaseData(
                "__Выделенный двумя символами текст__",
                "<strong>Выделенный двумя символами текст</strong>");
        }

        private static IEnumerable<TestCaseData> PartOfWordCases()
        {
            yield return new TestCaseData("_нач_але", "<em>нач</em>але");
            yield return new TestCaseData("сер_еди_не", "сер<em>еди</em>не");
            yield return new TestCaseData("кон_це._", "кон<em>це.</em>");
        }

        private static IEnumerable<TestCaseData> NestedTagCases()
        {
            yield return new TestCaseData("__a _b_ c__", "<strong>a <em>b</em> c</strong>");
            yield return new TestCaseData("_a _b_ c_", "<em>a <em>b</em> c</em>");
            yield return new TestCaseData("_a __b__ c_", "<em>a __b__ c</em>");
            yield return new TestCaseData(
                "Внутри __двойного выделения _одинарное_ тоже__ работает.",
                "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.");
            yield return new TestCaseData(
                "Но не наоборот — внутри _одинарного __двойное__ не_ работает.",
                "Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает.");
        }

        private static IEnumerable<TestCaseData> EscapeCases()
        {
            yield return new TestCaseData(@"\_a\_", "_a_");
            yield return new TestCaseData(@"a\\b", @"a\b");
            yield return new TestCaseData(@"\\_a_", @"\<em>a</em>");
            yield return new TestCaseData(@"\\\_", @"\_");
            yield return new TestCaseData(@"\__a_", "_<em>a</em>");
            yield return new TestCaseData(@"сим\волы\", @"сим\волы\");
            yield return new TestCaseData(@"\#", @"\#");
        }
    }
}
