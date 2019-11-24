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

        [Test]
        public void Should_ScreenAllSymbolInCodeTag()
        {
            var expectedTag = new CodeTag((@"`\\a\_\b\#\__c__\\`".Length, @"`\\a\_\b\#\__c__\\`"));
            mdTagParser.Parse(@"`\\a\_\b\#\__c__\\`").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }

        [Test]
        public void Should_NotWorkWithDigits()
        {
            var expectedTag = new SimpleTag(("цифрами_12_3".Length, "цифрами_12_3"));
            mdTagParser.Parse("цифрами_12_3").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }

        [Test]
        public void Should_OpenHeaderTag_When_SpaceBeforeContent()
        {
            var expectedTag = new HeaderTag((4, " abc"), "#");
            mdTagParser.Parse("# abc").Should().BeEquivalentTo(new List<Tag> {expectedTag});
        }

        [Test]
        public void Should_CloseHeaderTagAfterMovingInNewLine()
        {
            var expectedTag1 = new HeaderTag((3, "abc"), "#");
            var expectedTag2 = new SimpleTag((3, "abc"));
            var expectedTag3 = new StrongTag((1, "a"));
            mdTagParser.Parse("#abc\nabc__a__").Should().BeEquivalentTo(new List<Tag>
            {
                expectedTag1,
                expectedTag2,
                expectedTag3
            });
        }

        [Test]
        public void Should_Work_When_NewLineAfterHorizontalTag()
        {
            var expectedTag1 = new HorizontalTag();
            var expectedTag2 = new SimpleTag((3, "\r\nabc"));
            var res = mdTagParser.Parse("***\r\nabc");
            mdTagParser.Parse("***\r\nabc").Should().BeEquivalentTo(new List<Tag>
            {
                expectedTag1,
                expectedTag2
            });
        }
    }
}
