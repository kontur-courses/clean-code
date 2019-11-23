using System.Collections.Generic;
using FluentAssertions;
using Markdown.MdTags;
using NUnit.Framework;

namespace Markdown.Tests.Parser
{
    [TestFixture]
    internal class MdTagParserTests
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
            var tag = new StrongTag((3, "abc"));

            mdTagParser.Parse("__abc__").Should().BeEquivalentTo(new List<Tag> { tag });
        }

        [Test]
        public void Should_WorkOnEmTag()
        {
            var expectedTag = new EmTag((3, "abc"));

            mdTagParser.Parse("_abc_").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }

        [Test]
        public void Should_WorkOnStrikeTag()
        {
            var tag = new StrikeTag((3, "abc"));

            mdTagParser.Parse("~abc~").Should().BeEquivalentTo(new List<Tag> { tag });
        }

        [Test]
        public void Should_WorkOnCodeTag()
        {
            var expectedTag = new CodeTag((3, "abc"));
            mdTagParser.Parse("`abc`").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }

        [Test]
        public void Should_WorkOnListTag()
        {
            var expectedTag = new ListTag((3, "abc"));
            expectedTag.NestedTags.Add(new SimpleTag((4, "_cba")));
            expectedTag.NestedTags.Add(new SimpleTag((1, "_")));
            mdTagParser.Parse("*abc_cba_").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }
        
        [Test]
        public void Should_WorkOnHorizontalTag()
        {
            var expectedTag = new HorizontalTag((0, string.Empty));
            mdTagParser.Parse("***").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }

        [Test]
        public void Should_NotWork_When_SomethingAfterHorizontalTag()
        {
            var expectedTag = new SimpleTag((6, "***abc"));
            mdTagParser.Parse("***abc").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }

        [Test]
        public void Should_WorkOnBlockquote()
        {
            var expectedTag = new BlockquoteTag((3, "abc"));
            mdTagParser.Parse(">abc").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }

        [Test]
        public void Should_WorkOnHeader()
        {
            var expectedTag = new HeaderTag((3, "abc"), "###");
            mdTagParser.Parse("###abc").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }
    }
}
