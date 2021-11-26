using NUnit.Framework;
using FluentAssertions;
using Markdown;
using System.Collections.Generic;

namespace MarkdownTest
{
    public class TermToHtmlParserTests
    {
        private List<char> serviceSymbols = new List<char>() { '_', '#', '\\' };
        Dictionary<string, string> serviceSymbolHtml = new Dictionary<string, string>
            {
                {"#","h1"},
                {"_","em"},
                {"__","strong"}
            };

        [TestCase("_a_","<em>a</em>", TestName = "Word in italic tag when italic term is closing")]
        public void CheckMethod(string input, string expectedResult)
        {
            var terms = new StringToTermParser(serviceSymbols).ParseByServiceSymbols(input);
            var resultTerms = new TermsAnalizator(input).AnalyseTerms(terms);
            var result = TermsToHtmlParser.ParseTermsToHtml(resultTerms, input, serviceSymbolHtml);

            result.Should().Be(expectedResult);
        }
    }
}
