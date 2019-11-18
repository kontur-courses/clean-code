using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MD_should
    {
        private MD mdParser;
        [SetUp]
        public void SetUp()
        {
            mdParser = new MD();
        }
        
        [Test]
        public void ReturnCorrectTag_WithOneUnderscore()
        {
            var mdTag = "_abc_";
            var result = mdParser.Render(mdTag);
            var expected = "<em>abc</em>";
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void ReturnCorrectTag_WithTwoUnderscore()
        {
            var mdTag = "__abc__";
            var result = mdParser.Render(mdTag);
            var expected = "<strong>abc</strong>";
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void ReturnCorrectTag_WithMixedUnderscores()
        {
            var mdTag = "_abc_ __abc__";
            var result = mdParser.Render(mdTag);
            var expected = "<em>abc</em> <strong>abc</strong>";
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void ReturnCorrectTag_WithSpacedTags()
        {
            var mdTag = "_ abc_ __ abc__";
            var result = mdParser.Render(mdTag);
            Assert.AreEqual(mdTag, result);
            mdTag = "_abc _ __abc __";
            result = mdParser.Render(mdTag);
            Assert.AreEqual(mdTag, result);
        }
        
        [Test]
        public void ReturnCorrectTag_WithSlashedTags()
        {
            var mdTag = "\\_abc_ \\_\\_abc__";
            var result = mdParser.Render(mdTag);
            Assert.AreEqual(mdTag, result);
            mdTag = "_abc\\_ __abc\\__";
            result = mdParser.Render(mdTag);
            Assert.AreEqual(mdTag, result);
        }
        
        [Test]
        public void ReturnCorrectTag_WithNumberedTags()
        {
            var mdTag = "_1_2_";
            var result = mdParser.Render(mdTag);
            var expected = "<em>1_2</em>";
            Assert.AreEqual(expected, result);
            mdTag = "__1__2__";
            expected = "<strong>1__2</strong>";
            result = mdParser.Render(mdTag);
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void ReturnCorrectTag_WithCorrectNestedTags()
        {
            var mdTag = "__a_b_c__";
            var result = mdParser.Render(mdTag);
            var expected = "<strong>a<em>b</em>c</strong>";
            Assert.AreEqual(expected, result);
            mdTag = "__a_b_v_c_d__";
            result = mdParser.Render(mdTag);
            expected = "<strong>a<em>b</em>v<em>c</em>d</strong>";
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void ReturnCorrectTag_WithUncorrectNestedTags()
        {
            var mdTag = "_a__b__c_";
            var result = mdParser.Render(mdTag);
            var expected = "<em>a__b__c</em>";
            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void ReturnCorrectTag_WithComplicatedTags()
        {
            var mdTag = "__a_b_c_1_2_c__";
            var result = mdParser.Render(mdTag);
            var expected = "<strong>a<em>b</em>c<em>1_2</em>c</strong>";
            Assert.AreEqual(expected, result);
        }
    }
}