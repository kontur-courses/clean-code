using FluentAssertions;
using Markdown.ParserClasses;
using Markdown.ParserClasses.Nodes;
using Markdown.TokenizerClasses;
using NUnit.Framework;

namespace Markdown.Tests.ParserClassesTests
{
    public class ParseSpacedWord_Should
    {
        private readonly Tokenizer tokenizer = new Tokenizer();

        [TestCase(" 0", " 0", TestName = " 0")]
        [TestCase(" 1", " 1", TestName = " 1")]
        [TestCase(" 2", " 2", TestName = " 2")]
        [TestCase(" 3", " 3", TestName = " 3")]
        [TestCase(" 4", " 4", TestName = " 4")]
        [TestCase(" 5", " 5", TestName = " 5")]
        [TestCase(" 6", " 6", TestName = " 6")]
        [TestCase(" 7", " 7", TestName = " 7")]
        [TestCase(" 8", " 8", TestName = " 8")]
        [TestCase(" 9", " 9", TestName = " 9")]
        public void Parse_NumberSpacedWord_Successfully(string text, string expectedValue)
        {
            var tokens = tokenizer.Tokenize(text);
            var expected = new WordNode(WordType.SpacedWord, expectedValue);

            Parser.ParseSpacedWord(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [TestCase(" wage", " wage", TestName = "spaced plain text 'wage'")]
        [TestCase(" compromise", " compromise", TestName = "spaced plain text 'compromise'")]
        [TestCase(" instruction", " instruction", TestName = "spaced plain text 'instruction'")]
        public void Parse_PlainTextSpacedWord_Successfully(string text, string expectedValue)
        {
            var tokens = tokenizer.Tokenize(text);
            var expected = new WordNode(WordType.SpacedWord, expectedValue);

            Parser.ParseSpacedWord(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_Space_Successfully()
        {
            var text = " ";
            var tokens = tokenizer.Tokenize(text);
            var expected = new WordNode(WordType.Space, " ");

            Parser.ParseSpacedWord(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }
    }
}