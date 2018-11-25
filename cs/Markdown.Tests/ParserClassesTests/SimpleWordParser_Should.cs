using FluentAssertions;
using Markdown.ParserClasses;
using Markdown.ParserClasses.Nodes;
using Markdown.TokenizerClasses;
using NUnit.Framework;

namespace Markdown.Tests.ParserClassesTests
{
    public class SimpleWordParser_Should
    {
        private readonly Tokenizer tokenizer = new Tokenizer();
        private readonly Parser parser = new Parser();

        [TestCase("0", "0", TestName = "0")]
        [TestCase("1", "1", TestName = "1")]
        [TestCase("2", "2", TestName = "2")]
        [TestCase("3", "3", TestName = "3")]
        [TestCase("4", "4", TestName = "4")]
        [TestCase("5", "5", TestName = "5")]
        [TestCase("6", "6", TestName = "6")]
        [TestCase("7", "7", TestName = "7")]
        [TestCase("8", "8", TestName = "8")]
        [TestCase("9", "9", TestName = "9")]
        public void Parse_NumberSimpleWord_Successfully(string text, string expectedValue)
        {
            var tokens = tokenizer.Tokenize(text);
            var expected = new WordNode(WordType.SimpleWord, expectedValue);

            parser.SimpleWordParser(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [TestCase("theater", "theater", TestName = "plain text 'theater'")]
        [TestCase("pity", "pity", TestName = "plain text 'pity'")]
        [TestCase("gregarious", "gregarious", TestName = "plain text 'gregarious")]
        public void Parse_PlainTextSimpleWord_Successfully(string text, string expectedValue)
        {
            var tokens = tokenizer.Tokenize(text);
            var expected = new WordNode(WordType.SimpleWord, expectedValue);

            parser.SimpleWordParser(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }
    }
}