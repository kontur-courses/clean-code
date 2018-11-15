using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class RulesCheckerTests
    {
        [Test]
        public void RulesChecker_DoubleUnderscoreInsideUnderscore_JustUnderscore()
        {
            var tagPosition = new SortedDictionary<int, SymbolType>
            {
                {1, SymbolType.Underscore},
                {3, SymbolType.DoubleUnderscore},
                {6, SymbolType.DoubleUnderscore},
                {10, SymbolType.Underscore}
            };
            var expectedTagPosition = new SortedDictionary<int, string>
            {
                {1, "<em>"},
                {10, "</em>"}
            };
            var checker = new RulesChecker();
            var actualTagPosition = checker.CheckCorrectness(tagPosition);
            actualTagPosition.Should().BeEquivalentTo(expectedTagPosition);
        }


        [Test]
        public void RulesChecker_EmptyDictionary_EmptyDictionary()
        {
            var tagPosition = new SortedDictionary<int, SymbolType>();
            var checker = new RulesChecker();
            var correctTagPosition = checker.CheckCorrectness(tagPosition);
            correctTagPosition.Should().BeEmpty();
        }

        [Test]
        public void RulesChecker_HaveBackSlash_ShieldingSymbolsAfterThem()
        {
            var tagPosition = new SortedDictionary<int, SymbolType>
            {
                {1, SymbolType.Backslash},
                {2, SymbolType.DoubleUnderscore},
                {8, SymbolType.Backslash},
                {9, SymbolType.GraveAccent},
                {10, SymbolType.DoubleUnderscore}
            };
            var expectedTagPosition = new SortedDictionary<int, string>
            {
                {1, "backslash"},
                {8, "backslash"},
                {10, "<strong>"}
            };
            var checker = new RulesChecker();
            var actualTagPosition = checker.CheckCorrectness(tagPosition);
            actualTagPosition.Should().BeEquivalentTo(expectedTagPosition);
        }

        [Test]
        public void RulesChecker_HaveBackslashAwaySymbols_ExactTagPosition()
        {
            var tagPosition = new SortedDictionary<int, SymbolType>
            {
                {1, SymbolType.Backslash},
                {4, SymbolType.DoubleUnderscore},
                {8, SymbolType.Backslash},
                {11, SymbolType.GraveAccent},
                {12, SymbolType.DoubleUnderscore}
            };
            var expectedTagPosition = new SortedDictionary<int, string>
            {
                {1, "backslash"},
                {4, "<strong>"},
                {8, "backslash"},
                {11, "<code>"},
                {12, "</strong>"}
            };
            var checker = new RulesChecker();
            var actualTagPosition = checker.CheckCorrectness(tagPosition);
            actualTagPosition.Should().BeEquivalentTo(expectedTagPosition);
        }
    }
}