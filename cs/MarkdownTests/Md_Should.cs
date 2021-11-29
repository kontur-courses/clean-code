using System;
using NUnit.Framework;
using FluentAssertions;
using Markdown;

namespace MarkdownTests
{
    class Md_Should
    {
        private readonly Md markdown = new Md();

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
        public void ReturnSameStringWhen(string input)
        {
            var result = markdown.Render(input);

            result.Should().Be(input);
        }

        [TestCase("_hello_",
            "<em>hello</em>",
            TestName = "When one word been emphasized")]
        [TestCase("_hello wonderful world_",
            "<em>hello wonderful world</em>",
            TestName = "When one word been emphasized")]
        public void ReturnEmphasizedValueWhen(string input, string expected)
        {
            AssertRenderIsCorrect(input, expected);
        }

        [TestCase("__hello__",
            "<strong>hello</strong>",
            TestName = "When one word been marked")]
        [TestCase("__hello wonderful world__",
            "<strong>hello wonderful world</strong>",
            TestName = "When more than one word been marked")]
        public void ReturnStrongValueWhen(string input, string expected)
        {
            AssertRenderIsCorrect(input, expected);
        }

        [TestCase("# Hello world",
            "<h1>Hello world</h1>",
            TestName = "When paragraph was not closed with \\n\\n")]
        [TestCase("# Hello world\n\n",
            "<h1>Hello world</h1>",
            TestName = "When paragraph was closed with \\n\\n")]
        [TestCase("# Hello world\n\n# It's me",
            "<h1>Hello world</h1><h1>It's me</h1>",
            TestName = "When two different paragraphs contains header")]
        public void ReturnHeadedValueWhen(string input, string expected)
        {
            AssertRenderIsCorrect(input, expected);
        }

        [TestCase("__hello _great_ world__",
            "<strong>hello <em>great</em> world</strong>",
            TestName = "When <em> tag inside <strong>")]
        public void ReturnMarkedValueWithDifferentMarkingTypes(string input, string expected)
        {
            AssertRenderIsCorrect(input, expected);
        }

        [TestCase("_abc", TestName = "When <em> tag was not closed")]
        [TestCase("__abc", TestName = "When <strong> was not closed")]
        [TestCase("_a \n\n b_", TestName = "When closing symbols in different paragraphs")]
        public void IgnoreUnpairedSymbols(string input)
        {
            var result = markdown.Render(input);

            result.Should().NotContain("<strong>");
            result.Should().NotContain("<em>");
        }


        //Падает
        [Test]
        public void NotContainsStrongTag_WhenStrongTagWasInsideEmphasized()
        {
            var input = "_hello __great__ world_";
            var result = markdown.Render(input);

            result.Should().NotContain("<strong>");
        }
        
        [Test]
        public void EscapeEscapingSymbol()
        {
            var rawString = @"\\hello\\";
            var expected = @"\hello\";
            var result = markdown.Render(rawString);

            result.Should().Be(expected);
        }
        
        [Test]
        public void NotMarkTextWhichContainDigits()
        {
            var rawString = @"ab_c123_d";
            var result = markdown.Render(rawString);

            result.Should().Be(rawString);
        }
        
        [Test]
        public void NotMarkWhenMarkingSymbolBeenEscaped()
        {
            var rawString = @"\_hello\_";
            var expected = "_hello_";
            var result = markdown.Render(rawString);

            result.Should().Be(expected);
        }

        [Test]
        public void MarkWhen_TagWasInsideOneWord()
        {
            var input = "h_ell_o";
            var expected = "h<em>ell</em>o";
            AssertRenderIsCorrect(input, expected);
        }


        [TestCase("[MyLink](vk.com)", "<a href=\"vk.com\">MyLink</a>", TestName = "Only one link given")]
        [TestCase("[My site](vk.com)",
            "<a href=\"vk.com\">My site</a>",
            TestName = "Only link header contains multiplied words")]
        public void ReturnLinkedValue_When(string input, string expected)
        {
            AssertRenderIsCorrect(input, expected);
        }

        [TestCase("[Link(vk.com)]", TestName = "URL was inside header")]
        [TestCase("[Link]( vk.com)", TestName = "Space was inside URL")]
        [TestCase("[Link]\n\n(vk.com)", TestName = "URL was in different paragraph")]
        [TestCase("[]](vk.com)", TestName = "Header was closed twice")]
        [TestCase("[Link]((vk.com)", TestName = "URL was opened twice")]
        public void ReturnUnlinkedValue_When(string input)
        {
            var rendered = markdown.Render(input);

            rendered.Should().NotContain("<a href");
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
            var input = "h_ello worl_d";
            var expected = input;
            AssertRenderIsCorrect(input, expected);
        }

        [Test]
        public void ContainsNewLineItDoesNotCloseParagraph()
        {
            var rawString = "abc\ndef";
            var expected = rawString;

            AssertRenderIsCorrect(rawString, expected);
        }

        [Test]
        public void NotMarkWhen_TagWasInsideNumber()
        {
            var input = "1_2_3";
            var expected = "1_2_3";
            AssertRenderIsCorrect(input, expected);
        }

        private void AssertRenderIsCorrect(string input, string expected)
        {
            var result = markdown.Render(input);

            result.Should().StartWith(expected);
        }
    }
}