using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.BasicTextTokenizer.Tests
{
    public class TextTokenizerTests
    {
        private TextTokenizer tokenizer;
        public static readonly ItalicTagClassifier ItalicTag = new ItalicTagClassifier();
        public static readonly BoldTagClassifier BoldTag = new BoldTagClassifier();
        public static readonly LinkTextTagClassifier LinkTextTag = new LinkTextTagClassifier();
        public static readonly LinkUriTagClassifier LinkUriTag = new LinkUriTagClassifier();

        [SetUp]
        public void SetUp()
        {
            const char escapeSymbol = '\\';
            var escapableSymbols = new[] { '_', escapeSymbol, '[', ']', '(', ')' };
            bool IsEscapeSequence(string text, int position) => position + 1 < text.Length
                                                      && text[position] == escapeSymbol
                                                      && escapableSymbols.Contains(text[position + 1]);
            tokenizer = new TextTokenizer(new ITagClassifier[] { ItalicTag, BoldTag, LinkTextTag, LinkUriTag },
                IsEscapeSequence);
        }

        [Test]
        public void GetTokens_ShouldReturnOneTextToken_OnInputWithoutSymbols()
        {
            var text = "abcd ef g";
            var expectedResult = Token.CreateTextToken(0, text.Length);
            var result = tokenizer.GetTokens(text);
            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }

        [TestCaseSource(typeof(SingleUnderscoreCases))]
        [TestCaseSource(typeof(NestingCases))]
        [TestCaseSource(typeof(EscapedCases))]
        [TestCaseSource(typeof(LinksCases))]
        public void GetTokens_ShouldReturnCorrectTokens(string text, List<Token> expectedTokens)
        {
            var result = tokenizer.GetTokens(text).ToList();

            result.Should().BeEquivalentTo(expectedTokens, options => options.IgnoringCyclicReferences());
        }
    }

    internal class SingleUnderscoreCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return GetFullyUnderscoredItalicToken();
            yield return GetFullyUnderscoredBoldToken();
            yield return GetInsideUnderscoredItalicToken();
            yield return GetInsideUnderscoredBoldToken();
        }

        private static object[] GetFullyUnderscoredItalicToken()
        {
            var text = "_abcd_";

            var openingToken = Token.CreateControllingToken(0, 1,
                TokenType.Opening, TextTokenizerTests.ItalicTag, null);
            var closingToken = Token.CreateControllingToken(text.Length - 1, 1,
                TokenType.Ending, TextTokenizerTests.ItalicTag, openingToken);
            openingToken.PairedToken = closingToken;
            var expectedTokens = new List<Token>
            {
                openingToken,
                Token.CreateTextToken(1, 4),
                closingToken
            };

            return new object[] { text, expectedTokens };
        }

        private static object[] GetFullyUnderscoredBoldToken()
        {
            var text = "__abcd__";

            var openingToken = Token.CreateControllingToken(0, 2,
                TokenType.Opening, TextTokenizerTests.BoldTag, null);
            var closingToken = Token.CreateControllingToken(text.Length - 2, 2,
                TokenType.Ending, TextTokenizerTests.BoldTag, openingToken);
            openingToken.PairedToken = closingToken;
            var expectedTokens = new List<Token>
            {
                openingToken,
                Token.CreateTextToken(2, 4),
                closingToken
            };

            return new object[] { text, expectedTokens };
        }

        private static object[] GetInsideUnderscoredItalicToken()
        {
            var text = $"ef _abcd_ jk";

            var openingToken = Token.CreateControllingToken(3, 1,
                TokenType.Opening, TextTokenizerTests.ItalicTag, null);
            var closingToken = Token.CreateControllingToken("ef _abcd".Length, 1,
                TokenType.Ending, TextTokenizerTests.ItalicTag, openingToken);
            openingToken.PairedToken = closingToken;

            var expectedTokens = new List<Token>
            {
                Token.CreateTextToken(0, 3),
                openingToken,
                Token.CreateTextToken(4, 4),
                closingToken,
                Token.CreateTextToken(text.Length - 3, 3)
            };

            return new object[] { text, expectedTokens };
        }

        private static object[] GetInsideUnderscoredBoldToken()
        {
            var text = $"ef __abcd__ jk";

            var openingToken = Token.CreateControllingToken(3, 2,
                TokenType.Opening, TextTokenizerTests.BoldTag, null);
            var closingToken = Token.CreateControllingToken("ef __abcd".Length, 2,
                TokenType.Ending, TextTokenizerTests.BoldTag, openingToken);
            openingToken.PairedToken = closingToken;

            var expectedTokens = new List<Token>
            {
                Token.CreateTextToken(0, 3),
                openingToken,
                Token.CreateTextToken(5, 4),
                closingToken,
                Token.CreateTextToken(text.Length - 3, 3)
            };

            return new object[] { text, expectedTokens };
        }
    }

    internal class NestingCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return GetOneNestingUnderscore();
            yield return GetFewNestingUnderscores();
            yield return GetOpeningItalicRightAfterOpeningBold();
            yield return GetClosingBoldRightAfterClosingItalic();
        }

        private static object[] GetOneNestingUnderscore()
        {
            var text = "a __bc _de_ fg__ p";

            var boldOpening = Token.CreateControllingToken(2, 2, 
                TokenType.Opening, TextTokenizerTests.BoldTag, null);
            var boldClosing = Token.CreateControllingToken("a __bc _de_ fg".Length, 2, 
                TokenType.Ending, TextTokenizerTests.BoldTag, boldOpening);
            boldOpening.PairedToken = boldClosing;

            var italicOpening = Token.CreateControllingToken("a __bc ".Length, 1, 
                TokenType.Opening, TextTokenizerTests.ItalicTag, null);
            var italicClosing = Token.CreateControllingToken("a __bc _de".Length, 1, 
                TokenType.Ending, TextTokenizerTests.ItalicTag, italicOpening);
            italicOpening.PairedToken = italicClosing;

            var expectedTokens = new List<Token>
                {
                    Token.CreateTextToken(0, 2),
                    boldOpening,
                    Token.CreateTextToken("a __".Length, 3),
                    italicOpening,
                    Token.CreateTextToken("a __bc _".Length, 2),
                    italicClosing,
                    Token.CreateTextToken("a __bc _de_".Length, 3),
                    boldClosing,
                    Token.CreateTextToken("a __bc _de_ fg__".Length, 2)
                };

            return new object[] { text, expectedTokens };
        }

        private static object[] GetFewNestingUnderscores()
        {
            var text = "__abcd _ef_ ghi _jk_ m__";

            var boldOpening = Token.CreateControllingToken(0, 2, 
                TokenType.Opening, TextTokenizerTests.BoldTag, null);
            var boldClosing = Token.CreateControllingToken(text.Length - 2, 2, 
                TokenType.Ending, TextTokenizerTests.BoldTag, boldOpening);
            boldOpening.PairedToken = boldClosing;

            var firstItalicOpening = Token.CreateControllingToken("__abcd ".Length, 1, 
                TokenType.Opening, TextTokenizerTests.ItalicTag, null);
            var firstItalicClosing = Token.CreateControllingToken("__abcd _ef".Length, 1, 
                TokenType.Ending, TextTokenizerTests.ItalicTag, firstItalicOpening);
            firstItalicOpening.PairedToken = firstItalicClosing;

            var secondItalicOpening = Token.CreateControllingToken("__abcd _ef_ ghi ".Length, 1, 
                TokenType.Opening, TextTokenizerTests.ItalicTag, null);
            var secondItalicClosing = Token.CreateControllingToken("__abcd _ef_ ghi _jk".Length, 1, 
                TokenType.Ending, TextTokenizerTests.ItalicTag, secondItalicOpening);
            secondItalicOpening.PairedToken = secondItalicClosing;

            var expectedTokens = new List<Token>
            {
                boldOpening,
                Token.CreateTextToken(2, 5),
                firstItalicOpening,
                Token.CreateTextToken("__abcd _".Length, 2),
                firstItalicClosing,
                Token.CreateTextToken("__abcd _ef_".Length, 5),
                secondItalicOpening,
                Token.CreateTextToken("__abcd _ef_ ghi _".Length, 2),
                secondItalicClosing,
                Token.CreateTextToken("__abcd _ef_ ghi _jk_".Length, 2),
                boldClosing
            };

            return new object[] { text, expectedTokens };
        }

        private static object[] GetOpeningItalicRightAfterOpeningBold()
        {
            var text = "___a_ bcd__";

            var boldOpening = Token.CreateControllingToken(0, 2, 
                TokenType.Opening, TextTokenizerTests.BoldTag, null);
            var boldClosing = Token.CreateControllingToken(text.Length - 2, 2, 
                TokenType.Ending, TextTokenizerTests.BoldTag, boldOpening);
            boldOpening.PairedToken = boldClosing;

            var italicOpening = Token.CreateControllingToken(2, 1, 
                TokenType.Opening, TextTokenizerTests.ItalicTag, null);
            var italicClosing = Token.CreateControllingToken(4, 1, 
                TokenType.Ending, TextTokenizerTests.ItalicTag, italicOpening);
            italicOpening.PairedToken = italicClosing;

            var expectedTokens = new List<Token>
            {
                boldOpening,
                italicOpening,
                Token.CreateTextToken(3, 1),
                italicClosing,
                Token.CreateTextToken("___a_".Length, 4),
                boldClosing
            };

            return new object[] { text, expectedTokens };
        }

        private static object[] GetClosingBoldRightAfterClosingItalic()
        {
            var text = "__abc _d___";

            var boldOpening = Token.CreateControllingToken(0, 2, 
                TokenType.Opening, TextTokenizerTests.BoldTag, null);
            var boldClosing = Token.CreateControllingToken(text.Length - 2, 2,
                TokenType.Ending, TextTokenizerTests.BoldTag, boldOpening);
            boldOpening.PairedToken = boldClosing;

            var italicOpening = Token.CreateControllingToken("__abc ".Length, 1, 
                TokenType.Opening, TextTokenizerTests.ItalicTag, null);
            var italicClosing = Token.CreateControllingToken(text.Length - 3, 1, 
                TokenType.Ending, TextTokenizerTests.ItalicTag, italicOpening);
            italicOpening.PairedToken = italicClosing;

            var expectedTokens = new List<Token>
            {
                boldOpening,
                Token.CreateTextToken(2, 4),
                italicOpening,
                Token.CreateTextToken(italicOpening.Position + 1, 1),
                italicClosing,
                boldClosing
            };

            return new object[] { text, expectedTokens };
        }
    }

    internal class EscapedCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return  GetEscapedItalicText();
            yield return GetItalicTextWithEscapedEscapeSymbol();
        }

        private object[] GetEscapedItalicText() 
        {
            var text = @"_a \_ c_";
            var italicOpening = Token.CreateControllingToken(0, 1, 
                TokenType.Opening, TextTokenizerTests.ItalicTag, null);
            var italicClosing = Token.CreateControllingToken(text.Length - 1, 1, 
                TokenType.Ending, TextTokenizerTests.ItalicTag, italicOpening);
            italicOpening.PairedToken = italicClosing;

            var expectedTokens = new List<Token>
            {
                italicOpening,
                Token.CreateTextToken(1, 2),
                Token.CreateTextToken(4, 3),
                italicClosing
            };

            return new object[] { text, expectedTokens };
        }

        private object[] GetItalicTextWithEscapedEscapeSymbol()
        {
            var text = @"_\\_";
            var italicOpening = Token.CreateControllingToken(0, 1, 
                TokenType.Opening, TextTokenizerTests.ItalicTag, null);
            var italicClosing = Token.CreateControllingToken(text.Length - 1, 1, 
                TokenType.Ending, TextTokenizerTests.ItalicTag, italicOpening);
            italicOpening.PairedToken = italicClosing;

            var expectedTokens = new List<Token>
            {
                italicOpening,
                Token.CreateTextToken(2, 1),
                italicClosing
            };

            return new object[] { text, expectedTokens };
        }
    }

    internal class LinksCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            yield return GetSimpleLink();
        }

        private static object[] GetSimpleLink()
        {
            var text = "[abc](cde)";

            var openingText = Token.CreateControllingToken(0, 1,
                TokenType.Opening, TextTokenizerTests.LinkTextTag, null);
            var closingText = Token.CreateControllingToken(4, 1,
                TokenType.Ending, TextTokenizerTests.LinkTextTag, openingText);
            openingText.PairedToken = closingText;

            var openingUri = Token.CreateControllingToken(5, 1,
                TokenType.Opening, TextTokenizerTests.LinkUriTag, null);
            var closingUri = Token.CreateControllingToken(text.Length - 1, 1,
                TokenType.Ending, TextTokenizerTests.LinkUriTag, openingUri);
            openingUri.PairedToken = closingUri;

            var expectedTokens = new List<Token>
            {
                openingText,
                Token.CreateTextToken(1, 3),
                closingText,
                openingUri,
                Token.CreateTextToken(6, 3),
                closingUri
            };

            return new object[] {text, expectedTokens};
        }
    }
}
