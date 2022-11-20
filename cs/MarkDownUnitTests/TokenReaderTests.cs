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
        public void Read_ReturnsValidTokensList_WhenTextIsWithItalicMarkdownTag()
        {
            var inputText = "Some _italic_ text";

            var len = inputText.Length;

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


    }
}
