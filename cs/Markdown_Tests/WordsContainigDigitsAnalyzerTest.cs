using Markdown.Analyzers;
using NUnit.Framework;

namespace Markdown_Tests
{
    class WordsContainingDigitsAnalyzerTest
    {
        [TestCase("1a2", ExpectedResult = new []{true, true, true}, 
            TestName = "set true when symbol in word containing digits")]
        [TestCase("1 a", ExpectedResult = new []{true, false, false},
            TestName = "not set true when symbol separated with whitespace")]
        public bool[] GetMarkersOfIncludingToWordWithDigits_Should(string text)
        {
            return WordsContainingDigitsAnalyzer.GetMarkersOfIncludingToWordWithDigits(text);
        }
    }
}
