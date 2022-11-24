using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Internal;

namespace Markdown
{
    public class TokensToHTMLConverterTest
    {
        [Test]
        public void Tokenize_ItalicsInStartOfWord_StartLine()
        {
            
            var converter = new TokensToHTMLConverter();
            var semanticLevelList = new List<SecondLevelToken>();
            semanticLevelList.Add(new SecondLevelToken("_", SecondLevelTokenType.OpeningItalics));
            semanticLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            semanticLevelList.Add(new SecondLevelToken("_", SecondLevelTokenType.ClosingItalics));
            semanticLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            semanticLevelList.Add(new SecondLevelToken("_", SecondLevelTokenType.String));
            var result = converter.Convert(semanticLevelList);
            var correctResult = "\\<em>t\\</em>t_";
            result.Should().Be(correctResult);
        }
        [Test]
        public void Tokenize_HeaderLine()
        {
            
            var converter = new TokensToHTMLConverter();
            var semanticLevelList = new List<SecondLevelToken>();
            semanticLevelList.Add(new SecondLevelToken("#", SecondLevelTokenType.Header));
            semanticLevelList.Add(new SecondLevelToken(" ", SecondLevelTokenType.Space));
            semanticLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            var result = converter.Convert(semanticLevelList);
            var correctResult = "\\<h1>t\\</h1>";
            result.Should().Be(correctResult);
        }
        
        [Test]
        public void Tokenize_HeaderLineWithLattice()
        {
            
            var converter = new TokensToHTMLConverter();
            var semanticLevelList = new List<SecondLevelToken>();
            semanticLevelList.Add(new SecondLevelToken("#", SecondLevelTokenType.Header));
            semanticLevelList.Add(new SecondLevelToken("_", SecondLevelTokenType.Space));
            semanticLevelList.Add(new SecondLevelToken("t", SecondLevelTokenType.String));
            semanticLevelList.Add(new SecondLevelToken("#", SecondLevelTokenType.String));
            var result = converter.Convert(semanticLevelList);
            var correctResult = "\\<h1>t#\\</h1>";
            result.Should().Be(correctResult);
        }
    }
}