using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Markdown;
using NUnit.Framework.Constraints;

namespace MarkdownTests
{
    [TestFixture]
    class Md_Should
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            var tagHtml = new Tag("<em>", Language.Html);
            var tagMd = new Tag("_", Language.Md);
            var tagKeeper = new TagKeeper(tagHtml, tagMd);
            var tagList = new List<TagKeeper> { tagKeeper };
            md = new Md(tagList);
        }

        [Test]
        public void ReturnEmptyString_WhenInputIsEmpty()
        {
            var input = "";

            var result = md.Render(input);

            result.Should().BeEmpty();
        }

        [Test]
        public void ReplaceOneGroundSymbolsToEmTags()
        {
            var input = "_word_";
            var expected = "<em>word</em>";

            var result = md.Render(input);

            result.Should().Be(expected);
        }

        [Test]
        public void DoesntReplaceEscapedSymbols()
        {
            var input = @"\_word\_";
            var expected = "_word_";

            var result = md.Render(input);

            result.Should().Be(expected);
        }
    }
}
