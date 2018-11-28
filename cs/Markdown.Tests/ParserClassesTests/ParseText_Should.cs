using FluentAssertions;
using Markdown.ParserClasses;
using Markdown.ParserClasses.Nodes;
using Markdown.TokenizerClasses;
using NUnit.Framework;

namespace Markdown.Tests.ParserClassesTests
{
    public class ParseText_Should
    {
        private readonly Tokenizer tokenizer = new Tokenizer();
        private readonly Parser parser = new Parser();

        [TestCase(" ", WordType.Space, " ", TestName = "space")]
        [TestCase("5", WordType.SimpleWord, "5", TestName = "simple num")]
        [TestCase("suit", WordType.SimpleWord, "suit", TestName = "simple plain text")]
        [TestCase(" 0", WordType.SpacedWord, " 0", TestName = "spaced num")]
        [TestCase(" door", WordType.SpacedWord, " door", TestName = "spaced plain text)]")]
        public void Parse_SingleWord_Successfully(string text, WordType type, string value)
        {
            var tokens = tokenizer.Tokenize(text);

            var expected = new TextNode();
            expected.Add(new WordNode(type, value));

            parser.ParseText(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_SpaceAndSpace_Successfully()
        {
            var text = "  ";
            var tokens = tokenizer.Tokenize(text);

            var expected = new TextNode();
            expected.Add(new WordNode(WordType.Space, " "));
            expected.Add(new WordNode(WordType.Space, " "));

            parser.ParseText(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [TestCase("wriggle", WordType.SimpleWord, TestName = "simple plain text ")]
        [TestCase("6", WordType.SimpleWord, TestName = "simple number")]
        [TestCase(" productive", WordType.SpacedWord, TestName = "spaced plain text")]
        [TestCase(" 8", WordType.SpacedWord, TestName = "spaced number")]
        public void Parse_WordAndSpace_Successfully(string word, WordType wordType)
        {
            var text = word + " ";
            var tokens = tokenizer.Tokenize(text);

            var expected = new TextNode();
            expected.Add(new WordNode(wordType, word));
            expected.Add(new WordNode(WordType.Space, " "));

            parser.ParseText(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [TestCase("flea market", WordType.SimpleWord, WordType.SpacedWord, "flea", " market",
            TestName = "simple plain text and spaced plain text")]
        [TestCase("tournament 5", WordType.SimpleWord, WordType.SpacedWord, "tournament", " 5",
            TestName = "simple plain text and spaced number")]
        [TestCase("temperature8", WordType.SimpleWord, WordType.SimpleWord, "temperature", "8",
            TestName = "simple plain text and simple number")]
        [TestCase("1 original", WordType.SimpleWord, WordType.SpacedWord, "1", " original",
            TestName = "simple number and spaced plain text")]
        [TestCase("7 3", WordType.SimpleWord, WordType.SpacedWord, "7", " 3",
            TestName = "simple number and spaced number")]
        [TestCase(" immune 0", WordType.SpacedWord, WordType.SpacedWord, " immune", " 0",
            TestName = "spaced plain text and spaced number")]
        public void Parse_TwoWords_Successfully(string text, WordType firstType, WordType secondType, string firstExpectedValue, string secondExpectedValue)
        {
            var tokens = tokenizer.Tokenize(text);

            var expected = new TextNode();
            expected.Add(new WordNode(firstType, firstExpectedValue));
            expected.Add(new WordNode(secondType, secondExpectedValue));

            parser.ParseText(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }
    }
}