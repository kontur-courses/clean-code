using NUnit.Framework;
using FluentAssertions;
using Markdown;
using System.Collections.Generic;

namespace MarkdownTest
{
    public class TermsAnalizatorTests
    {
        private List<char> serviceSymbols = new List<char>() { '_', '#', '\\' };

        [TestCase("_a_", 2, TestName = "Tag is one term")]
        public void CheckMethod(string input, int resultCount)
        {
            var terms = new StringToTermParser(serviceSymbols).ParseByServiceSymbols(input);
            var termAnalizator = new TermsAnalizator(input);

            var resultTerms = termAnalizator.AnalyseTerms(terms);

            resultTerms.Should().HaveCount(resultCount);
        }
    }
}
