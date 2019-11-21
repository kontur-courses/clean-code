using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Markdown.Tags;
using Markdown.Wrappers;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MdProcessor_Should
    {
        private MdProcessor mdParser;

        [SetUp]
        public void SetUp()
        {
            mdParser = new MdProcessor(new List<ITag>
                {
                    MDTag.GetOneUnderscoreTag(),
                    MDTag.GetTwoUnderscoreTag()
                },
                new MDToHTMLWrapper(MDToHTMLWrapper.GetDefaultMap())
            );
        }
        
        [TestCase("_abc_", "<em>abc</em>", TestName = "ReturnCorrectTag_WithOneUnderscore ")]
        [TestCase("__abc__", "<strong>abc</strong>", TestName = "ReturnCorrectTag_WithTwoUnderscore ")]
        [TestCase("_abc_ __abc__", "<em>abc</em> <strong>abc</strong>", TestName =
            "ReturnCorrectTag_WithMixedUnderscores")]
        [TestCase("_1_2_", "<em>1_2</em>", TestName = "ReturnCorrectTag_WithNumberedTags: one underscore")]
        [TestCase("__1__2__", "<strong>1__2</strong>", TestName = "ReturnCorrectTag_WithNumberedTags: two underscore")]
        [TestCase("__a_b_c__", "<strong>a<em>b</em>c</strong>", TestName = "ReturnCorrectTag_WithCorrectNestedTags")]
        [TestCase("__a_b_v_c_d__", "<strong>a<em>b</em>v<em>c</em>d</strong>", TestName =
            "ReturnCorrectTag_WithCorrectNestedTags")]
        [TestCase("_a__b__c_", "<em>a__b__c</em>", TestName = "ReturnCorrectTag_WithUncorrectNestedTags")]
        [TestCase("__a_b_c_1_2_c__", "<strong>a<em>b</em>c<em>1_2</em>c</strong>", TestName =
            "ReturnCorrectTag_WithComplicatedTags")]
        public void WorkCorrect(string mdTag, string expected)
        {
            var result = mdParser.Render(mdTag);
            result.Should().Be(expected);
        }


        [TestCase("_ abc_ __ abc__", TestName = "ReturnCorrectTag_WithSpacedTags: opener tags")]
        [TestCase("_abc _ __abc __", TestName = "ReturnCorrectTag_WithSpacedTags: closer tags")]
        [TestCase("\\_abc_ \\__abc__", TestName = "Slashed opener tags")]
        [TestCase("_abc\\_ __abc\\__", TestName = "Slashed closer tags")]
        public void NotChange(string mdTag)
        {
            var result = mdParser.Render(mdTag);
            result.Should().Be(mdTag);
        }


        [Test]
        public void WorkLinear()
        {
            var mdTag = "__a_b_c__";
            var times = new List<long>();
            var res = new StringBuilder();
            var watch = new Stopwatch();
            for (var k = 0; k < 100; k++)
            {
                res.Append(" " + mdTag);
                var testedString = res.ToString();
                watch.Start();
                mdParser.Render(testedString);
                watch.Stop();
                times.Add(watch.ElapsedMilliseconds);
                watch.Reset();
            }

            for (var j = 0; j < times.Count - 1; j += 2)
            {
                Assert.AreEqual(times[j], times[j+1], 50);
            }
        }
    }
}