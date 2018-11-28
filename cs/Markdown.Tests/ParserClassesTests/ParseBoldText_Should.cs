using FluentAssertions;
using Markdown.ParserClasses;
using Markdown.ParserClasses.Nodes;
using Markdown.TokenizerClasses;
using NUnit.Framework;

namespace Markdown.Tests.ParserClassesTests
{
    public class ParseBoldText_Should
    {
        private readonly Tokenizer tokenizer = new Tokenizer();

        [Test]
        public void Parse_TextSurroundedByDoubleUnderscores_Successfully()
        {
            var text = "__what am i, chopped liver?__";
            var tokens = tokenizer.Tokenize(text);
            var expected = new TextNode(TextType.Bold);
            expected.Add(new WordNode(WordType.SimpleWord, "what"));
            expected.Add(new WordNode(WordType.SpacedWord, " am"));
            expected.Add(new WordNode(WordType.SpacedWord, " i,"));
            expected.Add(new WordNode(WordType.SpacedWord, " chopped"));
            expected.Add(new WordNode(WordType.SpacedWord, " liver?"));

            Parser.ParseBoldText(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_EmphasisTextInside_Successfully()
        {
            var text = "__a chip on _your_ shoulder__";
            var tokens = tokenizer.Tokenize(text);
            var expected = new TextNode(TextType.Bold);
            expected.Add(new WordNode(WordType.SimpleWord, "a"));
            expected.Add(new WordNode(WordType.SpacedWord, " chip"));
            expected.Add(new WordNode(WordType.SpacedWord, " on"));
            expected.Add(new WordNode(WordType.Space, " "));
            expected.Add(new WordNode(WordType.SimpleWord, "<em>"));
            expected.Add(new WordNode(WordType.SimpleWord, "your"));
            expected.Add(new WordNode(WordType.SimpleWord, "</em>"));
            expected.Add(new WordNode(WordType.SpacedWord, " shoulder"));

            Parser.ParseBoldText(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_SingleDoubleUnderscore_AsText()
        {
            var text = "__";
            var tokens = tokenizer.Tokenize(text);
            var expected = new TextNode();
            expected.Add(new WordNode(WordType.SimpleWord, "__"));

            Parser.ParseBoldText(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_IfAfterOpenDoubleUnderscoreFollowNonWhitespace_AsText()
        {
            var text = "__ warn";
            var tokens = tokenizer.Tokenize(text);
            var expected = new TextNode();
            expected.Add(new WordNode(WordType.SimpleWord, "__"));
            expected.Add(new WordNode(WordType.SpacedWord, " warn"));

            Parser.ParseBoldText(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_IfBeforeCloseUnderscoreNonWhitespace_AsText()
        {
            var text = "__insert __";
            var tokens = tokenizer.Tokenize(text);
            var expected = new TextNode();
            expected.Add(new WordNode(WordType.SimpleWord, "__"));
            expected.Add(new WordNode(WordType.SimpleWord, "insert"));
            expected.Add(new WordNode(WordType.Space, " "));

            Parser.ParseBoldText(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_NotPairedUnderscores_AsText()
        {
            var text = "__appeal_";
            var tokens = tokenizer.Tokenize(text);
            var expected = new TextNode();
            expected.Add(new WordNode(WordType.SimpleWord, "__"));
            expected.Add(new WordNode(WordType.SimpleWord, "appeal"));
            expected.Add(new WordNode(WordType.SimpleWord, "_"));

            Parser.ParseBoldText(tokens)
                .Should()
                .BeEquivalentTo(expected);
        }
    }
}