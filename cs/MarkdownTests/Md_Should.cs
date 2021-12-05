using System;
using NUnit.Framework;
using FluentAssertions;
using Markdown;

namespace MarkdownTests
{
    [TestFixture]
    public class Md_Should
    {
        private readonly Md markdown = new();

        [Test]
        public void Throw_WhenNullGiven()
        {
            Action action = () => markdown.Render(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestCase("", TestName = "When input is empty string")]
        [TestCase("abcDEF", TestName = "When input does not contains tags")]
        [TestCase("__hello _great__ world_", TestName = "When different tags was crossed")]
        [TestCase("____", TestName = "When nothing inside strong marked line")]
        public void ReturnSameStringWhen(string rawString)
        {
            AssertRenderReturnSameString(rawString);
        }

        [TestCase("_hello_",
            "<em>hello</em>",
            TestName = "When one word been emphasized")]
        [TestCase("_hello wonderful world_",
            "<em>hello wonderful world</em>",
            TestName = "When one word been emphasized")]
        public void ReturnEmphasizedValueWhen(string rawString, string expected)
        {
            AssertRenderIsCorrect(rawString, expected);
        }

        [TestCase("__hello__",
            "<strong>hello</strong>",
            TestName = "When one word been marked")]
        [TestCase("__hello wonderful world__",
            "<strong>hello wonderful world</strong>",
            TestName = "When more than one word been marked")]
        public void ReturnStrongValueWhen(string rawString, string expected)
        {
            AssertRenderIsCorrect(rawString, expected);
        }

        [TestCase("# Hello world",
            "<h1>Hello world</h1>",
            TestName = "When paragraph was not closed with \\n\\n")]
        [TestCase("# Hello world\n\n",
            "<h1>Hello world</h1>",
            TestName = "When paragraph was closed with \\n\\n")]
        [TestCase("# Hello world\n\n# It's me",
            "<h1>Hello world</h1>\n\n<h1>It's me</h1>",
            TestName = "When two different paragraphs contains header")]
        public void ReturnHeadedValueWhen(string rawString, string expected)
        {
            AssertRenderIsCorrect(rawString, expected);
        }

        [TestCase("__hello _great_ world__",
            "<strong>hello <em>great</em> world</strong>",
            TestName = "When <em> tag inside <strong>")]
        public void ReturnMarkedValueWithDifferentMarkingTypes(string rawString, string expected)
        {
            AssertRenderIsCorrect(rawString, expected);
        }

        [TestCase("_abc", TestName = "When <em> tag was not closed")]
        [TestCase("__abc", TestName = "When <strong> was not closed")]
        [TestCase("_a \n\n b_", TestName = "When closing symbols in different paragraphs")]
        public void IgnoreUnpairedSymbols(string rawString)
        {
            AssertRenderReturnSameString(rawString);
        }
        
        [Test]
        public void NotContainsStrongTag_WhenStrongTagWasInsideEmphasized()
        {
            var rawString = "_hello __great__ world_";
            AssertRenderReturnSameString(rawString);
        }
        
        [Test]
        public void EscapeEscapingSymbol()
        {
            var rawString = @"\\hello\\";
            var expected = @"\hello\";
            
            AssertRenderIsCorrect(rawString, expected);
        }
        
        [Test]
        public void NotMarkTextWhichContainDigits()
        {
            var rawString = @"ab_c123_d";
            
            AssertRenderReturnSameString(rawString);
        }
        
        [Test]
        public void NotMarkWhenMarkingSymbolBeenEscaped()
        {
            var rawString = @"\_hello\_";
            var expected = "_hello_";
            
            AssertRenderIsCorrect(rawString, expected);
        }

        [Test]
        public void MarkWhen_TagWasInsideOneWord()
        {
            var rawString = "h_ell_o";
            var expected = "h<em>ell</em>o";
            
            AssertRenderIsCorrect(rawString, expected);
        }


        [TestCase("[MyLink](vk.com)", "<a href=\"vk.com\">MyLink</a>", TestName = "Only one link given")]
        [TestCase("[My site](vk.com)",
            "<a href=\"vk.com\">My site</a>",
            TestName = "Only link header contains multiplied words")]
        public void ReturnLinkedValue_When(string rawString, string expected)
        {
            AssertRenderIsCorrect(rawString, expected);
        }

        [TestCase("[Link(vk.com)]", TestName = "URL was inside header")]
        [TestCase("[Link]( vk.com)", TestName = "Space was inside URL")]
        [TestCase("[Link]\n\n(vk.com)", TestName = "URL was in different paragraph")]
        [TestCase("[]](vk.com)", TestName = "Header was closed twice")]
        [TestCase("[Link]((vk.com)", TestName = "URL was opened twice")]
        public void ReturnUnlinkedValue_When(string rawString)
        {
            AssertRenderReturnSameString(rawString);
        }

        [Test]
        public void AllowMarkingInsideLinkHeader()
        {
            var rawString = "[Hello _my_ world](vk.com)";
            var expected = "<a href=\"vk.com\">Hello <em>my</em> world</a>";

            AssertRenderIsCorrect(rawString, expected);
        }

        [Test]
        public void NotMarkWhen_OpenedTagWasInsideOneWordAndClosingInsideOtherWord()
        {
            var rawString = "h_ello worl_d";
            
            AssertRenderReturnSameString(rawString);
        }

        [Test]
        public void ContainsNewLineItDoesNotCloseParagraph()
        {
            var rawString = "abc\ndef";

            AssertRenderReturnSameString(rawString);
        }

        [Test]
        public void NotMarkWhen_TagWasInsideNumber()
        {
            var rawString = "1_2_3";
            var expected = "1_2_3";
            
            AssertRenderIsCorrect(rawString, expected);
        }

        [Test]
        public void HeadFirstHeader_WhenMultipliedHeadersGiven()
        {
            var rawString = "# # hello";
            var expected = "<h1># hello</h1>";
            
            AssertRenderIsCorrect(rawString, expected);
        }

        private void AssertRenderIsCorrect(string rawString, string expected)
        {
            var result = markdown.Render(rawString);

            result.Should().Be(expected);
        }

        private void AssertRenderReturnSameString(string rawString)
        {
            AssertRenderIsCorrect(rawString, rawString);
        }
    }
}