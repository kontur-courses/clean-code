using System.Collections.Generic;
using FluentAssertions;
using Markdown.Lexer;
using NUnit.Framework;

namespace Markdown.Parser
{
    [TestFixture]
    public class MarkdownParser_Should
    {
        private static readonly Lexeme Italic = LexemeDefinitions.Italic;
        private static readonly Lexeme Bold = LexemeDefinitions.Bold;

        [Test]
        public void Parse_BuildsCorrectTree_WithoutTags()
        {
            const string text = "foo bar";
            var tokens = new[] {new Token(0, text, TokenType.Text)};
            var expected = new Text(text);

            TestParsing(tokens, expected);
        }

        [Test]
        public void Parse_BuildCorrectTree_WithItalicTag()
        {
            const string text = "foo";
            var tokens = new[]
            {
                new Token(0, Italic.Representation, TokenType.OpeningTag, Italic),
                new Token(1, text, TokenType.Text),
                new Token(4, Italic.Representation, TokenType.ClosingTag, Italic)
            };
            var expected = new MarkdownItalicElement(Italic.Representation);
            expected.ChildNodes.Add(new Text(text));

            TestParsing(tokens, expected);
        }

        [Test]
        public void Parse_BuildCorrectTree_WithBoldTag()
        {
            const string text = "bar";
            var tokens = new[]
            {
                new Token(0, Bold.Representation, TokenType.OpeningTag, Bold),
                new Token(2, text, TokenType.Text),
                new Token(5, Bold.Representation, TokenType.ClosingTag, Bold)
            };
            var expected = new MarkdownBoldElement(Bold.Representation);
            expected.ChildNodes.Add(new Text(text));

            TestParsing(tokens, expected);
        }

        [Test]
        public void Parse_BuildCorrectTree_WhenItalicIsEmbeddedInBoldTag()
        {
            const string text = "baz";
            var tokens = new[]
            {
                new Token(0, Bold.Representation, TokenType.OpeningTag, Bold),
                new Token(2, Italic.Representation, TokenType.OpeningTag, Italic),
                new Token(3, text, TokenType.Text),
                new Token(6, Italic.Representation, TokenType.ClosingTag, Italic),
                new Token(7, Bold.Representation, TokenType.ClosingTag, Bold)
            };
            var italic = new MarkdownItalicElement(Italic.Representation);
            italic.ChildNodes.Add(new Text(text));
            var expected = new MarkdownBoldElement(Bold.Representation);
            expected.ChildNodes.Add(italic);

            TestParsing(tokens, expected);
        }

        [Test]
        public void Parse_BuildCorrectTree_WhenBoldIsEmbeddedInItalicTag()
        {
            const string text = "baz";
            var tokens = new[]
            {
                new Token(0, Italic.Representation, TokenType.OpeningTag, Italic),
                new Token(1, Bold.Representation, TokenType.OpeningTag, Bold),
                new Token(3, text, TokenType.Text),
                new Token(6, Bold.Representation, TokenType.ClosingTag, Bold),
                new Token(8, Italic.Representation, TokenType.ClosingTag, Italic)
            };
            var expected = new MarkdownItalicElement(Italic.Representation);
            expected.ChildNodes.Add(new Text(Bold.Representation));
            expected.ChildNodes.Add(new Text(text));
            expected.ChildNodes.Add(new Text(Bold.Representation));

            TestParsing(tokens, expected);
        }

        [Test]
        public void Parse_BuildCorrectTree_WhenTagsHaveNoPairs()
        {
            var tokens = new[]
            {
                new Token(0, Bold.Representation, TokenType.OpeningTag, Bold),
                new Token(2, "foo", TokenType.Text),
                new Token(5, Italic.Representation, TokenType.OpeningTag, Italic),
                new Token(6, "bar ", TokenType.Text)
            };
            var expected = new INode[]
            {
                new Text(Bold.Representation), new Text("foo"), new Text(Italic.Representation), new Text("bar ")
            };
            
            TestParsing(tokens, expected);
        }

        [Test]
        public void Parse_BuildCorrectTree_WhenTagsIntersect()
        {
            var tokens = new[]
            {
                new Token(0, Bold.Representation, TokenType.OpeningTag, Bold),
                new Token(2, "foo", TokenType.Text),
                new Token(5, Italic.Representation, TokenType.OpeningTag, Italic),
                new Token(6, "bar", TokenType.Text),
                new Token(9, Bold.Representation, TokenType.ClosingTag, Bold),
                new Token(11, "baz", TokenType.Text),
                new Token(14, Italic.Representation, TokenType.ClosingTag, Italic)
            };
            var boldElement = new MarkdownBoldElement(Bold.Representation);
            boldElement.ChildNodes.Add(new Text("foo"));
            boldElement.ChildNodes.Add(new Text(Italic.Representation));
            boldElement.ChildNodes.Add(new Text("bar"));
            var expected = new INode[] {boldElement, new Text("baz"), new Text(Italic.Representation)};

            TestParsing(tokens, expected);
        }

        private static void TestParsing<T>(IEnumerable<Token> tokens, params T[] nodes) where T : INode
        {
            var root = MarkdownParser.Parse(tokens);

            root.ChildNodes.Should().BeEquivalentTo(nodes);
        }
    }
}