using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Markdown;
using Markdown.Tags;
using Markdown.Tokens;
using NUnit.Framework;
using FluentAssertions;
using Token = Markdown.Tokens.Token;

namespace MarkDownUnitTests
{
    [TestClass]
    public class TokenReaderTests
    {
        [TestMethod]
        public void Read_ReturnsInputText_WhenThereAreNoTags()
        {
            var inputText = "Text without tags";

            var reader = new TokenReader(new MdTagStorage());

            var tokens = reader.Read(inputText);

            tokens.Should().BeEquivalentTo(new[]
                {
                    new Token(TokenType.Text, 0, inputText.Length),
                }
            );
        }

        [TestMethod]
        public void Read_ReturnsValidTokensList_WhenTextIsWithHeaderMarkdownTag()
        {
            var inputText = "# Simple header\n";

            var reader = new TokenReader(new MdTagStorage());

            var tokens = reader.Read(inputText);

            tokens.Should().BeEquivalentTo(new[]
                {
                    new TagToken(TokenType.Tag, 0, 2, SubTagOrder.Opening),
                    new Token(TokenType.Text, 2, 13),
                    new TagToken(TokenType.Tag, 15, 1, SubTagOrder.Closing),
                }
            );
        }

        [TestMethod]
        public void Read_ReturnsValidTokensList_WhenTextIsWithItalicMarkdownTag()
        {
            var inputText = "Some _italic_ text";

            var reader = new TokenReader(new MdTagStorage());

            var tokens = reader.Read(inputText);

            tokens.Should().BeEquivalentTo(new[]
                {
                    new Token(TokenType.Text, 0, 5),
                    new TagToken(TokenType.Tag, 5, 1, SubTagOrder.Opening),
                    new Token(TokenType.Text, 6, 6),
                    new TagToken(TokenType.Tag, 12, 1, SubTagOrder.Closing),
                    new Token(TokenType.Text, 13, 5)
                }
            );
        }

        [TestMethod]
        public void Read_ReturnsValidTokensList_WhenTextIsWithBoldMarkdownTag()
        {
            var inputText = "Some __bold__ text";

            var reader = new TokenReader(new MdTagStorage());

            var tokens = reader.Read(inputText);

            tokens.Should().BeEquivalentTo(new[]
                {
                    new Token(TokenType.Text, 0, 5),
                    new TagToken(TokenType.Tag, 5, 2, SubTagOrder.Opening),
                    new Token(TokenType.Text, 7, 4),
                    new TagToken(TokenType.Tag, 11, 2, SubTagOrder.Closing),
                    new Token(TokenType.Text, 13, 5)
                }
            );
        }

        [TestMethod]
        public void Read_ReturnsValidTokensList_WhenTextIsWithItalicAndBoldMarkdownTag()
        {
            var inputText = "Some __bold__ and _italic_ text";

            var reader = new TokenReader(new MdTagStorage());

            var tokens = reader.Read(inputText);

            tokens.Should().BeEquivalentTo(new[]
                {
                    new Token(TokenType.Text, 0, 5),
                    new TagToken(TokenType.Tag, 5, 2, SubTagOrder.Opening),
                    new Token(TokenType.Text, 7, 4),
                    new TagToken(TokenType.Tag, 11, 2, SubTagOrder.Closing),
                    new Token(TokenType.Text, 13, 5),
                    new TagToken(TokenType.Tag, 18, 1, SubTagOrder.Opening),
                    new Token(TokenType.Text, 19, 6),
                    new TagToken(TokenType.Tag, 25, 1, SubTagOrder.Closing),
                    new Token(TokenType.Text, 26, 5)
                }
            );
        }

        [TestCase("One _unpaired opening italic tag", TestName = "Opening italic tag")]
        [TestCase("One unpaired_ closing italic tag", TestName = "Closing italic tag")]
        [TestCase("One __unpaired opening bold tag", TestName = "Opening bold tag")]
        [TestCase("One unpaired__ closing bold tag", TestName = "Closing bold tag")]
        [TestCase("# One unpaired opening header tag", TestName = "Opening header tag")]
        [TestCase("One unpaired closing header tag\n", TestName = "Closing header tag")]
        public void Read_ReturnsInputText_WhenThereIsOneUnpairedTag(string inputText)
        {
            var reader = new TokenReader(new MdTagStorage());

            var tokens = reader.Read(inputText);

            tokens.Should().BeEquivalentTo(new[]
                {
                    new Token(TokenType.Text, 0, inputText.Length),
                }
            );
        }

    }
}
