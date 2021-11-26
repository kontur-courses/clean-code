using NUnit.Framework;
using FluentAssertions;
using Markdown;
using System.Collections.Generic;

namespace MarkdownTest
{
    public class StringToTermParserTests
    {
        private List<char> serviceSymbols = new List<char>() { '_', '#', '\\' };

        [TestCase("as_aa_", 4, TestName = "service symbols and text between them should be terms")]
        public void CheckMethod(string input, int resultCount)
        {
            new StringToTermParser(serviceSymbols).ParseByServiceSymbols(input).Should().HaveCount(resultCount);
        }
    }
}
