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
    }
}