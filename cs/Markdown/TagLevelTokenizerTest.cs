using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
    public class TagLevelTokenizerTest
    {
        [TestCase("text", FirstLevelTokenType.String, 
            "text", SecondLevelTokenType.String)]
        [TestCase("#", FirstLevelTokenType.Lattice, 
            "#", SecondLevelTokenType.Header)]
        [TestCase(" ", FirstLevelTokenType.Space, 
            " ", SecondLevelTokenType.Space)]
        [TestCase("\\", FirstLevelTokenType.String, 
            "\\", SecondLevelTokenType.String)]
        public void Tokenize_CheckNotItalicsAndNotBoldOneWord_True(string firstTokenValue,
            FirstLevelTokenType firstLevelTokenType,
            string resultTokenValue,
            SecondLevelTokenType resultingTagType)
        {
            var tagLevelTokenizer = new TagLevelTokenizer();
            var characterLevelList = new List<FirstLevelToken>();
            characterLevelList.Add(new FirstLevelToken(firstTokenValue, firstLevelTokenType));
            var tagLevelList = tagLevelTokenizer.Tokenize(characterLevelList);
            tagLevelList[0].GetTokenValue().Should().Be(resultTokenValue);
            tagLevelList[0].GetSecondTokenType().Should().Be(resultingTagType);
        }

        [Test]
        public void Tokenize_OpeningItalics_StartLine()
        {
            
            var tagLevelTokenizer = new TagLevelTokenizer();
            var characterLevelList = new List<FirstLevelToken>();
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            characterLevelList.Add(new FirstLevelToken("t", FirstLevelTokenType.String));
            var tagLevelList = tagLevelTokenizer.Tokenize(characterLevelList);
            tagLevelList[0].GetTokenValue().Should().Be("_");
            tagLevelList[0].GetSecondTokenType().Should().Be(SecondLevelTokenType.OpeningItalics);
        }
        
        [Test]
        public void Tokenize_OpeningItalics_MiddleOfLine()
        {
            
            var tagLevelTokenizer = new TagLevelTokenizer();
            var characterLevelList = new List<FirstLevelToken>();
            characterLevelList.Add(new FirstLevelToken(" ", FirstLevelTokenType.Space));
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            characterLevelList.Add(new FirstLevelToken("t", FirstLevelTokenType.String));
            var tagLevelList = tagLevelTokenizer.Tokenize(characterLevelList);
            tagLevelList[1].GetTokenValue().Should().Be("_");
            tagLevelList[1].GetSecondTokenType().Should().Be(SecondLevelTokenType.OpeningItalics);
        }
        
        [Test]
        public void Tokenize_CloseItalics_EndOfLine()
        {
            
            var tagLevelTokenizer = new TagLevelTokenizer();
            var characterLevelList = new List<FirstLevelToken>();
            characterLevelList.Add(new FirstLevelToken("t", FirstLevelTokenType.String));
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            var tagLevelList = tagLevelTokenizer.Tokenize(characterLevelList);
            tagLevelList[1].GetTokenValue().Should().Be("_");
            tagLevelList[1].GetSecondTokenType().Should().Be(SecondLevelTokenType.ClosingItalics);
        }
        
        [Test]
        public void Tokenize_CloseItalics_MiddleOfLine()
        {
            
            var tagLevelTokenizer = new TagLevelTokenizer();
            var characterLevelList = new List<FirstLevelToken>();
            characterLevelList.Add(new FirstLevelToken("t", FirstLevelTokenType.String));
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            characterLevelList.Add(new FirstLevelToken(" ", FirstLevelTokenType.Space));
            var tagLevelList = tagLevelTokenizer.Tokenize(characterLevelList);
            tagLevelList[1].GetTokenValue().Should().Be("_");
            tagLevelList[1].GetSecondTokenType().Should().Be(SecondLevelTokenType.ClosingItalics);
        }
        
        [Test]
        public void Tokenize_OpenCloseItalics_MiddleOfLine()
        {
            
            var tagLevelTokenizer = new TagLevelTokenizer();
            var characterLevelList = new List<FirstLevelToken>();
            characterLevelList.Add(new FirstLevelToken("t", FirstLevelTokenType.String));
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            characterLevelList.Add(new FirstLevelToken("t", FirstLevelTokenType.String));
            var tagLevelList = tagLevelTokenizer.Tokenize(characterLevelList);
            tagLevelList[1].GetTokenValue().Should().Be("_");
            tagLevelList[1].GetSecondTokenType().Should().Be(SecondLevelTokenType.OpenCloseItalics);
        }
        
        [Test]
        public void Tokenize_OpenBold_StartOfLine()
        {
            var tagLevelTokenizer = new TagLevelTokenizer();
            var characterLevelList = new List<FirstLevelToken>();
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            characterLevelList.Add(new FirstLevelToken("t", FirstLevelTokenType.String));
            var tagLevelList = tagLevelTokenizer.Tokenize(characterLevelList);
            tagLevelList[0].GetTokenValue().Should().Be("__");
            tagLevelList[0].GetSecondTokenType().Should().Be(SecondLevelTokenType.OpeningBold);
            tagLevelList[1].GetTokenValue().Should().Be("t");
            tagLevelList[1].GetSecondTokenType().Should().Be(SecondLevelTokenType.String);
        }
        
        [Test]
        public void Tokenize_OpenBold_MiddleOfLine()
        {
            var tagLevelTokenizer = new TagLevelTokenizer();
            var characterLevelList = new List<FirstLevelToken>();
            characterLevelList.Add(new FirstLevelToken(" ", FirstLevelTokenType.Space));
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            characterLevelList.Add(new FirstLevelToken("t", FirstLevelTokenType.String));
            var tagLevelList = tagLevelTokenizer.Tokenize(characterLevelList);
            tagLevelList[1].GetTokenValue().Should().Be("__");
            tagLevelList[1].GetSecondTokenType().Should().Be(SecondLevelTokenType.OpeningBold);
            tagLevelList[2].GetTokenValue().Should().Be("t");
            tagLevelList[2].GetSecondTokenType().Should().Be(SecondLevelTokenType.String);
        }
        
        [Test]
        public void Tokenize_OpenCloseBold_MiddleOfLine()
        {
            var tagLevelTokenizer = new TagLevelTokenizer();
            var characterLevelList = new List<FirstLevelToken>();
            characterLevelList.Add(new FirstLevelToken("t", FirstLevelTokenType.String));
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            characterLevelList.Add(new FirstLevelToken("t", FirstLevelTokenType.String));
            var tagLevelList = tagLevelTokenizer.Tokenize(characterLevelList);
            tagLevelList[1].GetTokenValue().Should().Be("__");
            tagLevelList[1].GetSecondTokenType().Should().Be(SecondLevelTokenType.OpenCloseBold);
            tagLevelList[2].GetTokenValue().Should().Be("t");
            tagLevelList[2].GetSecondTokenType().Should().Be(SecondLevelTokenType.String);
        }
        
        [Test]
        public void Tokenize_CloseBold_EndOfLine()
        {
            var tagLevelTokenizer = new TagLevelTokenizer();
            var characterLevelList = new List<FirstLevelToken>();
            characterLevelList.Add(new FirstLevelToken("t", FirstLevelTokenType.String));
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            var tagLevelList = tagLevelTokenizer.Tokenize(characterLevelList);
            tagLevelList[0].GetTokenValue().Should().Be("t");
            tagLevelList[0].GetSecondTokenType().Should().Be(SecondLevelTokenType.String);
            tagLevelList[1].GetTokenValue().Should().Be("__");
            tagLevelList[1].GetSecondTokenType().Should().Be(SecondLevelTokenType.ClosingBold);
            tagLevelList.Count.Should().Be(2);
        }
        
        [Test]
        public void Tokenize_CloseBold_MiddleOfLine()
        {
            var tagLevelTokenizer = new TagLevelTokenizer();
            var characterLevelList = new List<FirstLevelToken>();
            characterLevelList.Add(new FirstLevelToken("t", FirstLevelTokenType.String));
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            characterLevelList.Add(new FirstLevelToken(" ", FirstLevelTokenType.Space));
            var tagLevelList = tagLevelTokenizer.Tokenize(characterLevelList);
            tagLevelList[0].GetTokenValue().Should().Be("t");
            tagLevelList[0].GetSecondTokenType().Should().Be(SecondLevelTokenType.String);
            tagLevelList[1].GetTokenValue().Should().Be("__");
            tagLevelList[1].GetSecondTokenType().Should().Be(SecondLevelTokenType.ClosingBold);
        }
        
        [Test]
        public void Tokenize_NotCloseBold()
        {
            var tagLevelTokenizer = new TagLevelTokenizer();
            var characterLevelList = new List<FirstLevelToken>();
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            characterLevelList.Add(new FirstLevelToken("_", FirstLevelTokenType.Underscore));
            var tagLevelList = tagLevelTokenizer.Tokenize(characterLevelList);
            tagLevelList[0].GetTokenValue().Should().Be("_");
            tagLevelList[0].GetSecondTokenType().Should().Be(SecondLevelTokenType.String);
            tagLevelList[1].GetTokenValue().Should().Be("_");
            tagLevelList[1].GetSecondTokenType().Should().Be(SecondLevelTokenType.String);
        }
    }
}