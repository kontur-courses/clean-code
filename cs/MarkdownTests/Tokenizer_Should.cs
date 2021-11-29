using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Markdown.Tokens;
using Markdown.Tokens.Parsers;
using NUnit.Framework;

namespace MarkdownTests
{
    public class Tokenizer_Should
    {
        private readonly Tokenizer tokenizer = new();
        
        [TestCase("\n\n", TestName = "When two \n symbols given")]
        [TestCase("\n     \n", TestName = "When two \n symbols given")]
        [TestCase("\n\n\n\n\n\n", TestName = "When multiplied \n symbols given")]
        public void AlwaysReturnEndParagraphToken(string rawString)
        {
            var result = tokenizer.GetTokens(rawString);
            
            result.Last().Should().BeOfType<ParagraphEndToken>();
        }
        
        [TestCase("_", typeof(ItalicToken), TestName = "When one underline given")]
        [TestCase("__", typeof(BoldToken), TestName = "When two underlines given")]
        [TestCase("#", typeof(Header1Token), TestName = "When grid symbol given")]
        [TestCase(" ", typeof(SpaceToken), TestName = "When space symbol given")]
        [TestCase("\n", typeof(EndLineToken), TestName = "When \\n symbol given")]
        [TestCase("[", typeof(OpeningSquareBracketToken), TestName = "When opening square bracket given")]
        [TestCase("]", typeof(ClosingSquareBracketToken), TestName = "When closing square bracket given")]
        [TestCase("(", typeof(OpeningRoundBracketToken), TestName = "When opening round bracket given")]
        [TestCase(")", typeof(ClosingRoundBracketToken), TestName = "When closing round bracket given")]
        public void ReturnMarkingTag_When(string rawString, Type expectedType)
        {
            var expectedTypes = new List<Type> {expectedType, typeof(ParagraphEndToken)};
            
            var result = tokenizer.GetTokens(rawString);

            AssertResultValuesHasExpectedTypes(result, expectedTypes);
        }
        
        [TestCase(@"\ ", typeof(SpaceToken), TestName = "When space been escaped")]
        [TestCase(@"\_", typeof(ItalicToken), TestName = "When one underline was escaped")]
        [TestCase(@"\_\_", typeof(BoldToken), TestName = "When two underlines was escaped")]
        [TestCase("\\\n", typeof(EndLineToken), TestName = "When \\n was escaped")]
        public void ShouldEscapeMarkingSymbols(string rawString, Type nonEscapedTokenType)
        {
            var result = tokenizer.GetTokens(rawString);

            result.Should().NotBeAssignableTo(nonEscapedTokenType);
        }
        
        [Test]
        public void ShouldNotEscapeNonMarkingSymbols()
        {
            var rawString = @"\a\2\c\4";

            var expectedWord = new WordToken(rawString);
            
            var result = tokenizer.GetTokens(rawString);

            using (new AssertionScope())
            {
                result.Should().HaveCount(2);
                result.First().Should().BeEquivalentTo(expectedWord);
            }
        }

        [TestCase("abcdef", "abcdef", TestName = "Multiplied Symbols Without Marking Given")]
        [TestCase(@"\_hello", "_hello", TestName = "Marking Symbol been escaped in the beginning")]
        [TestCase(@"hel\_lo", "hel_lo", TestName = "Marking Symbol been escaped in the middle")]
        [TestCase(@"hello\_", "hello_", TestName = "Marking Symbol been escaped in the end")]
        [TestCase("\\#\\_\\\n", "#_\n", TestName = "Multiplied marking")]
        public void ReturnOneWord_When(string rawString, string expectedWord)
        {
            var expectedToken = new WordToken(expectedWord);
            
            var result = tokenizer.GetTokens(rawString).ToArray();
            
            result.First().Should().BeEquivalentTo(expectedToken);
        }

        [TestCase("__ abc _", "B W IP", TestName = "A KAK NAZVATb")]
        [TestCase("[Link](vk.com)", "[W](W)P", TestName = "A KAK NAZVATb")]
        public void ReturnMultipliedTokens_When(string rawString, string expectedTokens)
        {
            var expectedTokensTypes = GetTokensFromStringWithFirstLetters(expectedTokens);
            var result = tokenizer.GetTokens(rawString).ToList();

            using (new AssertionScope())
            {
                result.Should().HaveSameCount(expectedTokensTypes);
                for (var i = 0; i < result.Count; i++)
                {
                    result[i].Should().BeOfType(expectedTokensTypes[i]);
                }
            }
        }

        [TestCase("abc\n\n", TestName = "Paragraph was ended with two new lines")]
        [TestCase("abc\n\n\n\n\n", TestName = "Paragraph was ended with more than two new lines")]
        [TestCase("abc\n\t\t\t\n", TestName = "\\t symbols given after new line")]
        [TestCase("abc\n    \n", TestName = "Space symbols given after new line")]
        public void ReturnParagraphEndToken_When(string rawString)
        {
            var tokens = tokenizer.GetTokens(rawString);
            tokens.Should().Contain(x => x is ParagraphEndToken);
        }
        
        [TestCase("abc", TestName = "Paragraph was not closed with empty line")]
        [TestCase("abc\ndef\n", TestName = "Line after \n contained non separator symbols")]
        [TestCase("abc\ndef\n", TestName = "Line after \n contained words and separator symbols")]
        public void ReturnTokensWithOnlyOneParagraphEndTokenInTheEndWhen_(string rawString)
        {
            var tokens = tokenizer.GetTokens(rawString);
            using (new AssertionScope())
            {
                tokens.SkipLast(1).Should().NotContain(x => x is ParagraphEndToken);
                tokens.Last().Should().BeOfType<ParagraphEndToken>();
            }
        }

        private void AssertResultValuesHasExpectedTypes(List<IToken> tokens, List<Type> expectedTypes)
        {
            using (new AssertionScope())
            {
                tokens.Should().HaveSameCount(expectedTypes);
                for (var i = 0; i < tokens.Count; i++)
                {
                    tokens[i].Should().BeOfType(expectedTypes[i]);
                }
            }
        }
        
        
        private List<Type> GetTokensFromStringWithFirstLetters(string tokens)
        {
            var types = new List<Type>();

            foreach (var symbol in tokens)
            {
                switch (symbol)
                {
                    case 'I':
                        types.Add(typeof(ItalicToken));
                        break;
                    case 'B':
                        types.Add(typeof(BoldToken));
                        break;
                    case ' ':
                        types.Add(typeof(SpaceToken));
                        break;
                    case '\n':
                        types.Add(typeof(EndLineToken));
                        break;
                    case 'H':
                        types.Add(typeof(Header1Parser));
                        break;
                    case 'W':
                        types.Add(typeof(WordToken));
                        break;
                    case 'P':
                        types.Add(typeof(ParagraphEndToken));
                        break;
                    case '[':
                        types.Add(typeof(OpeningSquareBracketToken));
                        break;
                    case ']':
                        types.Add(typeof(ClosingSquareBracketToken));
                        break;
                    case '(':
                        types.Add(typeof(OpeningRoundBracketToken));
                        break;
                    case ')':
                        types.Add(typeof(ClosingRoundBracketToken));
                        break;
                    default:
                        throw new Exception("Unknown token type in test argument");
                }
            }

            return types;
        }
    }
}