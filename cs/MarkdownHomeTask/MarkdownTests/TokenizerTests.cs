using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class TokenizerTests
    {
        [Test]
        public void Tokenize_withSimpleTag_shouldReturnExpectedTokens()
        {
            var text = "#abc";
            var tokens = new Tokenizer().Tokenize(text, new[] {"#"});
            var expectedTokens = new[]
            {
                new Token(0, "#", TokenType.Tag),
                new Token(1,"abc",TokenType.Text),
            };

            tokens.Should().Equal(expectedTokens);
        }
        
        [Test]
        public void Tokenize_withCompoundTag_shouldReturnExpectedTokens()
        {
            var text = "__abc";
            var tokens = new Tokenizer().Tokenize(text, new[] {"_","__"});
            var expectedTokens = new[]
            {
                new Token(0, "__", TokenType.Tag),
                new Token(2,"abc",TokenType.Text),
            };

            tokens.Should().Equal(expectedTokens);
        }
        
        [Test]
        public void Tokenize_whenTextHasPrefixOfTag_shouldReturnExpectedTokens()
        {
            var text = "__abc43";
            var tokens = new Tokenizer().Tokenize(text, new[] {"___"});
            var expectedTokens = new[]
            {
                new Token(0, "__abc", TokenType.Text),
                new Token(5, "43", TokenType.Number)
            };

            tokens.Should().Equal(expectedTokens);
        }
        
        [Test]
        public void Tokenize_withCompoundAndSimpleTags_shouldReturnExpectedTokens()
        {
            var text = "__abc#qwer";
            var tokens = new Tokenizer().Tokenize(text, new[] {"_","__","#"});
            var expectedTokens = new[]
            {
                new Token(0, "__", TokenType.Tag),
                new Token(2,"abc",TokenType.Text),
                new Token(5,"#",TokenType.Tag),
                new Token(6,"qwer",TokenType.Text)
            };

            tokens.Should().Equal(expectedTokens);
        }
        
        [Test]
        public void Tokenize_withEscape_shouldReturnExpectedTokens()
        {
            var text = @"\__abc";
            var tokens = new Tokenizer().Tokenize(text, new[] {"_","__","#"});
            var expectedTokens = new[]
            {
                new Token(1, "_", TokenType.Text),
                new Token(2, "_", TokenType.Tag),
                new Token(3,"abc",TokenType.Text)
            };

            tokens.Should().Equal(expectedTokens);
        }
        
        [Test]
        public void Tokenize_withDoubleEscape_shouldReturnExpectedTokens()
        {
            var text = @"_abc\\qwer";
            var tokens = new Tokenizer().Tokenize(text, new[] {"_","__","#"});
            var expectedTokens = new[]
            {
                new Token(0, "_", TokenType.Tag),
                new Token(1, @"abc\qwer", TokenType.Text)
            };

            tokens.Should().Equal(expectedTokens);
        }
        
        [Test]
        public void Tokenize_withWhiteSpace_shouldReturnExpectedTokens()
        {
            var text = @"_ abc\\ qwer";
            var tokens = new Tokenizer().Tokenize(text, new[] {"_","__","#"});
            var expectedTokens = new[]
            {
                new Token(0, "_", TokenType.Tag),
                new Token(1, "", TokenType.Text),
                new Token(2, @"abc\", TokenType.Text),
                new Token(8,"qwer",TokenType.Text)
            };

            tokens.Should().Equal(expectedTokens);
        }
        
        [Test]
        public void Tokenize_withWhiteSpace_shouldReturnExpectedTokens2()
        {
            var text = @"a a a a";
            var tokens = new Tokenizer().Tokenize(text, new[] {"_","__","#"});
            var expectedTokens = new[]
            {
                new Token(0, "a", TokenType.Text),
                new Token(2, "a", TokenType.Text),
                new Token(4, "a", TokenType.Text),
                new Token(6,"a",TokenType.Text)
            };

            tokens.Should().Equal(expectedTokens);
        }
        
        [Test]
        public void Tokenize_withAllCases_shouldReturnExpectedTokens()
        {
            var text = @"_ __ab1c\\ q234wer";
            var tokens = new Tokenizer().Tokenize(text, new[] {"_","__","#"});
            var expectedTokens = new[]
            {
                new Token(0, "_", TokenType.Tag),
                new Token(1, "", TokenType.Text),
                new Token(2,"__",TokenType.Tag),
                new Token(4, @"ab", TokenType.Text),
                new Token(6,"1",TokenType.Number),
                new Token(7,@"c\",TokenType.Text),
                new Token(11,"q",TokenType.Text),
                new Token(12,"234",TokenType.Number),
                new Token(15,"wer",TokenType.Text)
            };

            tokens.Should().Equal(expectedTokens);
        }
        
    }
}