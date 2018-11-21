using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Markdown
{
    [TestFixture]
    public class Markdown_should
    {
        private Markdown markdown;
        private TokenSelector tokenSelector;
        
        [SetUp]
        public void Init()
        {
            markdown = new Markdown();
            var emTag = new Tag("em");
            var strongTag = new Tag("strong");
            strongTag.TagDependencies.Add(upperTags => !upperTags.Contains(emTag));
            markdown.Tags.Add("__",strongTag);
            markdown.Tags.Add("_",emTag);
            
            tokenSelector = new TokenSelector();
            tokenSelector.TokenDependency.Add(x =>
                !(x.Next != null && x.Next.Value.HasNumber && x.Previous != null && x.Previous.Value.HasNumber));


        }

        [TestCase(@"__test__", ExpectedResult = "<strong>test</strong>", TestName = "Tag Strong works")]
        [TestCase(@"_test_", ExpectedResult = @"<em>test</em>", TestName = "Tag Em works" )]
        [TestCase(@"1_1test_", ExpectedResult = @"1_1test_", TestName = "Tag don't recognized if it in the middle of text with number")]
        [TestCase(@"_ test_", ExpectedResult = @"_ test_",TestName = "Tag don't recognize as Open if next token is WhiteSpace")]
        [TestCase(@"__test __",ExpectedResult = @"__test __", TestName = "Tag don't recognize as Close if previous token is WhiteSpace")]
        [TestCase(@"_te __test__ st_", ExpectedResult =@"<em>te __test__ st</em>", TestName = "Strong tag doesn't work in em tag")]
        [TestCase(@"__te _test_ st__", ExpectedResult =@"<strong>te <em>test</em> st</strong>" , TestName = "Em tag works in strong tag")]
        [TestCase(@"_\_test__", ExpectedResult = "__test__", TestName = "Ecranation works")]
        [TestCase(@"__test_", ExpectedResult = "__test_", TestName = "Diferent tags didn't recognize")]
        public string Test(string input)
        {
            return markdown.Render(input,tokenSelector);
        }
    }
}