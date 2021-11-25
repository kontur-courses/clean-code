using NUnit.Framework;
using FluentAssertions;
using Markdown;
using System.Collections.Generic;

namespace MarkdownTest
{
    public class StringToTermParserTests
    {
        private List<char> serviceSymbols = new List<char>() { '_', '#', '\\' };

        [TestCase("as_aa_", 4, TestName = "rr")]
        public void CheckMethod(string input, int resultCount)
        {
            new StringToTermParser(input, serviceSymbols).ParseByServiceSymbols().Should().HaveCount(resultCount);
        }
    }
}
