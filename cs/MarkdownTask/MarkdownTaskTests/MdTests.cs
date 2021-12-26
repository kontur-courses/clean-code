using System;
using FluentAssertions;
using MarkdownTask;
using MarkdownTask.Searchers;
using NUnit.Framework;

namespace MarkdownTaskTests
{
    public class MdTests
    {
        private Md md;

        [SetUp]
        public void Setup()
        {
            md = new Md(new ITagSearcher[]
            {
                new HeaderTagSearcher(),
                new StrongTagSearcher(),
                new ItalicTagSearcher()
            });
        }

        [Test]
        public void Render_ShouldBeImplemented()
        {
            Action act = () => md.Render(string.Empty);
            act.Should().NotThrow<NotImplementedException>();
        }

        [TestCase("_text_", @"<em>text</em>")]
        [TestCase("_te_xt", @"<em>te</em>xt")]
        [TestCase("t_ex_t", @"t<em>ex</em>t")]
        [TestCase("te_xt_", @"te<em>xt</em>")]
        public void Render_ShouldCorrectRender_EmTag(string mdText, string expectedResult)
        {
            var actualResult = md.Render(mdText);
            actualResult.Should().Be(expectedResult);
        }

        [TestCase("__text__", @"<strong>text</strong>")]
        [TestCase("__te__xt", @"<strong>te</strong>xt")]
        [TestCase("t__ex__t", @"t<strong>ex</strong>t")]
        [TestCase("te__xt__", @"te<strong>xt</strong>")]
        public void Render_ShouldCorrectRender_StrongTag(string mdText, string expectedResult)
        {
            var actualResult = md.Render(mdText);

            actualResult.Should().Be(expectedResult);
        }

        [TestCase("# text", "<h1>text</h1>")]
        [TestCase("# some\n\n# text", "<h1>some</h1>\n\n<h1>text</h1>")]
        public void Render_ShouldCorrectRender_HeaderTag(string mdText, string expectedResult)
        {
            var actualResult = md.Render(mdText);

            actualResult.Should().Be(expectedResult);
        }

        [TestCase("# _some_ __text__", "<h1><em>some</em> <strong>text</strong></h1>")]
        [TestCase("# _some_\n\n# __text__", "<h1><em>some</em></h1>\n\n<h1><strong>text</strong></h1>")]
        [TestCase("__some__ _text_\n\n# paragraph", "<strong>some</strong> <em>text</em>\n\n<h1>paragraph</h1>")]
        public void Render_ShouldCorrectRender_SequenceOfTags(string mdText, string expectedResult)
        {
            var actualResult = md.Render(mdText);

            actualResult.Should().Be(expectedResult);
        }
    }
}