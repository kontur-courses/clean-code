using System.Collections.Generic;
using FluentAssertions;
using Markdown.MdTags;
using NUnit.Framework;

namespace Markdown.Tests.Parser
{
    [TestFixture]
    class MdTagParser
    {
        private Markdown.Parser.MdTagParser mdTagParser;

        [SetUp]
        public void SetUp()
        {
            mdTagParser = new Markdown.Parser.MdTagParser();
        }

        [Test]
        public void Should_WorkOnStrongTag()
        {
            var tag = new StrongTag();
            tag.NestedTags.Add(new SimpleTag("abc"));

            mdTagParser.Parse("__abc__").Should().BeEquivalentTo(new List<Tag> { tag });
        }

        [Test]
        public void Should_WorkOnEmTag()
        {
            var expectedTag = new EmTag();
            expectedTag.NestedTags.Add(new SimpleTag("abc"));

            mdTagParser.Parse("_abc_").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }

        [Test]
        public void Should_WorkOnStrikeTag()
        {
            var tag = new StrikeTag();
            tag.NestedTags.Add(new SimpleTag("abc"));

            mdTagParser.Parse("~abc~").Should().BeEquivalentTo(new List<Tag> { tag });
        }

        [Test]
        public void Should_WorkOnCodeTag()
        {
            var expectedTag = new CodeTag();
            expectedTag.NestedTags.Add(new SimpleTag("abc"));
            mdTagParser.Parse("`abc`").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }

        [Test]
        public void Should_WorkOnListTag()
        {
            var expectedTag = new ListTag();
            expectedTag.NestedTags.Add(new SimpleTag("abc"));
            expectedTag.NestedTags.Add(new SimpleTag("_cba"));
            expectedTag.NestedTags.Add(new SimpleTag("_"));
            mdTagParser.Parse("*abc_cba_").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }

        [Test]
        public void Should_WorkOnHorizontalTag()
        {
            var expectedTag = new HorizontalTag();
            expectedTag.NestedTags.Add(new SimpleTag());
            mdTagParser.Parse("***").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }

        [Test]
        public void Should_NotWork_When_SomethingAfterHorizontalTag()
        {
            var expectedTag = new SimpleTag("***abc");

            mdTagParser.Parse("***abc").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }

        [Test]
        public void Should_WorkOnBlockquote()
        {
            var expectedTag = new BlockquoteTag();
            expectedTag.NestedTags.Add(new SimpleTag("abc"));
            mdTagParser.Parse(">abc").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }

        [Test]
        public void Should_WorkOnHeader()
        {
            var expectedTag = new HeaderTag("###");
            expectedTag.NestedTags.Add(new SimpleTag("abc"));
            mdTagParser.Parse("###abc").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }
    }
}
