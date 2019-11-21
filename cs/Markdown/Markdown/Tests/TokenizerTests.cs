using NUnit.Framework;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

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
        [TestCase("\\*sad*", ExpectedResult = 5)]
        [TestCase("a \\*a\\* a", ExpectedResult = 7)]
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
            var expected = new []
            {
                TokenType.Text,
                TokenType.Text,
                TokenType.WhiteSpace,
                TokenType.Text,
                TokenType.Text,
                TokenType.WhiteSpace,
                TokenType.Text
            };
            tokens.Select(token => token.Type)
                .Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_ShouldReturnCorrectValues()
        {
            var text = "*a* _a_";
            var tokens = tokenizer.Tokenize(text);
            var expected = new [] { "*", "a", "*", " ", "_", "a", "_" };
            tokens.Select(token => token.Value)
                .Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_ShouldReturnCorrectTypes()
        {
            var str = "*a _a_ a*";
            var tokens = tokenizer.Tokenize(str);
            var expected = new []
            {
                TokenType.MdElement,
                TokenType.Text,
                TokenType.WhiteSpace,
                TokenType.MdElement,
                TokenType.Text,
                TokenType.MdElement,
                TokenType.WhiteSpace,
                TokenType.Text,
                TokenType.MdElement
            };
            tokens.Select(token => token.Type)
                .Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_ShouldReturnCorrectMdTypes()
        {
            var text = "*_a_*";
            var tokens = tokenizer.Tokenize(text);
            var italics = md.elementSigns["_"];
            var strong = md.elementSigns["*"];
            var expected = new [] { strong, italics, null, italics, strong };
            tokens.Select(token => token.MdType)
                .Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_ShouldEncloseMdElementsCorrectly()
        { 
            md.AddElement(new MdElement("#", "<H1>", false));
            var text = "^ *a_*";
            var tokens = tokenizer.Tokenize(text);
            var expected = new [] { true, false, true, false, false, true };
            tokens.Select(token => token.IsClosed)
                .Should().BeEquivalentTo(expected);
        }
    }
}
