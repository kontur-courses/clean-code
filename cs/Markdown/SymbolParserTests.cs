using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class SymbolParserTests
    {
        [TestCase("_", SymbolType.Underscore, TestName = "Underscore")]
        [TestCase("\\", SymbolType.Backslash, TestName = "Backslash")]
        [TestCase("`", SymbolType.GraveAccent, TestName = "GraveAccent")]
        [TestCase("__", SymbolType.DoubleUnderscore, TestName = "DoubleUnderscore")]
        public void GetTagsPosition_ReturnCorrectType(string mdSymbol, SymbolType expectedSymbol)
        {
            var parser = new SymbolParser();
            var parsedSymbol = parser.GetTagsPosition(mdSymbol);
            parsedSymbol.Values.First().Should().Be(expectedSymbol);
        }

        [Test]
        public void GetTagPosition_WithoutSpecialSymbols_EmptyDictionary()
        {
            var mdText = "!hello/.*=its-me. +i &was       &wondering $...@ ";
            var parser = new SymbolParser();
            var tagsPosition = parser.GetTagsPosition(mdText);
            tagsPosition.Should().BeEmpty();
        }

        [Test]
        public void GetTagsPosition_CorrectTagsPosition()
        {
            var mdText = "\\i _ love `potato __";
            var parser = new SymbolParser();
            var tagsPosition = parser.GetTagsPosition(mdText);
            var expectedTag = new SortedDictionary<int, SymbolType>
            {
                {mdText.IndexOf('\\'), SymbolType.Backslash},
                {mdText.IndexOf('_'), SymbolType.Underscore},
                {mdText.IndexOf('`'), SymbolType.GraveAccent},
                {mdText.IndexOf("__"), SymbolType.DoubleUnderscore}
            };
            tagsPosition.Should().BeEquivalentTo(expectedTag);
        }
    }
}