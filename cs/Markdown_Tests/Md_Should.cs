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

        [TestCase(@"\_testText\_", "_testText_", TestName = "ReplaceEscapedGrounds")]
        [TestCase("_test _", "_test _", TestName = "NotReplaceGround_IfClosingGroundGoesAfterSpace")]
        [TestCase("_ test_", "_ test_", TestName = "NotReplaceGround_IfOpeningGroundGoesBeforeSpace")]
        [TestCase("digit_12_3", "digit_12_3", TestName = "NotReplaceGrounds_InsideTextWithDigitsDelimitedWithGrounds")]
        [TestCase("__test _Text", "__test _Text", TestName = "NotReplaceNotParedSelectionBorders")]
        [TestCase("_test __a__ Text_", "<em>test __a__ Text</em>", TestName =
            "NotReplaceTwoGroundsByStrongTag_InsideGrounds")]
        [TestCase("_testText_", "<em>testText</em>", TestName = "NotReplaceGround_IfClosingGroundGoesAfterSpace")]
        [TestCase("__test _a_ Text__", "<strong>test <em>a</em> Text</strong>", TestName =
            "ReplaceGroundsByEmTag_InsideTwoGrounds")]
        [TestCase("__testText__", "<strong>testText</strong>", TestName = "ReplaceTwoGroundsByStrongTag")]
        [TestCase("__test _a__ Text_", "<strong>test _a</strong> Text_", TestName =
            "ReplaceFirstOpenedTag_WhenCrossedAndSecondIsLeft")]
        [TestCase("__test_ a__ Text_", "<strong>test_ a</strong> Text_", TestName =
            "ReplaceFirstOpenedTag_WhenCrossedAndSecondIsRight")]
        public void Replace(string text, string expected)
        {
            Md.Render(text).Should().Be(expected);
        }

        [Test]
        public void NotFall_WhenEmptyStringGiven()
        {
            Func<string> testDelegate = () => Md.Render("");
            testDelegate.Should().NotThrow();
        }
    }
}