using NUnit.Framework;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    [TestFixture]
    class TokenizerTests
    {
        private Tokenizer tokenizer;
        private Md md;

        [SetUp]
        public void BaseSetup()
        {
            md = new Md();
            tokenizer = new Tokenizer(md.elementSigns);
        }

        [Test]
        public void Tokenize_ShouldThrowArgumentNullException_OnNullText()
        {
            Action act = () => tokenizer.Tokenize(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestCase("*a*", ExpectedResult = 3)]
        [TestCase("a a a a_ d", ExpectedResult = 10)]
        [TestCase("", ExpectedResult = 0)]
        [TestCase("\\\\", ExpectedResult = 1)]
        public int Tokenizer_ShouldReturnCorrectNumberOfElements(string text)
        {
            var tokens = tokenizer.Tokenize(text);
            return tokens.Count;
        }

        [Test]
        public void Tokenize_ShouldScreenElements()
        {
            var text = "\\*a \\_a \\\\";
            var tokens = tokenizer.Tokenize(text);
            tokens[0].Type.Should().Be(TokenType.Text);
            tokens[3].Type.Should().Be(TokenType.Text);
            tokens[6].Type.Should().Be(TokenType.Text);
        }

        [Test]
        public void Tokenize_ShouldReturnCorrectValues()
        {
            var text = "*a* _a_";
            var tokens = tokenizer.Tokenize(text);
            var expected = new List<char> { '*', 'a', '*', ' ', '_', 'a', '_' };
            tokens.Select(token => token.Value).ToList()
                .Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_ShouldReturnCorrectTypes()
        {
            var str = "*a _a_ a*";
            var tokens = tokenizer.Tokenize(str);
            var mdElem = TokenType.MdElement;
            var text = TokenType.Text;
            var ws = TokenType.WhiteSpace;
            var expected = new List<TokenType> { mdElem, text, ws, mdElem, text, mdElem, ws, text, mdElem };
            tokens.Select(token => token.Type).ToList()
                .Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_ShouldReturnCorrectMdTypes()
        {
            var text = "*_a_*";
            var tokens = tokenizer.Tokenize(text);
            var italics = md.elementSigns['_'];
            var strong = md.elementSigns['*'];
            var expected = new List<MdElement> { strong, italics, null, italics, strong };
            tokens.Select(token => token.MdType).ToList()
                .Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_ShouldEncloseMdElentsCorrectly()
        {
            md.AddElement(new MdElement('^', "<H1>", false));
            var text = "^ *a_*";
            var tokens = tokenizer.Tokenize(text);
            var expected = new List<bool> { true, false, true, false, false, true };
            tokens.Select(token => token.IsClosed).ToList()
                .Should().BeEquivalentTo(expected);
        }
    }
}
