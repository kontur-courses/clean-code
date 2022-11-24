using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
    public class CharacterLevelTokenizerTest
    {
        [Test] 
         
        [TestCase(" ", " ", FirstLevelTokenType.Space)]
        [TestCase("_", "_", FirstLevelTokenType.Underscore)]
        [TestCase("#", "#", FirstLevelTokenType.Lattice)]
        [TestCase("\\", "\\", FirstLevelTokenType.Backslash)]
        [TestCase("t", "t", FirstLevelTokenType.String)]
        [TestCase("text", "text", FirstLevelTokenType.String)]
        [TestCase("t ", "t", FirstLevelTokenType.String)]
        [TestCase("t_", "t", FirstLevelTokenType.String)]
        [TestCase("t#", "t", FirstLevelTokenType.String)]
        [TestCase("t\\", "t", FirstLevelTokenType.String)]
        [TestCase("  ", " ", FirstLevelTokenType.Space)]
        [TestCase("__", "_", FirstLevelTokenType.Underscore)]
        [TestCase("##", "#", FirstLevelTokenType.Lattice)]
        [TestCase("\\\\", "\\", FirstLevelTokenType.Backslash)]
        [TestCase("t1234", "t1234", FirstLevelTokenType.StringWithNumbers)]
        
        public void Tokenizer_CheckFirstToken(string inputText, string resultTokenValue,
            FirstLevelTokenType levelTokenType)
        {
            var tokenizer = new CharacterLevelTokenizer();
            var characterTokenList = tokenizer.Tokenize(inputText);
            characterTokenList[0].GetTokenValue().Should().Be(resultTokenValue);
            characterTokenList[0].GetFirstTokenType().Should().Be(levelTokenType);
        }
        
        [Test]
        public void Tokenizer_CheckListWithAllFirstLevelTokenTypes_AllEquals()
        {
            var tokenizer = new CharacterLevelTokenizer();
            var text = " #_\\text t123";
            var characterTokenList = tokenizer.Tokenize(text);
            characterTokenList[0].GetTokenValue().Should().Be(" ");
            characterTokenList[0].GetFirstTokenType().Should().Be(FirstLevelTokenType.Space);
            characterTokenList[1].GetTokenValue().Should().Be("#");
            characterTokenList[1].GetFirstTokenType().Should().Be(FirstLevelTokenType.Lattice);
            characterTokenList[2].GetTokenValue().Should().Be("_");
            characterTokenList[2].GetFirstTokenType().Should().Be(FirstLevelTokenType.Underscore);
            characterTokenList[3].GetTokenValue().Should().Be("\\");
            characterTokenList[3].GetFirstTokenType().Should().Be(FirstLevelTokenType.Backslash);
            characterTokenList[4].GetTokenValue().Should().Be("text");
            characterTokenList[4].GetFirstTokenType().Should().Be(FirstLevelTokenType.String);
            characterTokenList[5].GetTokenValue().Should().Be(" ");
            characterTokenList[5].GetFirstTokenType().Should().Be(FirstLevelTokenType.Space);
            characterTokenList[6].GetTokenValue().Should().Be("t123");
            characterTokenList[6].GetFirstTokenType().Should().Be(FirstLevelTokenType.StringWithNumbers);
        }
    }
}