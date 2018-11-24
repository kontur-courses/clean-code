using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    class StringExtensionTests
    {
        [TestCase("", ExpectedResult = "")]
        [TestCase("/", ExpectedResult = "")]
        [TestCase("///", ExpectedResult = "")]
        [TestCase("/Abc", ExpectedResult = "Abc")]
        [TestCase("Abc", ExpectedResult = "Abc")]
        [TestCase("A/b/c/d", ExpectedResult = "Abcd")]
        [TestCase(null, ExpectedResult = null)]
        public string ClearFromSymbolShouldClearFromCharToDelete(string str, char toDelete='/')
        {
            return str.ClearFromSymbol(toDelete);
        }

        [TestCase()]
        public void ClearFromSymbolComplexityShouldBeLinear(char toDelete = '/')
        {
            var str = GetAVeryLongString();

            Action act = () => str.ClearFromSymbol(toDelete);

            act.ExecutionTime().ShouldNotExceed(30.Milliseconds());
        }

        private static string GetAVeryLongString()
        {
            var builder = new StringBuilder();

            for (var i = 0; i < 100000; i++)
                builder.Append('a');

            return builder.ToString();
        }
    }
}
