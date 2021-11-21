using FluentAssertions;
using Markdown;
using Markdown.Interfaces;
using Markdown.Model;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MarkdownUnitTests
{
    public class TokenizerTest
    {
        private readonly Tokenizer tokenizer = new Tokenizer();

        [Test]
        public void Parse_ShouldBeCountTagIsZero_WhenStringIsEmpty()
        {
            tokenizer.Parse("").Count().Should().Be(0);
        }

        [Test]
        public void Parse_ShouldBeCountTagIsZero_WhenStringIsNull()
        {
            tokenizer.Parse(null).Count().Should().Be(0);
        }

        private static List<IToken> GetToken = new List<IToken>
        {
            new BoldToken(),
            new ItalicToken(),
            new HeaderToken(),
            new TextToken(),
        };

        [TestCase("__", 0, TestName = "BoldTag")]
        [TestCase("_", 1, TestName = "ItalicTag")]
        [TestCase("# ", 2, TestName = "HeaderTag")]
        [TestCase("tesst", 3, TestName = "TextTag")]
        public void Parse_ShouldBeFindTag_WhenInput(string tag, int item)
        {
            tokenizer.Parse(tag).First().GetType().Should().Be(GetToken[item].GetType());
        }

        [Test]
        public void Parse_ShouldBeParsingCorrectly()
        {
            var expectedTokens = new IToken[]
            {
                new BoldToken() {IsOpenTag = true},
                new TextToken() {IsOpenTag = false},
                new BoldToken() {IsOpenTag = false},
            };

            var actualTokens = tokenizer.Parse("__test__").ToList();

            SequenceEqual(expectedTokens.ToList(), actualTokens).Should().BeTrue();
        }

        [Test]
        public void Parse_ShouldBeFindCloseAndOpenItalicTag()
        {
            var expectedTokens = new IToken[]
            {
                new ItalicToken() {IsOpenTag = true},
                new TextToken() {IsOpenTag = false,Value = "test"},
                new ItalicToken() {IsOpenTag = false}
            };

            var actualTokens = tokenizer.Parse("_test_").ToList();

            SequenceEqual(expectedTokens.ToList(), actualTokens).Should().BeTrue();
        }

        [Test]
        public void Parse_ShouldBeFindCloseAndOpenBoldTag()
        {
            var expectedTokens = new IToken[]
            {
                new BoldToken() {IsOpenTag = true},
                new TextToken() {IsOpenTag = false,Value = "test"},
                new BoldToken() {IsOpenTag = false},
            };

            var actualTokens = tokenizer.Parse("__test__").ToList();

            SequenceEqual(expectedTokens.ToList(), actualTokens).Should().BeTrue();
        }
        [Test]
        public void Parse_ShouldBeFindCloseAndOpenHeaderTag()
        {
            var expectedTokens = new IToken[]
            {
                new HeaderToken() {IsOpenTag = true},
                new TextToken() {IsOpenTag = false,Value = "test"},
                new HeaderToken() {IsOpenTag = false}
            };
            var actualTokens = tokenizer.Parse("# test").ToList();

            SequenceEqual(expectedTokens.ToList(), actualTokens).Should().BeTrue();
        }

        [Test]
        public void Parse_ParsingNestedTag_ShouldBeCorrect()
        {
            var expectedTokens = new IToken[]
            {
                new TextToken() {IsOpenTag = false,Value = "Заголовок"},
                new TextToken() {IsOpenTag = false,Value = " "},
                new BoldToken() {IsOpenTag = true},
                new TextToken() {IsOpenTag = false,Value = "с"},
                new TextToken() {IsOpenTag = false,Value = " "},
                new ItalicToken(){IsOpenTag = true},
                new TextToken() {IsOpenTag = false, Value = "разными"},
                new ItalicToken(){IsOpenTag = false},
                new TextToken() {IsOpenTag = false,Value = " "},
                new TextToken() {IsOpenTag = false, Value = "символами"},
                new BoldToken() {IsOpenTag = false}
            };

            var actualTokens = tokenizer.Parse("Заголовок __с _разными_ символами__").ToList();

            SequenceEqual(expectedTokens.ToList(), actualTokens).Should().BeTrue();
        }

        [Test]
        public void Parse_ShouldBeIgnoreHeaderTag()
        {
            var expectedTokens = new IToken[]
            {
                new ItalicToken() {IsOpenTag = true},
                new TextToken() {IsOpenTag = false,Value = "test"},
                new ItalicToken() {IsOpenTag = false},
                new TextToken() {IsOpenTag = false,Value = "#"},
                new TextToken() {IsOpenTag = false,Value = " "}
            };

            var actualTokens = tokenizer.Parse("_test_# ").ToList();

            SequenceEqual(expectedTokens.ToList(), actualTokens).Should().BeTrue();
        }

        private bool SequenceEqual(List<IToken> expectedTokens, List<IToken> actualTokens)
        {
            return !actualTokens.Where((t, i) => expectedTokens[i].GetType() != t.GetType()).Any();
        }
    }
}
