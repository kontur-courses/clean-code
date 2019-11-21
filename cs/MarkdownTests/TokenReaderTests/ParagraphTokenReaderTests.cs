using System.Collections.Generic;
using Markdown.Core.Readers;
using Markdown.Core.Tokens;
using NUnit.Framework;
using FluentAssertions;
using MarkdownTests.Infrastructure;

namespace MarkdownTests.TokenReaderTests
{
    [TestFixture]
    public class ParagraphTokenReaderTests
    {
        private readonly ParagraphTokenReader paragraphTokenReader = new ParagraphTokenReader();
        
        [TestCaseSource(typeof(TokenReaderTestsData), nameof(TokenReaderTestsData.ReaderTestCases))]
        public void ReadTokensShouldReturnCorrectTokens(string source, List<Token> expectedTokens)
        {
            TestUtils.IsSameTokenLists(paragraphTokenReader.ReadTokens(source), expectedTokens).Should().BeTrue();  
        }
    }
}