using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MdTests
    {
        private readonly Md md = new ();

        [TestCase("# Заголовок", "<h1>Заголовок</h1>")]
        [TestCase(
            "# Заголовок __с _разными_ символами__",
            "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
        public void Render_ShouldWrapInH1_WhenParagraphStartsWithHash_Test(string markdown, string expectedHtml)
        {
            var html = md.Render(markdown);

            html
                .Should()
                .Be(expectedHtml);
        }

        [TestCase("#Заголовок")]
        [TestCase("a # b")]
        public void Render_ShouldKeepHash_WhenItIsNotHeaderPrefix_Test(string markdown)
        {
            var html = md.Render(markdown);

            html
                .Should()
                .Be(markdown);
        }

        [Test]
        public void Render_ShouldKeepHashWithoutBackslash_WhenHeaderPrefixIsEscaped_Test()
        {
            var html = md.Render(@"\# Не заголовок");

            html
                .Should()
                .Be("# Не заголовок");
        }

        [TestCase("a\nb", "a\nb")]
        [TestCase("a\r\nb", "a\nb")]
        [TestCase("# a\n_b_", "<h1>a</h1>\n<em>b</em>")]
        public void Render_ShouldRenderEachParagraph_WhenTextHasLineBreaks_Test(string markdown, string expectedHtml)
        {
            var html = md.Render(markdown);

            html
                .Should()
                .Be(expectedHtml);
        }

        [Test]
        public void Render_ShouldNotPairUnderscores_WhenTheyAreInDifferentParagraphs_Test()
        {
            var html = md.Render("_a\nb_");

            html
                .Should()
                .Be("_a\nb_");
        }
    }
}
