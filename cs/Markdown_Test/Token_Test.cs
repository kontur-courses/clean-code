using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace Markdown_Test
{
    [TestFixture]
    public class Token_Test
    {
        [Test]
        public void AddChar_ShouldAddCharInValue_IfTokenNotHaveInnerTokens()
        {
            var token = new Token();
            
            token.AddChar('1');

            token.GetValue.Should().Be("1");
        }
        
        [Test]
        public void AddChar_ShouldAddCharInInnerTokenValue_IfTokenHaveInnerTokens()
        {
            var token = new Token();
            var innerToken = token.CreateInnerToken();
            
            token.AddChar('1');

            innerToken.GetValue.Should().Be("1");
        }
        
        [Test]
        public void AddChar_ShouldAddCharInLastInnerTokenValue_IfTokenHaveInnerTokens()
        {
            var token = new Token();
            var firstInnerToken = token.CreateInnerToken();
            var innerToken = token.CreateInnerToken();
            
            token.AddChar('1');

            firstInnerToken.GetValue.Should().Be("");
            innerToken.GetValue.Should().Be("1");
        }
        
        [Test]
        public void AddChar_ShouldAddCharInLastInnerTokenValue_IfTokenHaveInnerTokensWithInnerTokens()
        {
            var token = new Token();
            var firstInnerToken = token.CreateInnerToken();
            var innerToken = firstInnerToken.CreateInnerToken();
            
            token.AddChar('1');

            firstInnerToken.GetValue.Should().Be("");
            innerToken.GetValue.Should().Be("1");
        }
    }
}