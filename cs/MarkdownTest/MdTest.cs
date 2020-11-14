using Markdown;
using FluentAssertions;
using NUnit.Framework;

namespace MarkdownTest
{
    [TestFixture]
    public class MdTest_Should
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }


        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void RenderShouldReturnEmpty_WhenInputEmptyOrNull(string input)
        {
            md.Render(input).Should().BeEmpty();
        }

        [Test]
        public void RenderShouldReturnSame_WhenInputWithoutTags()
        {
            var input = "Input string without tags";
            md.Render(input).Should().Be(input);
        }

        [Test]
        [TestCase("_Input string with em_", "<em>Input string with em</em>")]
        [TestCase("__Input string with strong__", "<strong>Input string with strong</strong>")]
        [TestCase("# Header", "<h1>Header</h1>")]
        public void RenderShouldReturnCorrectTag(string input, string output)
        {
            md.Render(input).Should().Be(output);
        }

        [Test]
        [TestCase("_Not closed italic", "_Not closed italic")]
        [TestCase("__Not closed bold", "__Not closed bold")]
        public void RenderShouldntConvert_WhenTagNotClosed(string input, string output)
        {
            md.Render(input).Should().Be(output);
        }

        [Test]
        public void RenderShouldntConvertBold_WhenBoldInsideItalic()
        {
            md.Render("_Bold can't be __inside__ italic_ tag").Should()
                .Be("<em>Bold can't be __inside__ italic</em> tag");
        }

        [Test]
        public void RenderShouldConvert_WhenItalicInsideBold()
        {
            md.Render("__Italic _inside_ bold__").Should().Be("<strong>Italic <em>inside</em> bold</strong>");
        }

        [Test]
        [TestCase("\\_Input string\\_", "_Input string_")]
        [TestCase("\\___Input string__\\_", "___Input string___")]
        [TestCase("__\\_Input string\\___", "<strong>_Input string_</strong>")]
        [TestCase("\\# Input string", "# Input string")]
        public void Render_BackslashesShouldEscaping(string input, string output)
        {
            md.Render(input).Should().Be(output);
        }

        [Test]
        public void RenderShouldntConvert_WhenUnderscoreHasNoPair() /// Переименовать
        {
            var input = "_ rtr_";
            var output = "_ rtr_";
            md.Render(input).Should().Be(output);
        }

        [Test]
        [TestCase("____", "____")]
        public void RenderRenderShouldntConvert_WhenEmptyTag(string input, string output)
        {
            md.Render(input).Should().Be(output);
        }

        [Test]
        public void RenderShouldReturnCorrect_ExampleFromSpecification()
        {
            var input = "# Заголовок __с _разными_ символами__";
            var output = "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>";
            md.Render(input).Should().Be(output);
        }

        [Test]
        public void RenderShouldntConvert_WhenTagsIntersect()
        {
            var input = "__когда _два тега__ пересекаются_";
            var output = "__когда _два тега__ пересекаются_";
            md.Render(input).Should().Be(output);
        }

        [Test]
        public void RenderShouldParseCorrect_WhenClosingUnderscoreContainsMoreThanTwoUnderscores()
        {
            var input = "__Input string___ ф_";
            var output = "<strong>Input string</strong>_ ф_";
            md.Render(input).Should().Be(output);
        }
    }
}