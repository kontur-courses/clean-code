using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using Markdown.MdTagsConverters;

namespace Markdown.Tests.MdTagsConverters_Tests.MdTagsToHtmlTagsConverter_Tests
{
    class MdTagsToHtmlTagsConverter_GetHtmlTagByMdTag_Tests
    {
        [TestCase("open_", "_", "<em>", "<em>")]
        [TestCase("close_", "_", "</em>", "</em>")]
        [TestCase("open__", "__", "<strong>", "<strong>")]
        [TestCase("close__", "__", "</strong>", "</strong>")]
        public void ShouldReturnCorrectHtmlTag(string mdId, string mdValue, string htmlId, string htmlValue)
        {
            var mdTag = new Tag() { Id = mdId, Value = mdValue };
            var expectedTag = new Tag() { Id = htmlId, Value = htmlValue };

            var result = GetHtml(mdTag);

            result.Should().BeEquivalentTo(expectedTag);
        }

        [Test]
        public void ShouldThrowArgumentException_WhenMdTagHaveNoAssotiationWithHtmlTag()
        {
            Action act = () => GetHtml(new Tag() { Id = "zxc", Value = "_" });

            act.Should().Throw<ArgumentException>();
        }

        private Tag GetHtml(Tag md) =>
            MdTagsToHtmlTagsConverter.GetHtmlTagByMdTag(md);
    }
}