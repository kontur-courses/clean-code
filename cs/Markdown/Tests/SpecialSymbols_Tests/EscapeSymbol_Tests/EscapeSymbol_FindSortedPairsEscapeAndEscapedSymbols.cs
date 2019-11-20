using System;
using NUnit.Framework;
using FluentAssertions;
using Markdown.SpecialSymbols;

namespace Markdown.Tests.SpecialSymbols_Tests.EscapeSymbol_Tests
{
    class EscapeSymbol_FindSortedPairsEscapeAndEscapedSymbols
    {
        [Test]
        public void ShouldThrowArgumentNullException_IfArgumentIsNull()
        {
            Action act = () => EscapeSymbol.FindSortedPairsEscapeAndEscapedSymbols(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ShouldThrowFormatException_IfEscapeSymbolHaveNoEscapedSymbol()
        {
            Action act = () => EscapeSymbol.FindSortedPairsEscapeAndEscapedSymbols("asd\\");

            act.Should().Throw<FormatException>();
        }

        [Test]
        public void ShouldReturnCorrectPairsEscapeAndEscapedSymbols()
        {
            var pairs = EscapeSymbol.FindSortedPairsEscapeAndEscapedSymbols("\\azxc\\\\\\swe\\x");

            pairs.Should().BeEquivalentTo(
                (0, 1),
                (5, 6),
                (7, 8),
                (11, 12)
                );
        }

        [Test]
        public void ShouldReturnPairsInAscOrder()
        {
            var pairs = EscapeSymbol.FindSortedPairsEscapeAndEscapedSymbols("\\azxc\\\\\\swe\\x");

            pairs.Should().BeInAscendingOrder(
                p => p.escapeSymbolIndex);
        }

        [TestCase("")]
        [TestCase("asdzx")]
        public void ShouldReturnEmptyEnumeration_IfArgumentStringNotContainsEscapeSymbol(string text)
        {
            var pairs = EscapeSymbol.FindSortedPairsEscapeAndEscapedSymbols(text);

            pairs.Should().BeEmpty();
        }
    }
}