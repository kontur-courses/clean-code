using System.Linq;
using ApprovalUtilities.Utilities;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace Markdown_Test
{
    [TestFixture]
    public class TokenReader_Test
    {
        private TokenReader tokenReader = new TokenReader(); 
        
        [Test]
        public void GetToken_ShouldReturnTokenWithValueInput_IfStringWithoutControlSymbols()
        {
            var input = "Just a string";

            var (actualToken, end) = tokenReader.GetToken(input, 0);
            var excepted = new Token();
            input.ForEach(excepted.AddChar);
            excepted.CloseToken();
            
            actualToken.Should().BeEquivalentTo(excepted);
            end.Should().Be(input.Length);
        }

        [Test]
        public void GetToken_ShouldReturnTokenFromBeginToControlSymbol_IfStringWithControlSymbol()
        {
            var input = "Just _a string";

            var (actualToken, end) = tokenReader.GetToken(input, 0);
            var excepted = new Token();
            "Just ".ForEach(excepted.AddChar);
            excepted.CloseToken();
            
            actualToken.Should().BeEquivalentTo(excepted);
            end.Should().Be(6);
        }
        
        [Test]
        public void GetToken_ShouldReturnUnClosedToken_IfStringWithControlSymbolButWithoutEnd()
        {
            var input = "_Just a string";

            var (actualToken, end) = tokenReader.GetToken(input, 0);
            var excepted = new Token("_");
            excepted.CreateInnerToken();
            input.Skip(1).ForEach(excepted.AddChar);
            
            actualToken.Should().BeEquivalentTo(excepted);
            end.Should().Be(input.Length);
        }
        
        [Test]
        public void GetToken_ShouldReturnClosedToken_IfStringWithControlSymbols()
        {
            var input = "_Just a string_";

            var (actualToken, end) = tokenReader.GetToken(input, 0);
            var excepted = new Token("_");
            excepted.CreateInnerToken();
            "Just a string".ForEach(excepted.AddChar);
            excepted.CloseToken();
            
            actualToken.Should().BeEquivalentTo(excepted);
            end.Should().Be(input.Length);
        }
        
        [Test]
        public void GetToken_ShouldReturnClosedTokenWithInnerToken_IfStringWithControlSymbols()
        {
            var input = "__Just _a_ string__";

            var (actualToken, end) = tokenReader.GetToken(input, 0);
            var excepted = new Token("__");
            excepted.CreateInnerToken();
            "Just ".ForEach(excepted.AddChar);
            var innerToken = excepted.CreateInnerToken("_");
            innerToken.CreateInnerToken();
            excepted.AddChar('a');
            innerToken.CloseToken();
            " string".ForEach(excepted.AddChar);
            excepted.CloseToken();
            
            actualToken.Should().BeEquivalentTo(excepted);
            end.Should().Be(input.Length);
        }
        
        [Test]
        public void GetToken_ShouldReturnClosedTokenWithInnerTokenThatNotHaveTag_IfStringWithControlSymbolsThatCloseAnotherOne()
        {
            var input = "_Just __a__ string_";

            var (actualToken, end) = tokenReader.GetToken(input, 0);
            var excepted = new Token("_");
            excepted.CreateInnerToken();
            "Just ".ForEach(excepted.AddChar);
            var innerToken = excepted.CreateInnerToken("__");
            innerToken.CreateInnerToken();
            excepted.AddChar('a');
            innerToken.CloseToken();
            " string".ForEach(excepted.AddChar);
            excepted.CloseToken();
            excepted.ClearTags(ControlSymbols.TagCloseNextTag["_"], "_");
            
            actualToken.Should().BeEquivalentTo(excepted);
            end.Should().Be(input.Length);
        }
    }
}