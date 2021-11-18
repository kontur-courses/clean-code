using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace MarkdownProcessor
{
    [TestFixture]
    public class TokenizerTests
    {
        private Tokenizer tokenizer = new Tokenizer();

        [Test]
        public void Tokenizer_ShouldReturnTextToken_WhenNoTags()
        {
            var tokens = tokenizer.GetTokens("abc").ToList();
            tokens.Count.Should().Be(1);
            tokens[0].Type.Should().Be(TokenType.Text);
            tokens[0].Value.Should().Be("abc");
        }
        

        [TestCase("_abc_", 3)]
        [TestCase("_asd_ das", 4)]
        [TestCase("_das_ _daf_ _sad", 10)]
        [TestCase("_sad", 2)]
        public void Tokenizer_ShouldReturnTokens_WhenTags(string text, int count)
        {
            var tokens = tokenizer.GetTokens(text).ToList();
            tokens.Count.Should().Be(count);
        }

        [Test]
        public void Tokenizer_ShouldReturnCorrectTokens_WhenItalicTag()
        {
            var tokens = tokenizer.GetTokens("_asd_ _fsd").ToList();
            
            tokens.Count.Should().Be(6);
            
            tokens[0].Type.Should().Be(TokenType.ItalicTag);
            tokens[0].Value.Should().Be("_");
            
            tokens[1].Type.Should().Be(TokenType.Text);
            tokens[1].Value.Should().Be("asd");
            
            tokens[2].Type.Should().Be(TokenType.ItalicTag);
            tokens[2].Value.Should().Be("_");
            
            tokens[3].Type.Should().Be(TokenType.Text);
            tokens[3].Value.Should().Be(" ");
            
            tokens[4].Type.Should().Be(TokenType.ItalicTag);
            tokens[4].Value.Should().Be("_");
            
            tokens[5].Type.Should().Be(TokenType.Text);
            tokens[5].Value.Should().Be("fsd");
        }
    }
}