using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.BasicTextTokenizer.Tests
{
    public class TextTokenizerTests
    {
        private TextTokenizer tokenizer;
        private readonly ItalicTagClassifier italicTag = new ItalicTagClassifier();
        private readonly BoldTagClassifier boldTag = new BoldTagClassifier();

        [SetUp]
        public void SetUp()
        {
            const char escapeSymbol = '\\';
            var escapableSymbols = new[] { '_', '\\' };
            bool IsEscapeSequence(string s, int i) => i + 1 < s.Length 
                                                      && s[i] == escapeSymbol 
                                                      && escapableSymbols.Contains(s[i + 1]);
            tokenizer = new TextTokenizer(new ITagClassifier[] { italicTag, boldTag },
                IsEscapeSequence);
        }

        [Test]
        public void GetTokens_ShouldReturnOneTextToken_OnInputWithoutSymbols()
        {
            var text = "abcd ef g";
            var expectedResult = new Token(0, text.Length);
            var result = tokenizer.GetTokens(text);
            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }

        [TestCase("_", TestName = "One underscore")]
        [TestCase("__", TestName = "Double underscore")]
        public void GetTokens_ShouldReturnThreeTokens_OnFullyUnderscoredInput(string underscore)
        {
            var text = $"{underscore}abcd{underscore}";
            var classifier = underscore.Length == 1 ? (ITagClassifier) italicTag : boldTag;
            var openingToken = new Token(0, underscore.Length, TokenType.Opening, classifier);
            var closingToken = new Token(text.Length - underscore.Length, underscore.Length, TokenType.Ending, 
                classifier, openingToken);
            openingToken.PairedToken = closingToken;
            var expectedResult = new List<Token>
            {
                openingToken,
                new Token(underscore.Length, 4),
                closingToken
            };

            var result = tokenizer.GetTokens(text).ToList();

            result.Should().BeEquivalentTo(expectedResult, options => options.IgnoringCyclicReferences());
            result[0].PairedToken.Should().BeEquivalentTo(result[2]);
            result[2].PairedToken.Should().BeEquivalentTo(result[0]);
        }

        [TestCase("_", TestName = "One underscore")]
        [TestCase("__", TestName = "Double underscore")]
        public void GetTokens_ShouldReturnFiveTokens_OnInputWithInsideUnderscore(string underscore)
        {
            var text = $"ef {underscore}abcd{underscore} jk";
            var classifier = underscore.Length == 1 ? (ITagClassifier)italicTag : boldTag;
            var openingToken = new Token(3, underscore.Length, TokenType.Opening, classifier);
            var closingToken = new Token(text.Length - underscore.Length - 3, underscore.Length, TokenType.Ending,
                classifier, openingToken);
            openingToken.PairedToken = closingToken;
            var expectedResult = new List<Token>
            {
                new Token(0, 3),
                openingToken,
                new Token(underscore.Length + 3, 4),
                closingToken,
                new Token(text.Length - 3, 3)
            };

            var result = tokenizer.GetTokens(text).ToList();

            result.Should().BeEquivalentTo(expectedResult, options => options.IgnoringCyclicReferences());
        }

        [Test]
        public void GetTokens_ShouldReturnCorrectTokens_OnOneNestingUnderscore()
        {
            var text = "a __bc _de_ fg__ p";
            var boldOpening = new Token(2, 2, TokenType.Opening, boldTag);
            var boldClosing = new Token("a __bc _de_ fg".Length, 2, TokenType.Ending, boldTag, boldOpening);
            boldOpening.PairedToken = boldClosing;
            var italicOpening = new Token("a __bc ".Length, 1, TokenType.Opening, italicTag);
            var italicClosing = new Token("a __bc _de".Length, 1, TokenType.Ending, italicTag, italicOpening);
            italicOpening.PairedToken = italicClosing;
            var expectedResult = new List<Token>
            {
                new Token(0, 2),
                boldOpening,
                new Token("a __".Length, 3),
                italicOpening,
                new Token("a __bc _".Length, 2),
                italicClosing,
                new Token("a __bc _de_".Length, 3),
                boldClosing,
                new Token("a __bc _de_ fg__".Length, 2)
            };

            var result = tokenizer.GetTokens(text).ToList();

            result.Should().BeEquivalentTo(expectedResult, options => options.IgnoringCyclicReferences());
        }

        [Test]
        public void GetTokens_ShouldReturnCorrectTokens_OnFewNestingUnderscore()
        {
            var text = "__abcd _ef_ ghi _jk_ m__";
            var boldOpening = new Token(0, 2, TokenType.Opening, boldTag);
            var boldClosing = new Token(text.Length - 2, 2, TokenType.Ending, boldTag, boldOpening);
            boldOpening.PairedToken = boldClosing;
            var firstItalicOpening = new Token("__abcd ".Length, 1, TokenType.Opening, italicTag);
            var firstItalicClosing = new Token("__abcd _ef".Length, 1, TokenType.Ending, italicTag, firstItalicOpening);
            firstItalicOpening.PairedToken = firstItalicClosing;
            var secondItalicOpening = new Token("__abcd _ef_ ghi ".Length, 1, TokenType.Opening, italicTag);
            var secondItalicClosing = new Token("__abcd _ef_ ghi _jk".Length, 1, TokenType.Ending, italicTag, secondItalicOpening);
            secondItalicOpening.PairedToken = secondItalicClosing;
            
            var expectedResult = new List<Token>
            {
                boldOpening,
                new Token(2, 5),
                firstItalicOpening,
                new Token("__abcd _".Length, 2),
                firstItalicClosing,
                new Token("__abcd _ef_".Length, 5),
                secondItalicOpening,
                new Token("__abcd _ef_ ghi _".Length, 2),
                secondItalicClosing,
                new Token("__abcd _ef_ ghi _jk_".Length, 2),
                boldClosing
            };

            var result = tokenizer.GetTokens(text).ToList();

            result.Should().BeEquivalentTo(expectedResult, options => options.IgnoringCyclicReferences());
        }

        [Test]
        public void GetTokens_ShouldReturnCorrectTokens_OnEscapedItalicText()
        {
            var text = @"_a \_ c_";
            var italicOpening = new Token(0, 1, TokenType.Opening, italicTag);
            var italicClosing = new Token(text.Length - 1, 1, TokenType.Ending, italicTag, italicOpening);
            italicOpening.PairedToken = italicClosing;
            var expectedResult = new List<Token>
            {
                italicOpening,
                new Token(1, 2),
                new Token(4, 3),
                italicClosing
            };

            var result = tokenizer.GetTokens(text).ToList();

            result.Should().BeEquivalentTo(expectedResult, options => options.IgnoringCyclicReferences());
        }

        [Test]
        public void GetTokens_ShouldReturnCorrectTokens_OnItalicWithEscapedEscapeSymbol()
        {
            var text = @"_\\_";
            var italicOpening = new Token(0, 1, TokenType.Opening, italicTag);
            var italicClosing = new Token(text.Length - 1, 1, TokenType.Ending, italicTag, italicOpening);
            italicOpening.PairedToken = italicClosing;
            var expectedResult = new List<Token>
            {
                italicOpening,
                new Token(2, 1),
                italicClosing
            };

            var result = tokenizer.GetTokens(text).ToList();

            result.Should().BeEquivalentTo(expectedResult, options => options.IgnoringCyclicReferences());
        }
    }
}
