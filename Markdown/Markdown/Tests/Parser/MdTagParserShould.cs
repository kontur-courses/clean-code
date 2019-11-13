using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Parser;
using FluentAssertions;
using Markdown.MdTag;
using NUnit.Framework;

namespace Markdown.Tests.Parser
{
    [TestFixture]
    class MdTagParserShould
    {
        private MdTagParser mdTagParser;

        [SetUp]
        public void SetUp()
        {
            mdTagParser = new MdTagParser();
        }

        [Test]
        public void WorkOnOneStrongTag()
        {
            var expectedTag = new Tag {mdTag = "__", htmlTag = "strong"};
            expectedTag.tagContent.Add("abc");

            mdTagParser.Parse("__abc__").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }

        [Test]
        public void WorkOnOneEmTag()
        {
            var expectedTag = new Tag {mdTag = "_", htmlTag = "em"};
            expectedTag.tagContent.Add("abc");

            mdTagParser.Parse("_abc_").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }

        [Test]
        public void WorkOnMoreThanOneTag()
        {
            var expectedTag1 = new Tag {mdTag = "_", htmlTag = "em"};
            expectedTag1.tagContent.Add("abc");

            var expectedTag2 = new Tag {mdTag = "_", htmlTag = "em"};
            expectedTag2.tagContent.Add("b");

            var expectedTag3 = new Tag {mdTag = "__", htmlTag = "strong"};
            expectedTag3.tagContent.Add("dv");

            mdTagParser.Parse("_abc_ _b_ __dv__").Should().BeEquivalentTo(new List<Tag> { expectedTag1, expectedTag2, expectedTag3 });
        }

        [Test]
        public void ShouldWorkOnNestedTags()
        {
            var exTag1 = new Tag { mdTag = "__", htmlTag = "strong" };
            exTag1.tagContent.AddRange(new List<string> { "s", " vs" });

            var exTag2 = new Tag { mdTag = "_", htmlTag = "em" };
            exTag2.tagContent.AddRange(new List<string> { "dv" });

            exTag1.NestedTags.Add(exTag2);

            mdTagParser.Parse("__s _dv_ vs__").Should().BeEquivalentTo(new List<Tag> { exTag1 });
        }

        [Test]
        public void NotWork_When_DifferentTags()
        {
            var expectedTag = new Tag {mdTag = "_", htmlTag = "em"};
            expectedTag.tagContent.Add("a");

            var exp2 = new Tag {fullToken = "", mdTag = "", htmlTag = ""};
            exp2.tagContent.Add("__b_");

            mdTagParser.Parse("_a_ __b_").Should().BeEquivalentTo(new List<Tag> { expectedTag, exp2 });
        }

        [Test]
        public void ShouldWork_When_StrongTagInsideEmTag()
        {
            var expectedTag = new Tag {mdTag = "_", htmlTag = "em"};
            expectedTag.tagContent.Add("fe __ee__ fe");

            mdTagParser.Parse("_fe __ee__ fe_").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }

        [Test]
        public void NotWorkWithNumbers()
        {
            var expectedTag = new Tag();
            expectedTag.tagContent.Add("_1__32__44_");

            mdTagParser.Parse("_1__32__44_").Should().BeEquivalentTo(new List<Tag> { expectedTag });
        }

        [Test]
        public void NotWorkWithSpace()
        {
            var expectedTag1 = new Tag();
            expectedTag1.tagContent.Add("_");

            var expectedTag2 = new Tag();
            expectedTag2.tagContent.Add("abc_");

            mdTagParser.Parse("_ abc_").Should().BeEquivalentTo(new List<Tag> { expectedTag1, expectedTag2 });
        }
    }
}
