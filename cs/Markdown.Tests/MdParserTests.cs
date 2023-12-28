using Markdown.Parsers;
using Markdown.Serializers;

namespace Markdown.Tests
{
    public class MdParserTests
    {
        private Md renderer;

        [SetUp]
        public void Setup()
        {
            renderer = new Md(new MdParser(new Tokenizer()), new HtmlTokenSerializer());
        }

        [TestCase("abc", ExpectedResult = "abc", TestName = "{m}_WithoutTags")]
        [TestCase("_a_", ExpectedResult = "<em>a</em>", TestName = "{m}_SingleItalic")]
        [TestCase("__a__", ExpectedResult = "<strong>a</strong>", TestName = "{m}_SingleBold")]
        [TestCase("# a", ExpectedResult = "<h1>a</h1>", TestName = "{m}_Header")]
        [TestCase("# a\nb", ExpectedResult = "<h1>a</h1>b", TestName = "{m}_HeaderOnOneLine")]
        [TestCase("__a _b_ c__", ExpectedResult = "<strong>a <em>b</em> c</strong>", 
            TestName = "{m}_ItalicInBold")]
        [TestCase("a_b_c", ExpectedResult = "a<em>b</em>c",TestName = "{m}_ItalicInWord")]
        [TestCase("a__b__c", ExpectedResult = "a<strong>b</strong>c",TestName = "{m}_BoldInWord")]
        [TestCase("_a_bc", ExpectedResult = "<em>a</em>bc",TestName = "{m}_ItalicStartOfWord")]
        [TestCase("__a__bc", ExpectedResult = "<strong>a</strong>bc",
            TestName = "{m}_BoldStartOfWord")]
        [TestCase("ab_c_", ExpectedResult = "ab<em>c</em>",TestName = "{m}_ItalicEndOfWord")]
        [TestCase("ab__c__", ExpectedResult = "ab<strong>c</strong>",TestName = "{m}_BoldEndOfWord")]
        public string Render_CorrectTags_ShouldRender(string markup)
        {
            var res = renderer.Render(markup);
            return res;
        }

        [TestCase("_a", ExpectedResult = "_a", TestName = "{m}_NotClosed")]
        [TestCase("_a__", ExpectedResult = "_a__", TestName = "{m}_DifferentTags")]
        [TestCase("__a _b__ c_", ExpectedResult = "__a _b__ c_",
            TestName = "{m}_IntersectingTagsBoldFirst")]
        [TestCase("_a __b_ c__", ExpectedResult = "_a __b_ c__",
            TestName = "{m}_IntersectingTagsItalicFirst")]
        [TestCase("a_1_b", ExpectedResult = "a_1_b", TestName = "{m}_InWordWithDigitsItalic")]
        [TestCase("a__1__b", ExpectedResult = "a__1__b", TestName = "{m}_InWordWithDigitsBold")]
        [TestCase("a_a b_b", ExpectedResult = "a_a b_b", TestName = "{m}_DifferentWordsItalic")]
        [TestCase("a__a b__b", ExpectedResult = "a__a b__b", TestName = "{m}_DifferentWordsBold")]
        [TestCase("__", ExpectedResult = "__", TestName = "{m}_EmptyItalic")]
        [TestCase("____", ExpectedResult = "____", TestName = "{m}_EmptyBold")]
        [TestCase("_a __b__ c_", ExpectedResult = "<em>a __b__ c</em>",
            TestName = "{m}_BoldInItalic")]
        [TestCase("_ a_", ExpectedResult = "_ a_", TestName = "{m}_NextSymbolNotSpaceItalic")]
        [TestCase("__ a__", ExpectedResult = "__ a__", TestName = "{m}_NextSymbolNotSpaceBold")]
        [TestCase("_ a_", ExpectedResult = "_ a_", TestName = "{m}_NextSymbolSpaceItalic")]
        [TestCase("__ a__", ExpectedResult = "__ a__", TestName = "{m}_NextSymbolSpaceBold")]
        [TestCase("__\ta__", ExpectedResult = "__\ta__", TestName = "{m}_NextSymbolTabBold")]
        [TestCase("_\ta_", ExpectedResult = "_\ta_", TestName = "{m}_NextSymbolTabBold")]
        public string Render_IncorrectTags_ShouldNotRender(string markup)
        {
            return renderer.Render(markup);
        }

        [TestCase(@"\_ a_", ExpectedResult = @"_ a_", TestName = "{m}_EscapeItalic")]
        [TestCase(@"\__ a__", ExpectedResult = @"__ a__", TestName = "{m}_EscapeBold")]
        [TestCase(@"\# a", ExpectedResult = @"# a", TestName = "{m}_EscapeHeader")]
        [TestCase(@"\\a", ExpectedResult = @"\a", TestName = "{m}_EscapeEscape")]
        [TestCase(@"\\_a_", ExpectedResult = @"\<em>a</em>", TestName = "{m}_EscapeSelfBeforeTag")]
        public string Render_CorrectEscapes_ShouldEscape(string markup)
        {
            return renderer.Render(markup);
        }

        [TestCase(@"\b", ExpectedResult = @"\b", TestName = "{m}_EscapeBeforeNonSepcSymbol")]
        public string Render_CorrectEscapes_ShouldNotEscape(string markup)
        {
            return renderer.Render(markup);
        }
    }
}