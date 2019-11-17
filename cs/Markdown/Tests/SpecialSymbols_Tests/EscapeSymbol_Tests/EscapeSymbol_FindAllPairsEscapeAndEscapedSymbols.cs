using System;
using NUnit.Framework;
using FluentAssertions;
using Markdown.SpecialSymbols;

namespace Markdown.Tests.SpecialSymbols_Tests.EscapeSymbol_Tests
{
    class EscapeSymbol_FindAllPairsEscapeAndEscapedSymbols
    {
        [Test]
        public void ShouldThrowArgumentNullException_IfArgumentIsNull()
        {
            Action act = () => EscapeSymbol.FindAllPairsEscapeAndEscapedSymbols(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ShouldThrowFormatException_IfEscapeSymbolHaveNoEscapedSymbol()
        {
            Action act = () => EscapeSymbol.FindAllPairsEscapeAndEscapedSymbols("asd\\");

            act.Should().Throw<FormatException>();
        }

        [Test]
        public void ShouldReturnCorrectPairsEscapeAndEscapedSymbols()
        {
            var pairs = EscapeSymbol.FindAllPairsEscapeAndEscapedSymbols("\\azxc\\\\\\swe\\x");

            pairs.Should().BeEquivalentTo(
                (0, 1),
                (5, 6),
                (7, 8),
                (11, 12)
                );
        }

        [TestCase("")]
        [TestCase("asdzx")]
        public void ShouldReturnEmptyEnumeration_IfArgumentStringNotContainsEscapeSymbol(string text)
        {
            var pairs = EscapeSymbol.FindAllPairsEscapeAndEscapedSymbols(text);

            pairs.Should().BeEmpty();
        }
    }
}