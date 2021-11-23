using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using Markdown.Parser;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MdParserTests
    {
        private MdParser sut;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            sut = new MdParser(DefaultTagSets.TokensBySeparator);
        }

        [TestCaseSource(typeof(MdParserTestCases), nameof(MdParserTestCases.ParseTokenTestCases))]
        public void ParseTokenTest(string input, IEnumerable<Token> expectedTokens)
        {
            var tokens = sut.ParseTokens(input);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }
    }
}