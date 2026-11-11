using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class InlineRendererTests
    {
        private readonly InlineRenderer inlineRenderer = new ();

        [TestCase("")]
        [TestCase("abc")]
        public void Render_ShouldReturnSameText_WhenTextHasNoMarkup_Test(string text)
        {
            var html = inlineRenderer.Render(text);

            html
                .Should()
                .Be(text);
        }

        [TestCase("_a_", "<em>a</em>")]
        [TestCase(
            "Текст, _окруженный с двух сторон_ одинарными",
            "Текст, <em>окруженный с двух сторон</em> одинарными")]
        [TestCase("_нач_але", "<em>нач</em>але")]
        [TestCase("кон_це._", "кон<em>це.</em>")]
        [TestCase("__a__", "<strong>a</strong>")]
        [TestCase(
            "__Выделенный двумя символами текст__",
            "<strong>Выделенный двумя символами текст</strong>")]
        public void Render_ShouldWrapInTag_WhenTextIsInsideUnderscores_Test(string text, string expectedHtml)
        {
            var html = inlineRenderer.Render(text);

            html
                .Should()
                .Be(expectedHtml);
        }

        [TestCase("__a _b_ c__", "<strong>a <em>b</em> c</strong>")]
        [TestCase("_a __b__ c_", "<em>a __b__ c</em>")]
        public void Render_ShouldNestTags_OnlyWhenSingleIsInsideDouble_Test(string text, string expectedHtml)
        {
            var html = inlineRenderer.Render(text);

            html
                .Should()
                .Be(expectedHtml);
        }

        [TestCase(@"\_a\_", "_a_")]
        [TestCase(@"\\_a_", @"\<em>a</em>")]
        [TestCase(@"сим\волы\", @"сим\волы\")]
        public void Render_ShouldRemoveBackslash_OnlyWhenItEscapesMarkup_Test(string text, string expectedHtml)
        {
            var html = inlineRenderer.Render(text);

            html
                .Should()
                .Be(expectedHtml);
        }

        [TestCase("__a_")]
        [TestCase("____")]
        [TestCase("цифрами_12_3")]
        public void Render_ShouldKeepUnderscores_WhenTheyDoNotFormTags_Test(string text)
        {
            var html = inlineRenderer.Render(text);

            html
                .Should()
                .Be(text);
        }
    }
}
