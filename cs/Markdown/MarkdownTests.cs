using  NUnit.Framework;
using  FluentAssertions;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MarkdownTests
    {
        [TestFixture]
        public class TextToTokensParserTests
        {
            [Test]
            public void Parser_Should_ParseTokensCorrectly1()
            {
                var text = "ab_aaa_bbb";
                var expectedResult= new List<string>() {"_aaa_"};
                var result = TextToTokensParser.Parse(text);
                result.Select(t => t.Line)
                      .ToList()
                      .Should()
                      .BeEquivalentTo(expectedResult);
            }

            [Test]
            public void Parser_Should_ParseTokensCorrectly2()
            {
                var text = "ab_aaa_bb_b_";
                var expectedResult = new List<string>() { "_aaa_","_b_" };
                var result = TextToTokensParser.Parse(text);
                result.Select(t => t.Line)
                    .ToList()
                    .Should()
                    .BeEquivalentTo(expectedResult);
            }

            [Test]
            public void Parser_Should_ParseTokensCorrectly3()
            {
                var text = "ab__aaa__bb_b_";
                var expectedResult = new List<string>() { "__aaa__", "_b_" };
                var result = TextToTokensParser.Parse(text);
                result.Select(t => t.Line)
                    .ToList()
                    .Should()
                    .BeEquivalentTo(expectedResult);
            }

            [Test]
            public void Parser_Should_ParseTokensCorrectly4()
            {
                var text = "ab_aaa_bb__b a__ acc";
                var expectedResult = new List<string>() { "_aaa_", "__b a__" };
                var result = TextToTokensParser.Parse(text);
                result.Select(t => t.Line)
                    .ToList()
                    .Should()
                    .BeEquivalentTo(expectedResult);
            }
        }

        [TestFixture]
        public class MarkdownToHtmlParserTests
        {
            [Test]
            public void Parser_Should_ParseTokensToTokensCorrectly1()
            {
               var text= "ab_aaa_bb__b a__ acc";
               var expectedResult= new List<string>() { "<em>aaa</em>", "<strong>b a</strong>" };
                var textTokens = TextToTokensParser.Parse(text);
               var dict=new Dictionary<Token,Token>();
               var htmltokens = MarkdownToHtmlParser.Parse(textTokens, dict);
               htmltokens.Select(t => t.Line)
                   .ToList()
                   .Should()
                   .BeEquivalentTo(expectedResult);
            }

            [Test]
            public void Parser_Should_ParseTokensToTokensCorrectly2()
            {
                var text = "ab__aaa__bb_b_";
                var expectedResult = new List<string>() { "<strong>aaa</strong>", "<em>b</em>" };
                var textTokens = TextToTokensParser.Parse(text);
                var dict = new Dictionary<Token, Token>();
                var htmltokens = MarkdownToHtmlParser.Parse(textTokens, dict);
                htmltokens.Select(t => t.Line)
                    .ToList()
                    .Should()
                    .BeEquivalentTo(expectedResult);
            }
        }

        [TestFixture]
        public class RenderTests
        {
            [TestCase("ab__aaa__bb_b_",ExpectedResult = "ab<strong>aaa</strong>bb<em>b</em>")]
            [TestCase("ab_a aa_bb__b__", ExpectedResult = "ab<em>a aa</em>bb<strong>b</strong>")]
            public string Render_ShouldRenderCorrectly1(string text)
            {
                return new Md().Render(text);
            }
        }
    }
}
