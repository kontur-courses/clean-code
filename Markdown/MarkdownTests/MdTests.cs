using System;
using System.Collections.Generic;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class MdTests
    {
        private Md md;

        private readonly List<MdType> types = new List<MdType>
        {
            MdType.SingleUnderLine,
            MdType.DoubleUnderLine,
            MdType.Sharp,
            MdType.TripleGraveAccent,
            MdType.Link
        };

    [SetUp]
        public void SetUp()
        {
            md = new Md(types);
        }

        [Test]
        public void NullString_ShouldRenderNull() =>
            Assert.Throws<ArgumentNullException>(() => md.Render(null));

        [TestCase("hello world",
            ExpectedResult = "hello world",
            TestName = "WithoutTags")]
        [TestCase("hello _world_",
            ExpectedResult = "hello <em>world</em>",
            TestName = "SingleUnderLineTag")]
        [TestCase(@"hello \_world\_",
            ExpectedResult = "hello _world_",
            TestName = "EscapedSingleUnderLineTags")]
        [TestCase("hello __world__",
            ExpectedResult = "hello <strong>world</strong>",
            TestName = "DoubleUnderLineTag")]
        [TestCase("__hello _happy_ world__",
            ExpectedResult = "<strong>hello <em>happy</em> world</strong>",
            TestName = "SingleUnderLineTagIsInsideDoubleUnderLineTag")]
        [TestCase("_hello __happy__ world_",
            ExpectedResult = "<em>hello __happy__ world</em>",
            TestName = "DoubleUnderLineTagIsInsideSingleUnderLineTag")]
        [TestCase("_12_3",
            ExpectedResult = "_12_3",
            TestName = "SingleUnderLineTagInsideTheNumber")]
        [TestCase("__hello _world",
            ExpectedResult = "__hello _world",
            TestName = "UnPairedTags")]
        [TestCase("__a _b_ _c_ d__",
            ExpectedResult = "<strong>a <em>b</em> <em>c</em> d</strong>",
            TestName = "SeveralSingleUnderLineTagsInsideDoubeUnderLineTag")]
        [TestCase("hello_ world_",
            ExpectedResult = "hello_ world_",
            TestName = "IncorrectOpenTag")]
        [TestCase("__a__ _b_ #c#",
            ExpectedResult = "<strong>a</strong> <em>b</em> <h1>c</h1>",
            TestName = "ManyTags")]
        [TestCase("#hello world#",
            ExpectedResult = "<h1>hello world</h1>",
            TestName = "SharpTag")]
        [TestCase("```hello world```",
            ExpectedResult = "<code>hello world</code>",
            TestName = "TrippleGraveAccent")]
        [TestCase("[hello](world)",
            ExpectedResult = "<a href=\"world\">hello</a>",
            TestName = "Link")]
        public string Render(string input) =>
            md.Render(input);
    }
}