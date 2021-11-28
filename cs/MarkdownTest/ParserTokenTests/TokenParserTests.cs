using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using Markdown.TokenParser;
using NUnit.Framework;

namespace MarkdownTest.ParserTokenTests
{
    public class TokenParserTests
    {
        private TokenParser tokenParser;
        
        [SetUp]
        public void SetUp()
        {
            tokenParser = new TokenParser();
        }
    
        [TestCaseSource(typeof(ParseTokenSourceData), nameof(ParseTokenSourceData.Italics))]
       [TestCaseSource(typeof(ParseTokenSourceData), nameof(ParseTokenSourceData.Strong))]
        [TestCaseSource(typeof(ParseTokenSourceData), nameof(ParseTokenSourceData.ItalicsAndStrong))]
        public void Parse_ShouldReturn(IEnumerable<IToken> tokens, TokenTree[] expected)
        {
            var actual = tokenParser.Parse(tokens);
            
            actual.Should().BeEquivalentTo(expected, 
                options => options
                    .RespectingRuntimeTypes());
        }
    }
}