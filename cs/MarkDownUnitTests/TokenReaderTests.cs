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
                    new TypedToken(0, 2,TokenType.Tag, TagType.Header,SubTagOrder.Opening),
                    new TypedToken(2, 13, TokenType.Text),
                    new TypedToken(15, 1,TokenType.Tag, TagType.Header, SubTagOrder.Closing),
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
                    new TypedToken(0, 5, TokenType.Text),
                    new TypedToken(5, 1,TokenType.Tag, TagType.Italic,SubTagOrder.Opening),
                    new TypedToken(6, 6,TokenType.Text),
                    new TypedToken(12, 1,TokenType.Tag, TagType.Italic,SubTagOrder.Closing),
                    new TypedToken(13, 5, TokenType.Text)
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
                    new TypedToken(0, 5, TokenType.Text),
                    new TypedToken(5, 2, TokenType.Tag,TagType.Strong, SubTagOrder.Opening),
                    new TypedToken(7, 4, TokenType.Text),
                    new TypedToken(11, 2, TokenType.Tag, TagType.Strong, SubTagOrder.Closing),
                    new TypedToken(13, 5, TokenType.Text)
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
                    new TypedToken(0, 5, TokenType.Text),
                    new TypedToken(5, 2,TokenType.Tag,TagType.Strong, SubTagOrder.Opening),
                    new TypedToken(7, 4, TokenType.Text),
                    new TypedToken(11, 2,TokenType.Tag,TagType.Strong, SubTagOrder.Closing),
                    new TypedToken(13, 5, TokenType.Text),
                    new TypedToken(18, 1,TokenType.Tag,TagType.Italic, SubTagOrder.Opening),
                    new TypedToken(19, 6, TokenType.Text),
                    new TypedToken( 25, 1,TokenType.Tag,TagType.Italic, SubTagOrder.Closing),
                    new TypedToken(26, 5, TokenType.Text)
                }
            );
        }

        [TestMethod]
        public void Read_ReturnsValidTokensList_WhenTextIsWithUnpairedOpeningItalicTag()
        {
            var inputText = "One _unpaired opening italic tag";

            var reader = new TokenReader(new MdTagStorage());

            var tokens = reader.Read(inputText);

            tokens.Should().BeEquivalentTo(new[]
                {
                    new TypedToken(0, 4, TokenType.Text),
                    new TypedToken(4, 1, TokenType.Text),
                    new TypedToken(5, 27, TokenType.Text),
                }
            );
        }

        [TestMethod]
        public void Read_ReturnsValidTokensList_WhenTextIsWithUnpairedClosingItalicTag()
        {
            var inputText = "One unpaired_ closing italic tag";

            var reader = new TokenReader(new MdTagStorage());

            var tokens = reader.Read(inputText);

            tokens.Should().BeEquivalentTo(new[]
                {
                    new TypedToken(0, 12, TokenType.Text),
                    new TypedToken(12, 1, TokenType.Text),
                    new TypedToken(13, 19, TokenType.Text),
                }
            );
        }

        [TestMethod]
        public void Read_ReturnsValidTokensList_WhenTextIsWithUnpairedOpeningBoldTag()
        {
            var inputText = "One __unpaired opening bold tag";

            var reader = new TokenReader(new MdTagStorage());

            var tokens = reader.Read(inputText);

            tokens.Should().BeEquivalentTo(new[]
                {
                    new TypedToken(0, 4, TokenType.Text),
                    new TypedToken(4, 2, TokenType.Text),
                    new TypedToken(6, 25, TokenType.Text),
                }
            );
        }

        [TestMethod]
        public void Read_ReturnsValidTokensList_WhenTextIsWithUnpairedClosingBoldTag()
        {
            var inputText = "One unpaired__ closing bold tag";

            var reader = new TokenReader(new MdTagStorage());

            var tokens = reader.Read(inputText);

            tokens.Should().BeEquivalentTo(new[]
                {
                    new TypedToken(0, 12, TokenType.Text),
                    new TypedToken(12, 2, TokenType.Text),
                    new TypedToken(14, 17, TokenType.Text),
                }
            );
        }

        [TestMethod]
        public void Read_ReturnsValidTokensList_WhenTextIsWithUnpairedOpeningHeaderTag()
        {
            var inputText = "# One unpaired opening header tag";

            var reader = new TokenReader(new MdTagStorage());

            var tokens = reader.Read(inputText);

            tokens.Should().BeEquivalentTo(new[]
                {
                    new TypedToken(0, 2, TokenType.Text),
                    new TypedToken(2, 31, TokenType.Text),
                }
            );
        }

        [TestMethod]
        public void Read_ReturnsValidTokensList_WhenTextIsWithUnpairedTagAndFullTag()
        {
            var inputText = "Unpaired_ italic tag and __bold__ tag";

            var reader = new TokenReader(new MdTagStorage());

            var tokens = reader.Read(inputText);

            tokens.Should().BeEquivalentTo(new[]
                {
                    new TypedToken(0, 8, TokenType.Text),
                    new TypedToken(8, 1, TokenType.Text),
                    new TypedToken(9, 16, TokenType.Text),
                    new TypedToken(25, 2,TokenType.Tag,TagType.Strong, SubTagOrder.Opening),
                    new TypedToken(27, 4, TokenType.Text),
                    new TypedToken(31, 2,TokenType.Tag, TagType.Strong,SubTagOrder.Closing),
                    new TypedToken(33, 4, TokenType.Text),
                }
            );
        }

        [TestMethod]
        public void Read_ReturnsValidTokensList_WhenTextIsWithEscapedTag()
        {
            var inputText = @"One \_escaped\_ italic tag";

            var reader = new TokenReader(new MdTagStorage());

            var tokens = reader.Read(inputText);

            tokens.Should().BeEquivalentTo(new[]
                {
                    new TypedToken(0, 4, TokenType.Text),
                    new TypedToken(5,1,TokenType.Text),
                    new TypedToken(6,7,TokenType.Text),
                    new TypedToken(14,1,TokenType.Text),
                    new TypedToken(15,11,TokenType.Text)
                }
            );
        }

        [TestMethod]
        public void Read_ReturnsValidTokensList_WhenTextIsWithDoubleEscapedCharacter()
        {
            var inputText = @"Double \\escaped character";

            var reader = new TokenReader(new MdTagStorage());

            var tokens = reader.Read(inputText);

            tokens.Should().BeEquivalentTo(new[]
                {
                    new TypedToken(0, 7, TokenType.Text),
                    new TypedToken(8,1,TokenType.Text),
                    new TypedToken(9,17,TokenType.Text)
                }
            );
        }

        [TestMethod]
        public void Read_ReturnsValidTokensList_WhenTextIsWithEscapeInsideWord()
        {
            var inputText = @"Escape insi\de word";

            var reader = new TokenReader(new MdTagStorage());

            var tokens = reader.Read(inputText);

            tokens.Should().BeEquivalentTo(new[]
                {
                    new TypedToken(0, 11, TokenType.Text),
                    new TypedToken(11,1,TokenType.Text),
                    new TypedToken(12,7,TokenType.Text)
                }
            );
        }
    }
}
