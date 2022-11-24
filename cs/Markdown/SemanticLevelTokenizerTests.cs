using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
    public class SemanticLevelTokenizerTests
    {
        [Test]
        public void Tokenize_ItalicsInStartOfWord_StartLine()
        {
            
            var semanticLevelTokenizer = new SemanticLevelTokenizer();
            var tagLevelList = new List<SecondLevelToken>();
            tagLevelList.Add(new SecondLevelToken("_", SecondLevelTokenType.OpeningItalics));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("_", SecondLevelTokenType.OpenCloseItalics));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("_", SecondLevelTokenType.String));
            var semanticLevelList = semanticLevelTokenizer.Tokenize(tagLevelList);
            semanticLevelList[0].GetTokenValue().Should().Be("_");
            semanticLevelList[0].GetSecondTokenType().Should().Be(SecondLevelTokenType.OpeningItalics);
            semanticLevelList[2].GetTokenValue().Should().Be("_");
            semanticLevelList[2].GetSecondTokenType().Should().Be(SecondLevelTokenType.ClosingItalics);
            semanticLevelList[4].GetTokenValue().Should().Be("_");
            semanticLevelList[4].GetSecondTokenType().Should().Be(SecondLevelTokenType.String);
        }
        
        [Test]
        public void Tokenize_ItalicsInMiddleOfWord_StartLine()
        {
            
            var semanticLevelTokenizer = new SemanticLevelTokenizer();
            var tagLevelList = new List<SecondLevelToken>();
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("_", SecondLevelTokenType.OpenCloseItalics));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("_", SecondLevelTokenType.OpenCloseItalics));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            var semanticLevelList = semanticLevelTokenizer.Tokenize(tagLevelList);
            semanticLevelList[1].GetTokenValue().Should().Be("_");
            semanticLevelList[1].GetSecondTokenType().Should().Be(SecondLevelTokenType.OpeningItalics);
            semanticLevelList[3].GetTokenValue().Should().Be("_");
            semanticLevelList[3].GetSecondTokenType().Should().Be(SecondLevelTokenType.ClosingItalics);
        }
        
        [Test]
        public void Tokenize_ItalicsInTheEnd_StartLine()
        {
            
            var semanticLevelTokenizer = new SemanticLevelTokenizer();
            var tagLevelList = new List<SecondLevelToken>();
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("_", SecondLevelTokenType.OpeningItalics));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("_", SecondLevelTokenType.ClosingItalics));
            var semanticLevelList = semanticLevelTokenizer.Tokenize(tagLevelList);
            semanticLevelList[1].GetTokenValue().Should().Be("_");
            semanticLevelList[1].GetSecondTokenType().Should().Be(SecondLevelTokenType.OpeningItalics);
            semanticLevelList[3].GetTokenValue().Should().Be("_");
            semanticLevelList[3].GetSecondTokenType().Should().Be(SecondLevelTokenType.ClosingItalics);
        }
        
        [Test]
        public void Tokenize_BoldInStartOfWord_StartLine()
        {
            
            var semanticLevelTokenizer = new SemanticLevelTokenizer();
            var tagLevelList = new List<SecondLevelToken>();
            tagLevelList.Add(new SecondLevelToken("__", SecondLevelTokenType.OpeningBold));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("__", SecondLevelTokenType.OpenCloseBold));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("__", SecondLevelTokenType.String));
            var semanticLevelList = semanticLevelTokenizer.Tokenize(tagLevelList);
            semanticLevelList[0].GetTokenValue().Should().Be("__");
            semanticLevelList[0].GetSecondTokenType().Should().Be(SecondLevelTokenType.OpeningBold);
            semanticLevelList[2].GetTokenValue().Should().Be("__");
            semanticLevelList[2].GetSecondTokenType().Should().Be(SecondLevelTokenType.ClosingBold);
            semanticLevelList[4].GetTokenValue().Should().Be("__");
            semanticLevelList[4].GetSecondTokenType().Should().Be(SecondLevelTokenType.String);
        }
        
        [Test]
        public void Tokenize_BoldInMiddleOfWord_StartLine()
        {
            
            var semanticLevelTokenizer = new SemanticLevelTokenizer();
            var tagLevelList = new List<SecondLevelToken>();
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("__", SecondLevelTokenType.OpenCloseBold));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("__", SecondLevelTokenType.OpenCloseBold));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            var semanticLevelList = semanticLevelTokenizer.Tokenize(tagLevelList);
            semanticLevelList[1].GetTokenValue().Should().Be("__");
            semanticLevelList[1].GetSecondTokenType().Should().Be(SecondLevelTokenType.OpeningBold);
            semanticLevelList[3].GetTokenValue().Should().Be("__");
            semanticLevelList[3].GetSecondTokenType().Should().Be(SecondLevelTokenType.ClosingBold);
        }
        
        [Test]
        public void Tokenize_BoldInTheEnd_StartLine()
        {
            
            var semanticLevelTokenizer = new SemanticLevelTokenizer();
            var tagLevelList = new List<SecondLevelToken>();
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("__", SecondLevelTokenType.OpeningBold));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("__", SecondLevelTokenType.ClosingBold));
            var semanticLevelList = semanticLevelTokenizer.Tokenize(tagLevelList);
            semanticLevelList[1].GetTokenValue().Should().Be("__");
            semanticLevelList[1].GetSecondTokenType().Should().Be(SecondLevelTokenType.OpeningBold);
            semanticLevelList[3].GetTokenValue().Should().Be("__");
            semanticLevelList[3].GetSecondTokenType().Should().Be(SecondLevelTokenType.ClosingBold);
        }
        
        [Test]
        public void Tokenize_BoldInDifferentWords()
        {
            
            var semanticLevelTokenizer = new SemanticLevelTokenizer();
            var tagLevelList = new List<SecondLevelToken>();
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("__", SecondLevelTokenType.OpenCloseBold));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken(" ", SecondLevelTokenType.Space));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("__", SecondLevelTokenType.OpenCloseBold));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            var semanticLevelList = semanticLevelTokenizer.Tokenize(tagLevelList);
            semanticLevelList[1].GetTokenValue().Should().Be("__");
            semanticLevelList[1].GetSecondTokenType().Should().Be(SecondLevelTokenType.String);
            semanticLevelList[5].GetTokenValue().Should().Be("__");
            semanticLevelList[5].GetSecondTokenType().Should().Be(SecondLevelTokenType.String);
        }
        
        [Test]
        public void Tokenize_IntersectionBoldAndItalics()
        {
            
            var semanticLevelTokenizer = new SemanticLevelTokenizer();
            var tagLevelList = new List<SecondLevelToken>();
            tagLevelList.Add(new SecondLevelToken("__", SecondLevelTokenType.OpeningBold));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("_", SecondLevelTokenType.OpenCloseItalics));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("__", SecondLevelTokenType.OpenCloseBold));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("_", SecondLevelTokenType.ClosingItalics));
            var semanticLevelList = semanticLevelTokenizer.Tokenize(tagLevelList);
            semanticLevelList[0].GetTokenValue().Should().Be("__");
            semanticLevelList[0].GetSecondTokenType().Should().Be(SecondLevelTokenType.String);
            semanticLevelList[2].GetTokenValue().Should().Be("_");
            semanticLevelList[2].GetSecondTokenType().Should().Be(SecondLevelTokenType.String);
            semanticLevelList[4].GetTokenValue().Should().Be("__");
            semanticLevelList[4].GetSecondTokenType().Should().Be(SecondLevelTokenType.String);
            semanticLevelList[6].GetTokenValue().Should().Be("_");
            semanticLevelList[6].GetSecondTokenType().Should().Be(SecondLevelTokenType.String);
        }
        
        [Test]
        public void Tokenize_ItalicsInBold_Work()
        {
            
            var semanticLevelTokenizer = new SemanticLevelTokenizer();
            var tagLevelList = new List<SecondLevelToken>();
            tagLevelList.Add(new SecondLevelToken("__", SecondLevelTokenType.OpeningBold));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken(" ", SecondLevelTokenType.Space));
            tagLevelList.Add(new SecondLevelToken("_", SecondLevelTokenType.OpeningItalics));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("_", SecondLevelTokenType.ClosingItalics));
            tagLevelList.Add(new SecondLevelToken(" ", SecondLevelTokenType.Space));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("__", SecondLevelTokenType.ClosingBold));
            var semanticLevelList = semanticLevelTokenizer.Tokenize(tagLevelList);
            semanticLevelList[0].GetTokenValue().Should().Be("__");
            semanticLevelList[0].GetSecondTokenType().Should().Be(SecondLevelTokenType.OpeningBold);
            semanticLevelList[3].GetTokenValue().Should().Be("_");
            semanticLevelList[3].GetSecondTokenType().Should().Be(SecondLevelTokenType.OpeningItalics);
            semanticLevelList[5].GetTokenValue().Should().Be("_");
            semanticLevelList[5].GetSecondTokenType().Should().Be(SecondLevelTokenType.ClosingItalics);
            semanticLevelList[8].GetTokenValue().Should().Be("__");
            semanticLevelList[8].GetSecondTokenType().Should().Be(SecondLevelTokenType.ClosingBold);
        }
        
        [Test]
        public void Tokenize_BoldInItalics_DoesNotWork()
        {
            
            var semanticLevelTokenizer = new SemanticLevelTokenizer();
            var tagLevelList = new List<SecondLevelToken>();
            tagLevelList.Add(new SecondLevelToken("_", SecondLevelTokenType.OpeningItalics));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken(" ", SecondLevelTokenType.Space));
            tagLevelList.Add(new SecondLevelToken("__", SecondLevelTokenType.OpeningBold));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("__", SecondLevelTokenType.ClosingBold));
            tagLevelList.Add(new SecondLevelToken(" ", SecondLevelTokenType.Space));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("_", SecondLevelTokenType.ClosingItalics));
            var semanticLevelList = semanticLevelTokenizer.Tokenize(tagLevelList);
            semanticLevelList[0].GetTokenValue().Should().Be("_");
            semanticLevelList[0].GetSecondTokenType().Should().Be(SecondLevelTokenType.OpeningItalics);
            semanticLevelList[3].GetTokenValue().Should().Be("__");
            semanticLevelList[3].GetSecondTokenType().Should().Be(SecondLevelTokenType.String);
            semanticLevelList[5].GetTokenValue().Should().Be("__");
            semanticLevelList[5].GetSecondTokenType().Should().Be(SecondLevelTokenType.String);
            semanticLevelList[8].GetTokenValue().Should().Be("_");
            semanticLevelList[8].GetSecondTokenType().Should().Be(SecondLevelTokenType.ClosingItalics);
        }
        
        [Test]
        public void Tokenize_SpaceBeforeClosingBold_DoesNotWork()
        {
            
            var semanticLevelTokenizer = new SemanticLevelTokenizer();
            var tagLevelList = new List<SecondLevelToken>();
            tagLevelList.Add(new SecondLevelToken("__", SecondLevelTokenType.OpeningBold));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken(" ", SecondLevelTokenType.Space));
            tagLevelList.Add(new SecondLevelToken("__", SecondLevelTokenType.OpeningBold));
            tagLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            tagLevelList.Add(new SecondLevelToken("__", SecondLevelTokenType.ClosingBold));
            var semanticLevelList = semanticLevelTokenizer.Tokenize(tagLevelList);
            semanticLevelList[0].GetTokenValue().Should().Be("__");
            semanticLevelList[0].GetSecondTokenType().Should().Be(SecondLevelTokenType.String);
            semanticLevelList[3].GetTokenValue().Should().Be("__");
            semanticLevelList[3].GetSecondTokenType().Should().Be(SecondLevelTokenType.OpeningBold);
            semanticLevelList[5].GetTokenValue().Should().Be("__");
            semanticLevelList[5].GetSecondTokenType().Should().Be(SecondLevelTokenType.ClosingBold);
        }
    }
}