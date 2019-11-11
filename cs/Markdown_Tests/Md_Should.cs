using System;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace Markdown_Tests
{
    [TestFixture]
    public class Md_Should
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void NotFall_WhenEmptyStringGiven()
        {
            Func<string> testDelegate = () => Md.Render("");
            testDelegate.Should().NotThrow();
        }

        [Test]
        public void NotReplaceEscapedGrounds()
        {
            Md.Render(@"\_testText\_").Should().Be(@"_testText_");
        }

        [Test]
        public void NotReplaceGround_IfClosingGroundGoesAfterSpace()
        {
            Md.Render("_test _").Should().Be("_test _");
        }

        [Test]
        public void NotReplaceGround_IfOpeningGroundGoesBeforeSpace()
        {
            Md.Render("_ test_").Should().Be("_ test_");
        }

        [Test]
        public void NotReplaceGrounds_InsideTextWithDigitsDelimitedWithGrounds()
        {
            Md.Render("digit_12_3").Should().Be("digit_12_3");
        }

        [Test]
        public void NotReplaceNotParedSelectionBorders()
        {
            Md.Render("__test _Text").Should().Be("__test _Text");
        }

        [Test]
        public void NotReplaceTwoGroundsByStrongTag_InsideGrounds()
        {
            Md.Render("_test__a__Text_").Should().Be("<em>test__a__Text</em>");
        }

        [Test]
        public void ReplaceGroundByEmTag()
        {
            Md.Render("_testText_").Should().Be("<em>testText</em>");
        }

        [Test]
        public void ReplaceGroundsByEmTag_InsideTwoGrounds()
        {
            Md.Render("__test_a_Text__").Should().Be("<strong>test<em>a</em>Text</strong>");
        }

        [Test]
        public void ReplaceTwoGroundsByStrongTag()
        {
            Md.Render("__testText__").Should().Be("<strong>testText</strong>");
        }
    }
}