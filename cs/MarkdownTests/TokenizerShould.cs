using System.Collections.Generic;
using Markdown;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestClass]
    public class TokenizerShould
    {
        [TestMethod]
        public void ReturnEmptyListOnEmptyString()
        {
            var markdownSource = "";

            Tokenizer.Tokenize(markdownSource).Should().BeEquivalentTo(new List<Token>());
        }

        [TestMethod]
        public void ReturnEmptyListOnNullString()
        {
            string markdownSource = null;

            Tokenizer.Tokenize(markdownSource).Should().BeEquivalentTo(new List<Token>());
        }

        [Test]
        public void ReturnOneTokenOnRawString()
        {
            var markdownSource = "Hello, world!";

            Tokenizer.Tokenize(markdownSource).Should().BeEquivalentTo(new[]
            {
                new Token(0, 13, Tag.Raw)
            });
        }

        [TestCase("hello _people", Description = "Single updaired underscore")]
        [TestCase("hello __people", Description = "Single unpaired double-underscore")]
        [TestCase("hel_lo__people", Description = "Mix of unpaired underscores")]
        public void ReturnOnlyRawTokensOnUnpairedUnderscores(string markdownSource)
        {
            Tokenizer.Tokenize(markdownSource).Should().OnlyContain(token => token.Tag == Tag.Raw);
        }

        [TestCase("hello_ people_!", Description = "Opening underscore before space")]
        [TestCase("hello_\tpeople_!", Description = "Opening underscore before whitespace")]
        [TestCase("hello _people _!", Description = "Closing underscore after space")]
        [TestCase("hello _people\t_!", Description = "Closing underscore after whitespace")]
        public void ReturnOnlyRawTokensOnUnderscoresBeforeWhitespaces(string markdownSource)
        {
            Tokenizer.Tokenize(markdownSource).Should().OnlyContain(token => token.Tag == Tag.Raw);
        }
    }
}
