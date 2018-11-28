using FluentAssertions;
using Markdown.ParserClasses;
using Markdown.ParserClasses.Nodes;
using Markdown.TokenizerClasses;
using NUnit.Framework;

namespace Markdown.Tests.ParserClassesTests
{
    public class ParseSentence_Should
    {
        private readonly Tokenizer tokenizer = new Tokenizer();
        private readonly Parser parser = new Parser();

        [Test]
        public void Parse_SimpleText_Successfully()
        {
            var text = "hard pill to swallow";
            var tokens = tokenizer.Tokenize(text);
            var textNode = new TextNode();
            textNode.Add(new WordNode(WordType.SimpleWord, "hard"));
            textNode.Add(new WordNode(WordType.SpacedWord, " pill"));
            textNode.Add(new WordNode(WordType.SpacedWord, " to"));
            textNode.Add(new WordNode(WordType.SpacedWord, " swallow"));
            var expected = new SentenceNode();
            expected.Add(textNode);

            parser.ParseSentence(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_SingleTextStartingWithSpace_Successfully()
        {
            var text = " an arm and a leg";
            var tokens = tokenizer.Tokenize(text);
            var textNode = new TextNode();
            textNode.Add(new WordNode(WordType.SpacedWord, " an"));
            textNode.Add(new WordNode(WordType.SpacedWord, " arm"));
            textNode.Add(new WordNode(WordType.SpacedWord, " and"));
            textNode.Add(new WordNode(WordType.SpacedWord, " a"));
            textNode.Add(new WordNode(WordType.SpacedWord, " leg"));
            var expected = new SentenceNode();
            expected.Add(textNode);

            parser.ParseSentence(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_SingleTextEndingWithSpace_Successfully()
        {
            var text = "barking up the wrong tree ";
            var tokens = tokenizer.Tokenize(text);
            var textNode = new TextNode();
            textNode.Add(new WordNode(WordType.SimpleWord, "barking"));
            textNode.Add(new WordNode(WordType.SpacedWord, " up"));
            textNode.Add(new WordNode(WordType.SpacedWord, " the"));
            textNode.Add(new WordNode(WordType.SpacedWord, " wrong"));
            textNode.Add(new WordNode(WordType.SpacedWord, " tree"));
            textNode.Add(new WordNode(WordType.Space, " "));
            var expected = new SentenceNode();
            expected.Add(textNode);

            parser.ParseSentence(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_SingleTextStartingAndEndingWithSpace_Successfully()
        {
            var text = " everything but the kitchen sink ";
            var tokens = tokenizer.Tokenize(text);
            var textNode = new TextNode();
            textNode.Add(new WordNode(WordType.SpacedWord, " everything"));
            textNode.Add(new WordNode(WordType.SpacedWord, " but"));
            textNode.Add(new WordNode(WordType.SpacedWord, " the"));
            textNode.Add(new WordNode(WordType.SpacedWord, " kitchen"));
            textNode.Add(new WordNode(WordType.SpacedWord, " sink"));
            textNode.Add(new WordNode(WordType.Space, " "));
            var expected = new SentenceNode();
            expected.Add(textNode);

            parser.ParseSentence(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_TextAndEmphasis_Successfully()
        {
            var text = "break the ice _go for broke_";
            var tokens = tokenizer.Tokenize(text);

            var textNode = new TextNode();
            textNode.Add(new WordNode(WordType.SimpleWord, "break"));
            textNode.Add(new WordNode(WordType.SpacedWord, " the"));
            textNode.Add(new WordNode(WordType.SpacedWord, " ice"));
            textNode.Add(new WordNode(WordType.Space, " "));

            var emphasisNode = new TextNode(TextType.Emphasis);
            emphasisNode.Add(new WordNode(WordType.SimpleWord, "go"));
            emphasisNode.Add(new WordNode(WordType.SpacedWord, " for"));
            emphasisNode.Add(new WordNode(WordType.SpacedWord, " broke"));

            var expected = new SentenceNode();
            expected.Add(textNode);
            expected.Add(emphasisNode);

            parser.ParseSentence(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_TextEmphasisAndBold_Successfully()
        {
            var text = "_the wrong_ barking up __tree__";
            var tokens = tokenizer.Tokenize(text);

            var textNode = new TextNode();
            textNode.Add(new WordNode(WordType.SpacedWord, " barking"));
            textNode.Add(new WordNode(WordType.SpacedWord, " up"));
            textNode.Add(new WordNode(WordType.Space, " "));

            var emphasisNode = new TextNode(TextType.Emphasis);
            emphasisNode.Add(new WordNode(WordType.SimpleWord, "the"));
            emphasisNode.Add(new WordNode(WordType.SpacedWord, " wrong"));

            var boldNode = new TextNode(TextType.Bold);
            boldNode.Add(new WordNode(WordType.SimpleWord, "tree"));

            var expected = new SentenceNode();
            expected.Add(emphasisNode);
            expected.Add(textNode);
            expected.Add(boldNode);

            parser.ParseSentence(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }
    }
}