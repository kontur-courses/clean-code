using NUnit.Framework;
using FluentAssertions;
using Markdown;

namespace MarkdownTests
{
    class Md_Should
    {
        private readonly Md markdown = new Md();

        [TestCase("", TestName = "When input is empty string")]
        [TestCase("abcDEF", TestName = "When input does not contains tags")]
        [TestCase("__hello _great__ world_", TestName = "When different tags was crossed")] //Этот тест упадёт
        public void ReturnSameStringWhen(string input)
        {
            var result = markdown.Render(input);

            result.Should().Be(input);
        }
        
        [TestCase("_hello_", "\\<em>hello\\</em>", 
            TestName = "When one word been emphasized")]
        [TestCase("_hello wonderful world_", "\\<em>hello wonderful world\\</em>",
            TestName = "When one word been emphasized")]
        public void ReturnEmphasizedValueWhen(string input, string expected)
        {
            AssertRenderIsCorrect(input, expected);
        }

        [TestCase("__hello__", "\\<strong>hello\\</strong>",
            TestName = "When one word been marked")]
        [TestCase("__hello wonderful world__", "\\<strong>hello wonderful world\\</strong>",
            TestName = "When one word been marked")]
        public void ReturnStrongValueWhen(string input, string expected)
        {
            AssertRenderIsCorrect(input, expected);
        }
        
        //Эти тесты упадут так как у меня после <h1> всегда приписывается ' '
        [TestCase("# Hello world", 
            "\\<h1>Hello world\\</h1>", 
            TestName = "When paragraph was not closed with \\n\\n")]
        [TestCase("# Hello world\n\n", 
            "\\<h1>Hello world\\</h1>", 
            TestName = "When paragraph was closed with \\n\\n")]
        [TestCase("# Hello world\n\n# Have a good day!", 
            "\\<h1>Hello world\\</h1>\\</h1>Have a good day!\\</h1>", 
            TestName = "When two different paragraphs contains header")]
        public void ReturnHeadedValueWhen(string input, string expected)
        {
            AssertRenderIsCorrect(input, expected);
        }
        
        [TestCase("__hello _great_ world__",
            "\\<strong>hello \\<em>great\\</em> world\\</strong>",
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

            result.Should().Be(input);
        }

        [Test]
        public void NotContainsStrongTag_WhenStrongTagWasInsideEmphasized()
        {
            var input = "_hello __great__ world_";
            var result = markdown.Render(input);

            result.Should().NotContain("strong");
        }

        [Test]
        public void MarkWhen_TagWasInsideOneWord()
        {
            var input = "h_ell_o";
            var expected = "h\\<em>ell\\</em>o";
            AssertRenderIsCorrect(input, expected);
        }
        
        //Этот тест упадёт
        [Test]
        public void NotMarkWhen_OpenedTagWasInsideOneWordAndClosingInsideOtherWord()
        {
            var input = "h_ello worl_d";
            var expected = input;
            AssertRenderIsCorrect(input, expected);
        }
       
        //Этот тест упадёт
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

            result.Should().Be(expected);
        }
    }
}