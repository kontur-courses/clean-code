using System.Collections.Generic;
using FluentAssertions;
using Markdown.Converter;
using Markdown.Tags;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public class TokenConverter_Tests
    {
        private TokenConverter converter;
        
        [SetUp]
        public void SetUp()
        {
            var availableTags = new List<Tag>()
            {
                new BoldTag(), new HeaderTag(), new ItalicsTag()
            };

            converter = new TokenConverter(availableTags);
        }

        [Test]
        public void ConvertTokensInText_ReturnsConvertedText_WhenItalicsTagInText()
        {
            var text = "abc _xzc_ asd";
            var tokens = new List<IToken>()
            {
                new TagToken(new ItalicsTag(), 4, 8, "_xzc_")
            };

            var expectedText = "abc <em>xzc</em> asd";

            var convertedText = converter.ConvertTokensInText(tokens, text);

            convertedText.Should().Be(expectedText);
        }

        [Test]
        public void ConvertTokensInText_ReturnsConvertedText_WhenBoldTagInText()
        {
            var text = "abc __xzc__ asd";
            var tokens = new List<IToken>()
            {
                new TagToken(new BoldTag(), 4, 10, "__xzc__")
            };

            var expectedText = "abc <strong>xzc</strong> asd";

            var convertedText = converter.ConvertTokensInText(tokens, text);

            convertedText.Should().Be(expectedText);
        }
        
        [Test]
        public void ConvertTokensInText_ReturnsConvertedText_WhenHeaderTagInText()
        {
            var text = "# aaaaa";
            var tokens = new List<IToken>()
            {
                new TagToken(new HeaderTag(), 0, 6, "# aaaaa")
            };

            var expectedText = "<h1> aaaaa</h1>";

            var convertedText = converter.ConvertTokensInText(tokens, text);

            convertedText.Should().Be(expectedText);
        }

        [Test]
        public void ConvertTokensInText_ReturnsConvertedText_WhenItalicsTokenIsNested()
        {
            var text = "__xvc _as_ qwe__";
            var tokens = new List<IToken>()
            {
                new TagToken(new BoldTag(), 0, 15, "__xvc _as_ qwe__"),
                new TagToken(new ItalicsTag(), 6, 9, "_as_")
            };

            var expectedText = "<strong>xvc <em>as</em> qwe</strong>";

            var convertedText = converter.ConvertTokensInText(tokens, text);

            convertedText.Should().Be(expectedText);
        }
        
        [Test]
        public void ConvertTokensInText_ReturnsConvertedText_WhenTokensNested()
        {
            var text = "# asd __xvc _as_ qwe__";
            var tokens = new List<IToken>()
            {
                new TagToken(new HeaderTag(), 0, 21, "# asd __xvc _as_ qwe__"),
                new TagToken(new BoldTag(), 6, 21, "__xvc _as_ qwe__"),
                new TagToken(new ItalicsTag(), 12, 15, "_as_")
            };

            var expectedText = "<h1> asd <strong>xvc <em>as</em> qwe</strong></h1>";

            var convertedText = converter.ConvertTokensInText(tokens, text);

            convertedText.Should().Be(expectedText);
        }

        [Test]
        public void ConvertTokensInText_DoNotChangeText_WhenNoTokensInText()
        {
            var text = "asd asd xcx cvb";
            var tokens = new List<IToken>();

            var expectedText = "asd asd xcx cvb";

            var convertedText = converter.ConvertTokensInText(tokens, text);

            convertedText.Should().Be(text);
        }

        [TestCase(@"asd cvb \_zxc_", TestName = "WhenEscapeCharEscapesItalicsTag", ExpectedResult = "asd cvb _zxc_")]
        [TestCase(@"asd \\ zxc", TestName = "WhenEscapeCharIsEscaped", ExpectedResult = @"asd \ zxc")]
        [TestCase(@"Do\ no\t re\move\", TestName = "WhenEscapeCharIsNotEscaping", ExpectedResult = @"Do\ no\t re\move\")]
        public string ConvertTokensInText_ProcessesEscapeChars(string text)
        {
            var tokens = new List<IToken>();

            var convertedText = converter.ConvertTokensInText(tokens, text);

            return convertedText;
        }
    }
}