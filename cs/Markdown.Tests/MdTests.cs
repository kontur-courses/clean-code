using System;
using System.Security.Cryptography;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("_a_",ExpectedResult = "<em>a</em>",TestName = "{m}(_a_)")]
        [TestCase("__a__", ExpectedResult = "<strong>a</strong>", TestName = "{m}(__a__)")]
        [TestCase("#a\n", ExpectedResult = "<h1>a</h1>", TestName = "{m}(#a)")]
        public string RenderTest_On(string line)
        {
            return Md.Render(line);
        }
        [TestCase("_aaa__b__ccc_", ExpectedResult = "<em>aaa__b__ccc</em>",TestName = "{m}_DoubleUnderscoreInUnderscore")]
        [TestCase("__a_b___", ExpectedResult = "<strong>a<em>b</em></strong>", TestName = "{m}_UnderscoreInDoubleUnderscore")]
        [TestCase("#__a_b___\n", ExpectedResult = "<h1><strong>a<em>b</em></strong></h1>", TestName = "{m}_TagsInHeader")]
        public string Render_NestingTest_On(string line)
        {
            return Md.Render(line);
        }

        [Test]
        public void Render_Should_Fail_OnNull()
        {
            Action action = () => Md.Render(null);
            Assert.Throws<ArgumentNullException>(() => action());
        }
    }
}