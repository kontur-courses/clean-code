using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Token_Test
    {
        [TestCase(0, 8)]
        [TestCase(9, 2)]
        public void NextIndex_ShouldReturn_NextPositionAfterToken(int start, int length)
        {
            var token = new Token(start, length, MdTags.Bold, TextType.Default);
            token.NextIndex.Should().Be(start + length);
        }

        [Test]
        public void GetValue_ShouldReturn_RightText_WhenNoInternalTokens_AndNoConvertTagFunction()
        {
            const string text = "this is _italic_ text";
            var token = new Token(8, 8,MdTags.Italic, TextType.Italic);
            token.GetValue(type => new Tag("", ""), text)
                .Should().Be("italic");
        }

        [Test]
        public void GetValue_ShouldReturn_RightText_WhenHasInternalTokens_AndNoConvertTagFunction()
        {
            const string text = "this is _italic __bold1__ __bold2___ text";
            var token = new Token(8, 28, MdTags.Italic, TextType.Italic);
            var internalToken1 = new Token(16, 9, MdTags.Bold, TextType.Bold);
            var internalToken2 = new Token(26, 9, MdTags.Bold, TextType.Bold);
            token.AddInternalToken(internalToken1);
            token.AddInternalToken(internalToken2);
            token.GetValue(type => new Tag("", ""), text)
                .Should().Be("italic bold1 bold2");
        }

        [Test]
        public void GetValue_ShouldReturnDecoratedText_WhenHasConvertFunction()
        {
            const string text = "this is _italic __bold1__ __bold2___ text";
            var rootToken = new Token(0, 41, MdTags.Default, TextType.Default);
            var token = new Token(8, 28, MdTags.Italic, TextType.Italic);
            var internalToken1 = new Token(16, 9, MdTags.Bold, TextType.Bold);
            var internalToken2 = new Token(26, 9, MdTags.Bold, TextType.Bold);
            rootToken.AddInternalToken(token);
            token.AddInternalToken(internalToken1);
            token.AddInternalToken(internalToken2);
            rootToken.GetValue(ConvertFunction, text)
                .Should().Be("this is <i>italic <b>bold1</b> <b>bold2</b></i> text");
        }

        private static Tag ConvertFunction(TextType textType)
        {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (textType)
            {
                case TextType.Default:
                    return new Tag("", "");
                case TextType.Italic:
                    return new Tag("<i>", "</i>");
                case TextType.Bold:
                    return new Tag("<b>", "</b>");
                default:
                    return new Tag("<unknown>", "</unknown>");
            }
        }
    }
}