using System.Collections.Generic;
using FluentAssertions;
using Markdown.Parser;
using Markdown.Tags;
using Markdown.Tokens;
using Markdown.Validator;
using NUnit.Framework;

namespace MarkdownTests
{
    public class TokenValidator_Tests
    {
        private TextParser parser;
        private TokenValidator validator;
        
        [SetUp]
        public void SetUp()
        {
            var availableTags = new List<Tag>()
            {
                new BoldTag(), new HeaderTag(), new ItalicsTag()
            };

            parser = new TextParser(availableTags);
            validator = new TokenValidator();
        }

        [TestCase("text with digits_12_3", TestName = "WhenTokenContainsDigits")]
        [TestCase("aa ____", TestName = "WhenTokenIsEmpty")]
        [TestCase("in diff_erent wo_rds", TestName = "WhenTokenIsInsideOfDifferentWords")]
         public void ValidateTokens_ReturnsEmptyCollection(string text)
        {
            var tokens = parser.GetTokens(text);
            
            var validatedTokens = validator.ValidateTokens(tokens, text);

            validatedTokens.Should().BeEmpty();
        }

        [Test]
        public void ValidateTokens_RemovesNestedToken_WhenBoldTagIsNestedInItalicsTag()
        {
            var text = "asd _a __vc__ d_ asd";
            var tokens = parser.GetTokens(text);
            var expectedTokens = new List<TagToken>()
            {
                new TagToken(new ItalicsTag(), 4, 15, "_a __vc__ d_")
            };

            var validatedTokens = validator.ValidateTokens(tokens, text);

            validatedTokens.Should().BeEquivalentTo(expectedTokens);
        }
    }
}