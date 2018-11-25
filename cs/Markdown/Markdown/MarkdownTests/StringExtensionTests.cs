using System;
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
            var str = new string('a', 100000);

            Action act = () => str.ClearFromSymbol(toDelete);

            act.ExecutionTime().ShouldNotExceed(30.Milliseconds());
        }
    }
}
