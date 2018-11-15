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
            var emTag = MakeTag("_", "<em>");
            var strongTag = MakeTag("__", "<strong>");
            var tagList = new List<TagKeeper>
            {
                emTag,
                strongTag
            };
            md = new Md(tagList);
        }

        private TagKeeper MakeTag(string md, string html)
        {
            var tagHtml = new Tag(html, Language.Html);
            var tagMd = new Tag(md, Language.Md);
            return new TagKeeper(tagHtml, tagMd);
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
        public void ReplaceTwoGroundSymbolsToStrongTags()
        {
            var input = "__word__";
            var expected = "<strong>word</strong>";

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
