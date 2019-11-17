using System;
using NUnit.Framework;
using FluentAssertions;
using Markdown.SpecialSymbols;

namespace Markdown.Tests.SpecialSymbols_Tests.EscapeSymbol_Tests
{
    class EscapeSymbol_RemoveAllEscapeSymbolEntries_Tests
    {
        [Test]
        public void ShouldThrow_IfAgrumentIsNull()
        {
            Action act = () => EscapeSymbol.RemoveAllEscapeSymbolEntries(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [TestCase("asd", ExpectedResult = "asd")]
        [TestCase("\\asd", ExpectedResult = "asd")]
        [TestCase("zxc\\", ExpectedResult = "zxc")]
        [TestCase("\\\\asd", ExpectedResult = "\\asd")]
        [TestCase("\\\\\\_asd", ExpectedResult = "\\_asd")]
        [TestCase("\\\\\\\\\\\\\\z", ExpectedResult = "\\\\\\z")]
        public string ShouldReturnStringWithoutEscapeSymbols(string text)
        {
            return EscapeSymbol.RemoveAllEscapeSymbolEntries(text);
        }
    }
}