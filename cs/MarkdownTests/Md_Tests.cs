using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class Md_Tests
    {
        [TestCase("qwerty", ExpectedResult = "qwerty", TestName = "No space")]
        [TestCase("foo bar", ExpectedResult = "foo bar", TestName = "With space")]
        public string ParseWithoutTokens(string markdown)
        {
            return new Md().Render(markdown);
        }

        [TestCase("_qwerty_", ExpectedResult = "<em>qwerty</em>", TestName = "Simple em token")]
        [TestCase("_foo_ bar", ExpectedResult = "<em>foo</em> bar", TestName = "Spaced underscore")]
        [TestCase("foo bar_1_23", ExpectedResult = "foo bar_1_23", TestName = "Non-spaced underscore")]
        [TestCase(@"_foo\_bar_", ExpectedResult = "<em>foo_bar</em>", TestName = "Escaped underscore")]
        [TestCase(@"\_qwerty\_", ExpectedResult = "_qwerty_", TestName = "Escaped underscore 2")] //to be implemented
        public string ParseEmTokens(string markdown)
        {
            return new Md().Render(markdown);
        }

        [TestCase("__foo bar__", ExpectedResult = "<strong>foo bar</strong>", TestName = "Simple strong token (underscores)")]
        [TestCase("foo **bar**", ExpectedResult = "foo <strong>bar</strong>", TestName = "Spaced strong token (asterisks)")]
        [TestCase(@"__foo\__bar__", ExpectedResult = "<strong>foo__bar</strong>", TestName = "Escaped underscores")]
        [TestCase(@"**foo\**bar**", ExpectedResult = "<strong>foo**bar</strong>", TestName = "Escaped asterisks")]
        [TestCase(@"**qwerty__", ExpectedResult = "**qwerty__", TestName = "Mixed strong tokens")] //to be implemented
        [TestCase(@"**foo__bar**", ExpectedResult = "<strong>foo__bar</strong>", TestName = "Mixed strong tokens 2")] //to be implemented
        public string ParseStrongTokens(string markdown)
        {
            return new Md().Render(markdown);
        }

        [TestCase("__f _f_ f__", ExpectedResult = "<strong>f <em>f</em> f</strong>",
            TestName = "Single underscores inside double underscores work")]
        [TestCase("_f __f__ f_", ExpectedResult = "<em>f __f__ f</em>",
            TestName = "Double underscores inside single underscores do not work")]
        [TestCase("_f __f__ __f__ f_", ExpectedResult = "<em>f __f__ __f__ f</em>",
            TestName = "Several double underscores inside single underscores do not work")]
        [TestCase("_f __f__ f", ExpectedResult = "_f <strong>f</strong> f",
            TestName = "Double underscores work if single underscore wrapper is not closed")]
        [TestCase("_f __f__ __f__ f", ExpectedResult = "_f <strong>f</strong> <strong>f</strong> f",
            TestName = "Several double underscores work if single underscore wrapper is not closed")]
        public string ParseMixedEmAndStrongTokens(string markdown)
        {
            return new Md().Render(markdown);
        }
    }
}