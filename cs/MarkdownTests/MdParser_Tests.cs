using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MdParser_Tests
    {
        [TestCase("just text", 1)]
        [TestCase("text _emTag_", 2)]
        [TestCase("text _cutEmTag", 2)]
        [TestCase("text __strongTag__ _cutEmTag", 4)]
        [TestCase("_emStart __strongTagInEmTag__ emFinish_", 1)]
        public void MdParser_GetTokens_ReturnRightTokensCount(string text, int tokensCount)
        {
            new MdParser(text).GetTokens().Count().Should().Be(tokensCount);
        }
        
        [TestCase("just text", 0)]
        [TestCase("text _emTag_", 0, 5)]
        [TestCase("text _cutEmTag", 0, 5)]
        [TestCase("text __strongTag__ _cutEmTag", 0, 5, 18, 19)]
        [TestCase("_emStart __strongTagInEmTag__ emFinish_", 0)]
        public void MdParser_GetTokens_RightTokensPositions(string text, params int[] expectedPositions)
        {
            var tokens = new MdParser(text).GetTokens();
            var positions = tokens.Select(token => token.Position).ToArray();
            positions.Should().BeEquivalentTo(expectedPositions);
        }

        [TestCase("just text", 9)]
        [TestCase("text _emTag_", 5, 7)]
        [TestCase("text _cutEmTag", 5, 9)]
        [TestCase("text __strongTag__ _cutEmTag", 5, 13, 1, 9)]
        [TestCase("_emStart __strongTagInEmTag__ emFinish_", 39)]
        public void MdParser_GetTokens_RightTokensLength(string text, params int[] expectedLengths)
        {
            var tokens = new MdParser(text).GetTokens();
            var positions = tokens.Select(token => token.Length).ToArray();
            positions.Should().BeEquivalentTo(expectedLengths);
        }
        
        /*[TestCase("just text", true)]
        [TestCase("text _emTag_", true, true)]
        [TestCase("text _cutEmTag", true, false)]
        [TestCase("text __strongTag__ _cutEmTag", true, true, true, false)]
        [TestCase("_emStart __strongTagInEmTag__ emFinish_", true)]
        public void MdParser_GetTokens_RightTokensClosingState(string text, params bool[] expectedClosingStates)
        {
            var tokens = new MdParser(text).GetTokens();
            var positions = tokens.Select(token => token.IsClosed).ToArray();
            positions.Should().BeEquivalentTo(expectedClosingStates);
        }
        
        [TestCase("just text", true)]
        [TestCase("text _emTag_", true, true)]
        [TestCase("text _cutEmTag", true, true)]
        [TestCase("text __strongTag__ _cutEmTag", true, true, true, true)]
        [TestCase("_emStart __strongTagInEmTag__ emFinish_", true)]
        public void MdParser_GetTokens_RightTokensOpeningState(string text, params bool[] expectedOpeningStates)
        {
            var tokens = new MdParser(text).GetTokens();
            var positions = tokens.Select(token => token.IsClosed).ToArray();
            positions.Should().BeEquivalentTo(expectedOpeningStates);
        }*/
    }
}