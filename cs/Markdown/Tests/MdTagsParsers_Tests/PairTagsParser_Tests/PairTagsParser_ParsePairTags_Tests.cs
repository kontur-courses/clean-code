using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using Markdown.MdTagsParsers;

namespace Markdown.Tests.MdTagsParsers_Tests.PairTagsParser_Tests
{
    class PairTagsParser_ParsePairTags_Tests
    {
        private PairTagsParser sut;

        [SetUp]
        public void SetUp()
        {
            sut = new PairTagsParser();
        }

        [Test]
        public void ShouldThrow_WhenArgumentIsNull()
        {
            Action act = () => sut.ParsePairTags(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ShouldReturnCorrectTagPairs()
        {
            var text = "zxc_asd_qwe__asd__";

            var result = sut.ParsePairTags(text);

            result.Should().BeEquivalentTo(
            (
            new TagToken()
            {
                Tag = new Tag() { Id = "open__", Value = "__" },
                Token = new Token() { StartIndex = 11, Count = 2, Str = text }
            },
            new TagToken()
            {
                Tag = new Tag() { Id = "close__", Value = "__" },
                Token = new Token() { StartIndex = 16, Count = 2, Str = text }
            }
            ),
            (
            new TagToken()
            {
                Tag = new Tag() { Id = "open_", Value = "_" },
                Token = new Token() { StartIndex = 3, Count = 1, Str = text }
            },
            new TagToken()
            {
                Tag = new Tag() { Id = "close_", Value = "_" },
                Token = new Token() { StartIndex = 7, Count = 1, Str = text }
            }
            ));
        }
    }
}