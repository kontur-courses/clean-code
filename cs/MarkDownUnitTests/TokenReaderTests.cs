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
        public void Read_ReturnsValidTokensList_WhenTextIsWithHeaderMarkdownTag()
        {
            var inputText = "# Simple header\n";

            var reader = new TokenReader(new MdTagStorage());

            var tokens = reader.Read(inputText);

            tokens.Should().BeEquivalentTo(new[]
                {
                    new TagToken(TokenType.Tag, 0, 1, SubTagOrder.Opening),
                    new Token(TokenType.Text, 1, 14),
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

            var chars = inputText.ToCharArray();

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
    }
}
