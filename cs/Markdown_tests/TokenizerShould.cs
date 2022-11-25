using FluentAssertions;
using Markdown;
using NUnit.Framework;
using System.Collections.Generic;

namespace Markdown_tests
{
    [TestFixture]
    public class TokenizerShould
    {
        private Tokenizer tokenizer;    

        [SetUp]
        public void SetUp()
        {
            var separators = new Dictionary<string, Mod>()
            {
                [""] = Mod.Common,
                ["#"] = Mod.Title,
                ["__"] = Mod.Bold,
                ["_"] = Mod.Italic,
                ["\\"] = Mod.Slash,
                ["["] = Mod.LinkName,
                ["("] = Mod.LinkUrl
            };

            tokenizer = new Tokenizer(separators);
        }

        [Test]
        public void Tokenize_ItalicMarkdown_ShouldReturnListWithItalic()
        {
            var testString = "text _text_ text";

            var current = tokenizer.TikenizeText(testString);
            var expected = new List<Token>()
            {
                new Token(0, 4, Mod.Common, false),
                new Token(5, 5, Mod.Italic),
                new Token(6, 9, Mod.Common, false),
                new Token(10, 10, Mod.Italic),
                new Token(11, 16, Mod.Common, false)
            };

            current.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_BoldMarkdown_ShouldReturnListWithBold()
        {
            var testString = "text __text__ text";

            var current = tokenizer.TikenizeText(testString);
            var expected = new List<Token>()
            {
                new Token(0, 4, Mod.Common, false),
                new Token(5, 6, Mod.Bold),
                new Token(7, 10, Mod.Common, false),
                new Token(11, 12, Mod.Bold),
                new Token(13, 18, Mod.Common, false)
            };

            current.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_SlashesInText_ShouldReturnOnlyCommonTokens()
        {
            var testString = @"text \_text \__text";

            var current = tokenizer.TikenizeText(testString);
            var expected = new List<Token>()
            {
                new Token(0, 4, Mod.Common, false),
                new Token(6, 6, Mod.Common, false),
                new Token(7, 11, Mod.Common, false),
                new Token(13, 14, Mod.Common, false),
                new Token(15, 19, Mod.Common, false)
            };

            current.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_HashtagInText_ShouldReturnTitleToken()
        {
            var testString = @"# text";

            var current = tokenizer.TikenizeText(testString);
            var expected = new List<Token>()
            {
                new Token(0, 6, Mod.Title),
                new Token(1, 6, Mod.Common, false)
            };

            current.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Tokenize_LinkInText_ShouldReturnLinkTokens()
        {
            var testString = @"text [link name](www.link.com) text";

            var current = tokenizer.TikenizeText(testString);
            var expected = new List<Token>()
            {
                new Token(0, 4, Mod.Common, false),
                new Token(5, 15, Mod.LinkName),
                new Token(16, 29, Mod.LinkUrl),
                new Token(30, 35, Mod.Common, false),
            };

            current.Should().BeEquivalentTo(expected);
        }
    }
}
