using System;
using System.Collections.Generic;
using FluentAssertions;
using Markdown.Parser;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MdParserTests
    {
        public readonly IReadOnlyDictionary<string, Func<int, Token>> TokensBySeparator = new Dictionary<string, Func<int, Token>>
        {
            { ItalicToken.Separator, index => new ItalicToken(index) },
            { BoldToken.Separator, index => new BoldToken(index) },
            { HeaderToken.Separator, index => new HeaderToken(index) },
            { ScreeningToken.Separator, index => new ScreeningToken(index) },
            { ImageToken.Separator, index => new ImageToken(index) }
        };

        private MdParser sut;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            sut = new MdParser(TokensBySeparator);
        }

        [TestCaseSource(typeof(MdParserTestCases), nameof(MdParserTestCases.ParseTokenTestCases))]
        public void ParseTokenTest(string input, IEnumerable<Token> expectedTokens)
        {
            var tokens = sut.ParseTokens(input);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }
    }
}