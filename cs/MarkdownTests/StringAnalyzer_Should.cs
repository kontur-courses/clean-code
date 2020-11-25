using FluentAssertions;
using Markdown.Core;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class StringAnalyzer_Should
    {
        [TestCase(-1)]
        [TestCase(5)]
        [TestCase(6)]
        public void IsCharInsideString_IncorrectIndexes_False(int index) =>
            new StringAnalyzer("_test").IsCharInsideValue(index).Should().BeFalse();

        [TestCase(0)]
        [TestCase(4)]
        public void IsCharInsideString_CorrectIndexes_True(int index) =>
            new StringAnalyzer("_test").IsCharInsideValue(index).Should().BeTrue();

        [TestCase("test case", 4)]
        [TestCase("test ", 4)]
        [TestCase(" test", 0)]
        public void HasValueWhiteSpace_OnSpaceChars_True(string input, int index) =>
            new StringAnalyzer(input).HasValueWhiteSpaceAt(index).Should().BeTrue();

        [TestCase("test case", -1)]
        [TestCase("test ", 3)]
        [TestCase(" test", 2)]
        public void HasValueWhiteSpace_OnSpaceChars_False(string input, int index) =>
            new StringAnalyzer(input).HasValueWhiteSpaceAt(index).Should().BeFalse();

        [TestCase("te_st", 0)]
        [TestCase("test_", 3)]
        [TestCase("_test", 5)]
        public void HasValueUnderscoreAt_OnInCorrectValues_False(string input, int index) =>
            new StringAnalyzer(input).HasValueUnderscoreAt(index).Should().BeFalse();
        
        [TestCase("te_st", 2)]
        [TestCase("test_", 4)]
        [TestCase("_test", 0)]
        public void HasValueUnderscoreAt_OnInCorrectValues_True(string input, int index) =>
            new StringAnalyzer(input).HasValueUnderscoreAt(index).Should().BeTrue();
        
        [TestCase("_te_st phrase", 0, -1)]
        [TestCase("__te__st phrase", -1, 0)]
        [TestCase("te_st", -1, -1)]
        [TestCase("te__st__", 2, 7)]
        [TestCase("te_st_", 2, 5)]
        public void HasValueSelectionPartWordInDifferentWords_OnInCorrectValues_False(
            string input, 
            int startIndex, 
            int endIndex
        ) => new StringAnalyzer(input).HasValueSelectionPartWordInDifferentWords(startIndex, endIndex).Should().BeFalse();
        
        [TestCase("te_st phr_se", 2, 9)]
        [TestCase("te__st phra__se", 2, 9)]
        [TestCase("ba_r foo_te_st", 2, 7)]
        public void HasValueSelectionPartWordInDifferentWords_OnCorrectValues_True(
            string input, 
            int startIndex, 
            int endIndex
        ) => new StringAnalyzer(input).HasValueSelectionPartWordInDifferentWords(startIndex, endIndex).Should().BeTrue();
    }
}