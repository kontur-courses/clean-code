using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MdRenderTest
    {
        private readonly Md md = new Md();

        [TestCase("asdf\nasdf", ExpectedResult = "asdf\nasdf", TestName = "NotStyledText")]
        [TestCase("asdf\n#asdf", ExpectedResult = "asdf\n<h1>asdf</h1>", TestName = "HeaderStrings")]
        [TestCase("_asdf_", ExpectedResult = "<em>asdf</em>", TestName = "ItalicTag")]
        [TestCase("__asdf__", ExpectedResult = "<strong>asdf</strong>", TestName = "BoldTag")]
        [TestCase("__one_two_three__", ExpectedResult = "<strong>one<em>two</em>three</strong>", TestName = "ItalicTagInBold")]
        [TestCase("_one__two__three_", ExpectedResult = "<em>one__two__three</em>", TestName = "BoldInItalic")]
        [TestCase(@"one\_two_", ExpectedResult = "one_two_", TestName = "ShieldOpenItalicTag")]
        [TestCase(@"one_two\_", ExpectedResult = "one_two_", TestName = "ShieldCloseItalicTag")]
        [TestCase(@"\#asdf", ExpectedResult = "#asdf", TestName = "ShieldHeaderTag")]
        [TestCase(@"asdf\asdf", ExpectedResult = @"asdf\asdf", TestName = "IgnoreSimpleSlash")]
        [TestCase(@"\__asdf_", ExpectedResult = @"_<em>asdf</em>", TestName = "ShieldOpenBoldTagWithItalic")]
        [TestCase(@"_asdf\__", ExpectedResult = "<em>asdf_</em>", TestName = "ShieldCloseBoldTagWithItalic")]
        [TestCase(@"\__asdf__", ExpectedResult = @"__asdf__", TestName = "ShieldOpenBoldTagWithBold")]
        [TestCase(@"__asdf\__", ExpectedResult = "__asdf__", TestName = "ShieldCloseBoldTagWithBold")]
        [TestCase(@"___", ExpectedResult = "___", TestName = "EmptyStringInTag")]
        [TestCase(@"__as_df__a_", ExpectedResult = "__as_df__a_", TestName = "IntersectBoldAndItalic")]
        [TestCase("__asdf __", ExpectedResult = "__asdf __", TestName = "CloseBoldTagAfterWhitespace")]
        [TestCase("_asdf _", ExpectedResult = "_asdf _", TestName = "CloseItalicTagAfterWhitespace")]
        [TestCase("_as\ndf_", ExpectedResult = "_as\ndf_", TestName = "ItalicTagsInDifferentParagraphes")]
        [TestCase("__as\ndf__", ExpectedResult = "__as\ndf__", TestName = "BoldTagsInDifferentParagraphes")]
        [TestCase("on_e t_wo", ExpectedResult = "on_e t_wo", TestName = "ItalicTagsInDifferentWords")]
        [TestCase("One__e t__wo", ExpectedResult = "One__e t__wo", TestName = "BoldTagsInDifferentWords")]
        [TestCase("123_456_789", ExpectedResult = "123_456_789", TestName = "ItalicTagsInDigits")]
        [TestCase("123__456__789", ExpectedResult = "123__456__789", TestName = "BoldTagsInDigits")]
        [TestCase("* asdf\nasdf", ExpectedResult = "<ul>\n<li>asdf</li>\n</ul>\nasdf", TestName = "UnorderedList")]
        [TestCase("#_one_ __two_three_four__",
            ExpectedResult = "<h1><em>one</em> <strong>two<em>three</em>four</strong></h1>",
            TestName = "HeaderBoldAndItalicTags")]
        
        public string Render(string text)
        {
            return md.Render(text);
        }
    }
}