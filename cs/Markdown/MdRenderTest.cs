using NUnit.Framework;

namespace Markdown
{
    public class MdRenderTest
    {
        private Md md = new Md();

        [TestCase("asdf\nasdf", ExpectedResult = "asdfasdf", TestName = "NotStyledText")]
        [TestCase("asdf\n#asdf", ExpectedResult = "asdf<h1>asdf</h1>", TestName = "HeaderStrings")]
        [TestCase("_asdf_", ExpectedResult = "<em>asdf</em>", TestName = "ItalicToken")]
        [TestCase("__asdf__", ExpectedResult = "<strong>asdf</strong>", TestName = "BoldToken")]
        [TestCase("__one_two_three__", ExpectedResult = "<strong>one<em>two</em>three</strong>", TestName = "ItalicTokenInBold")]
        [TestCase("_one__two__three_", ExpectedResult = "<em>one__two__three</em>", TestName = "BoldInItalic")]
        [TestCase("#_one_ __two _three_ four__",
            ExpectedResult = "<h1><em>one</em> <strong>two <em>three</em> four</strong></h1>",
            TestName = "HeaderBoldAndItalicTokens")]
        [TestCase(@"one\_two_", ExpectedResult = "one_two_", TestName = "ShieldOpenItalicToken")]
        [TestCase(@"one_two\_", ExpectedResult = "one_two_", TestName = "ShieldCloseItalicToken")]
        [TestCase(@"\#asdf", ExpectedResult = "#asdf", TestName = "ShieldHeaderToken")]
        [TestCase(@"asdf\asdf", ExpectedResult = @"asdf\asdf", TestName = "IgnoreSimpleSlash")]
        public string Render(string text)
        {
            return md.Render(text);
        }
    }
}