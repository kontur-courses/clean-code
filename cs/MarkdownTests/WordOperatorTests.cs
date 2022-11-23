using NUnit.Framework;
using Markdown;
using Markdown.TagClasses;

namespace MarkdownTests
{
    internal class WordOperatorTests
    {
        [TestCase(" aaaa ", 1, ExpectedResult = "aaaa", Description = "Word beginning position")]
        [TestCase(" aaaa ", 5, ExpectedResult = "aaaa", Description = "Word end position")]
        [TestCase(" aaaa ", 3, ExpectedResult = "aaaa", Description = "In word middle position")]
        [TestCase(" aaaa  ", 6, ExpectedResult = "", Description = "Not in word")]
        public string GetWordAtPosition_SendTagPosition_ShouldReturnCorrectWord(string text, int position)
        {
            return WordOperator.GetWordAtPosition(text, position);
        }

        [TestCase("aa1aa", ExpectedResult = true, Description = "Word contains digit")]
        [TestCase("aaaa", ExpectedResult = false, Description = "Word doesn't contains digit")]
        [TestCase("123123", ExpectedResult = true, Description = "Word consist from digits")]
        [TestCase("", ExpectedResult = false, Description = "Not a word")]
        public bool IsWordContainsDigits_ShouldReturnCorrectResult(string word)
        {
            return WordOperator.IsWordContainsDigits(word);
        }

        [TestCase(2, 12, ExpectedResult = true, Description = "Middles of different words")]
        [TestCase(2, 8, ExpectedResult = false, Description = "Middle of same word")]
        [TestCase(3, 4, ExpectedResult = false, Description = "Middle of same word point-blank")]
        [TestCase(1, 9, ExpectedResult = false, Description = "Begginning and end of same word")]
        [TestCase(9, 11, ExpectedResult = true, Description = "Different word between space")]
        public bool InDifferentWords_CheckDiffirentPositions_ShouldDetermineAreTheyInDifferentWords(int firstPosition,
            int secondPosition)
        {
            var text = " 123456789 123456789"; // simplify words for position counting
            var firstTag = new PairedTag(firstPosition, TagType.Strong);
            var secondTag = new PairedTag(secondPosition, TagType.Strong);

            return WordOperator.InDifferentWords(text, firstTag, secondTag);
        }
    }
}