using System.Collections.Generic;
using Markdown.Core.Readers;
using Markdown.Core.Tokens;
using NUnit.Framework;
using FluentAssertions;

namespace MarkdownTests.TokenReaderTests
{
    [TestFixture]
    public class ParagraphTokenReaderTests
    {
        private readonly ParagraphTokenReader paragraphTokenReader = new ParagraphTokenReader();
        
        [TestCaseSource(typeof(TokenReaderTestsData), nameof(TokenReaderTestsData.ReaderTestCases))]
        public void ReadTokensShouldReturnCorrectTokens(string source, List<Token> expectedTokens)
        {
            IsSameTokenLists(paragraphTokenReader.ReadTokens(source), expectedTokens).Should().BeTrue();  
        }

        private bool IsSameTokenLists(List<Token> firstTokens, List<Token> secondTokens)
        {
            if (firstTokens.Count != secondTokens.Count)
                return false;

            for (var i = 0; i < firstTokens.Count; i++)
            {
                var first = firstTokens[i];
                var second = secondTokens[i];
                if (!first.Equals(second))
                    return false;
            }

            return true;
        }
    }
}