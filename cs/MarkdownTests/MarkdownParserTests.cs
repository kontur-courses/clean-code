using FluentAssertions;
using Markdown.Parsers;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkdownParserTests
    {
        private MarkdownParser sut;
        private static readonly HashSet<string> tagsSymbols = new HashSet<string>
        {
            "_", "__", "# ", "\n", "\\"
        };

        [SetUp]
        public void SetUp()
        {
            sut = new MarkdownParser(tagsSymbols);
        }

        [TestCase("", TestName = "ParseText_EmptyString_ThrowsArgumentException")]
        [TestCase(null, TestName = "ParseText_StringIsNull_ThrowsArgumentException")]
        public void ParseText_EmptyString_ThrowsArgumentException(string text)
        {
            var action = () => sut.ParseText(text).First();
            action.Should().Throw<ArgumentException>();
        }

        [TestCaseSource(typeof(MarkdownParserTestData), nameof(MarkdownParserTestData.TestData))]
        public void ParseText_WithDifferentCases_ReturnsCorrectTokens(string text, IEnumerable<Token> expected)
        {
            var result = sut.ParseText(text);
            result.Should().BeEquivalentTo(expected);
        }
    }
}