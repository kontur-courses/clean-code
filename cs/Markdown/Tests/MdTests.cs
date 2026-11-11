using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MdTests
    {
        [TestCase("# Заголовок", "<h1>Заголовок</h1>")]
        [TestCase(
            "# Заголовок __с _разными_ символами__",
            "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
        public void Render_ShouldWrapInH1_WhenParagraphStartsWithHash_Test(string markdown, string expectedHtml)
        {
            var html = Md.Render(markdown);

            html
                .Should()
                .Be(expectedHtml);
        }

        [TestCase("#Заголовок")]
        [TestCase("a # b")]
        public void Render_ShouldKeepHash_WhenItIsNotHeaderPrefix_Test(string markdown)
        {
            var html = Md.Render(markdown);

            html
                .Should()
                .Be(markdown);
        }

        [Test]
        public void Render_ShouldKeepHashWithoutBackslash_WhenHeaderPrefixIsEscaped_Test()
        {
            var html = Md.Render(@"\# Не заголовок");

            html
                .Should()
                .Be("# Не заголовок");
        }

        [TestCase("a\n\nb", "a\n\nb")]
        [TestCase("a\nb", "a\nb")]
        [TestCase("a\r\nb", "a\nb")]
        [TestCase("# a\n_b_", "<h1>a</h1>\n<em>b</em>")]
        public void Render_ShouldRenderEachParagraph_WhenTextHasLineBreaks_Test(string markdown, string expectedHtml)
        {
            var html = Md.Render(markdown);

            html
                .Should()
                .Be(expectedHtml);
        }

        [TestCase("- a\n- b", "<ul><li>a</li>\n<li>b</li></ul>")]
        [TestCase("a\n- _b_", "a\n<ul><li><em>b</em></li></ul>")]
        [TestCase("- a\nb", "<ul><li>a</li></ul>\nb")]
        public void Render_ShouldCreateList_WhenParagraphsStartWithDash_Test(string markdown, string expectedHtml)
        {
            var html = Md.Render(markdown);

            html
                .Should()
                .Be(expectedHtml);
        }

        [TestCase(@"\- a", "- a")]
        [TestCase("-a", "-a")]
        public void Render_ShouldNotCreateList_WhenDashIsNotListPrefix_Test(string markdown, string expectedHtml)
        {
            var html = Md.Render(markdown);

            html
                .Should()
                .Be(expectedHtml);
        }

        [Test]
        public void Render_ShouldNotPairUnderscores_WhenTheyAreInDifferentParagraphs_Test()
        {
            var html = Md.Render("_a\nb_");

            html
                .Should()
                .Be("_a\nb_");
        }
    }
}
