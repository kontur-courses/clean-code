using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Markdown;
using Markdown.Tags;
using Markdown.Tokens;
using NUnit.Framework;
using FluentAssertions;
using Token = Markdown.Tokens.TypedToken;

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
                    new Token(0, inputText.Length, TokenType.Text),
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
                    new TagToken(0, 2, TagType.Header,SubTagOrder.Opening),
                    new Token(2, 13, TokenType.Text),
                    new TagToken(15, 1,TagType.Header, SubTagOrder.Closing),
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
                    new Token(0, 5, TokenType.Text),
                    new TagToken(5, 1, TagType.Italic,SubTagOrder.Opening),
                    new Token(6, 6,TokenType.Text),
                    new TagToken(12, 1, TagType.Italic,SubTagOrder.Closing),
                    new Token(13, 5, TokenType.Text)
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
                    new Token(0, 5, TokenType.Text),
                    new TagToken(5, 2,TagType.Strong, SubTagOrder.Opening),
                    new Token(7, 4, TokenType.Text),
                    new TagToken(11, 2, TagType.Strong, SubTagOrder.Closing),
                    new Token(13, 5, TokenType.Text)
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
                    new Token(0, 5, TokenType.Text),
                    new TagToken(5, 2,TagType.Strong, SubTagOrder.Opening),
                    new Token(7, 4, TokenType.Text),
                    new TagToken(11, 2,TagType.Strong, SubTagOrder.Closing),
                    new Token(13, 5, TokenType.Text),
                    new TagToken(18, 1,TagType.Italic, SubTagOrder.Opening),
                    new Token(19, 6, TokenType.Text),
                    new TagToken( 25, 1,TagType.Italic, SubTagOrder.Closing),
                    new Token(26, 5, TokenType.Text)
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
                    new Token(0, inputText.Length, TokenType.Text)
                }
            );
        }

        [TestMethod]
        public void Read_ReturnsValidTokensList_WhenUnpairedTagAndFullTag()
        {
            var inputText = "Unpaired_ italic tag and __bold__ tag";

            var reader = new TokenReader(new MdTagStorage());

            var tokens = reader.Read(inputText);

            tokens.Should().BeEquivalentTo(new[]
                {
                    new Token(0, 25, TokenType.Text),
                    new TagToken(25, 2,TagType.Strong, SubTagOrder.Opening),
                    new Token(27, 4, TokenType.Text),
                    new TagToken(31, 2, TagType.Strong,SubTagOrder.Closing),
                    new Token(33, 4, TokenType.Text),
                }
            );
        }


        [TestCase(@"One \_escaped\_ italic tag", TestName = "Escaped italic tag")]
        public void Read_ReturnsInputText_WhenTagIsEscaped(string inputText)
        {
            var reader = new TokenReader(new MdTagStorage());

            var tokens = reader.Read(inputText);

            tokens.Should().BeEquivalentTo(new[]
                {
                    new Token(0, inputText.Length - 2, TokenType.Text),
                }
            );
        }
    }
}
