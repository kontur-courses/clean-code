using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class Md_Should
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [TestCase("", "",
            TestName = "Empty string")]

        [TestCase("_word_", "<em>word</em>",
            TestName = "One italic tag")]

        [TestCase("word _ word _ word", "word _ word _ word",
            TestName = "Not render italics if space after opening low line or space before closing low line")]

        [TestCase("word _word_ _word_ _word_ word", "word <em>word</em> <em>word</em> <em>word</em> word", 
            TestName = "Some italic tag")]

        [TestCase("__word__", "<strong>word</strong>", 
            TestName = "One strong tag")]

        [TestCase("word __word__ word", "word <strong>word</strong> word", 
            TestName = "Not render strong if space after opening low line or space before closing low line")]

        [TestCase("word __word__ __word__ __word__ word", "word <strong>word</strong> <strong>word</strong> <strong>word</strong> word", 
            TestName = "More strong tag")]

        [TestCase("word _word word", "word _word word", 
            TestName = "Low line has no closing pair")]

        [TestCase("word word__ word", "word word__ word", 
            TestName = "Double low line has no opening pair")]

        [TestCase("word __word _word_ word__ word", "word <strong>word <em>word</em> word</strong> word", 
            TestName = "Italic tag in strong tag")]

        [TestCase("word _word __word__ word_ word", "word <em>word __word__ word</em> word", 
            TestName = "Double low line in italic tag")]

        [TestCase("word _1_1_1_1_ word", "word _1_1_1_1_ word", 
            TestName = "Not render italics within digits")]

        [TestCase(@"word \_word\_ word", @"word _word_ word", 
            TestName = "Escaping low line")]

        [TestCase(@"word \__word\__ word", @"word __word__ word", 
            TestName = "Escaping double low line")]

        [TestCase(@"word \\ word", @"word \ word", 
            TestName = "Escaping escape char")]
        public void Return(string input, string expected)
        {
            md.Render(input).Should().Be(expected);
        }
    }
}
