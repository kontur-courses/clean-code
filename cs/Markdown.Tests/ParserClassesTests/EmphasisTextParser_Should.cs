using FluentAssertions;
using Markdown.ParserClasses;
using Markdown.ParserClasses.Nodes;
using Markdown.TokenizerClasses;
using NUnit.Framework;

namespace Markdown.Tests.ParserClassesTests
{
    public class EmphasisTextParser_Should
    {
        private readonly Tokenizer tokenizer = new Tokenizer();
        private readonly Parser parser = new Parser();

        [Test]
        public void Parse_TextSurroundedByUnderscores_Successfully()
        {
            var text = "_what am i, chopped liver?_";
            var tokens = tokenizer.Tokenize(text);
            var expected = new TextNode(TextType.Emphasis);
            expected.Add(new WordNode(WordType.SimpleWord, "what"));
            expected.Add(new WordNode(WordType.SpacedWord, " am"));
            expected.Add(new WordNode(WordType.SpacedWord, " i,"));
            expected.Add(new WordNode(WordType.SpacedWord, " chopped"));
            expected.Add(new WordNode(WordType.SpacedWord, " liver?"));

            parser.EmphasisTextParser(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_DoubleUnderscoresInside_AsText()
        {
            var text = "_easy __as__ pie_";
            var tokens = tokenizer.Tokenize(text);
            var expected = new TextNode(TextType.Emphasis);
            expected.Add(new WordNode(WordType.SimpleWord, "easy"));
            expected.Add(new WordNode(WordType.Space, " "));
            expected.Add(new WordNode(WordType.SimpleWord, "__"));
            expected.Add(new WordNode(WordType.SimpleWord, "as"));
            expected.Add(new WordNode(WordType.SimpleWord, "__"));
            expected.Add(new WordNode(WordType.SpacedWord, " pie"));

            parser.EmphasisTextParser(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_SingleUnderscoreAsText()
        {
            var text = "_";
            var tokens = tokenizer.Tokenize(text);
            var expected = new TextNode();
            expected.Add(new WordNode(WordType.SimpleWord, "_"));

            parser.EmphasisTextParser(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_IfAfterOpenUnderscoreFollowNonWhitespace_AsText()
        {
            var text = "_ science";
            var tokens = tokenizer.Tokenize(text);
            var expected = new TextNode();
            expected.Add(new WordNode(WordType.SimpleWord, "_"));
            expected.Add(new WordNode(WordType.SpacedWord, " science"));

            parser.EmphasisTextParser(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_IfBeforeCloseUnderscoreNonWhitespace_AsText()
        {
            var text = "_norm _";
            var tokens = tokenizer.Tokenize(text);
            var expected = new TextNode();
            expected.Add(new WordNode(WordType.SimpleWord, "_"));
            expected.Add(new WordNode(WordType.SimpleWord, "norm"));
            expected.Add(new WordNode(WordType.Space, " "));

            parser.EmphasisTextParser(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_NotPairedUnderscores_AsText()
        {
            var text = "_treatment__";
            var tokens = tokenizer.Tokenize(text);
            var expected = new TextNode();
            expected.Add(new WordNode(WordType.SimpleWord, "_"));
            expected.Add(new WordNode(WordType.SimpleWord, "treatment"));
            expected.Add(new WordNode(WordType.SimpleWord, "__"));

            parser.EmphasisTextParser(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }
    }
}