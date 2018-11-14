using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown.ParserClasses.Nodes;
using Markdown.ParserClasses.Parsers;
using Markdown.TokenizerClasses;
using NUnit.Framework;

namespace Markdown.Tests.ParseMultiple
{
    [TestFixture]
    class UtilsTests
    {
        [Test]
        public void ParseMultiple_MultipleMixedTextSentences_Parse()
        {
            var tokens = new TokenList(new List<Token>
            {
                new Token("UNDERSCORE", "_"),
                new Token("UNDERSCORE", "_"),
                new Token("TEXT", "first"),
                new Token("UNDERSCORE", "_"),
                new Token("UNDERSCORE", "_"),
              
                new Token("TEXT", "second"),

                new Token("UNDERSCORE", "_"),
                new Token("TEXT", "third"),
                new Token("UNDERSCORE", "_")
            });
            var parser = new SentenceParser();
            var expectedNodes = new List<Node>
            {
                new Node("BOLD", "first", 5),
                new Node("TEXT", "second", 1),
                new Node("EMPHASIS", "third", 3)
            };
            var expectedConsumed = 9;

            var actualNodes = Utils.ParseMultiple(tokens, parser.Parse).Item1;
            var actualConsumed = Utils.ParseMultiple(tokens, parser.Parse).Item2;

            actualNodes.Should().BeEquivalentTo(expectedNodes);
            actualConsumed.Should().BeGreaterOrEqualTo(expectedConsumed);
        }

        [Test]
        public void ParseMultiple_MultiplePlainTextSentences_Parse()
        {
            var tokens = new TokenList(new List<Token>
            {
                new Token("TEXT", "first sentence"),
                new Token("TEXT", "second sentence"),
                new Token("TEXT", "third sentence")
            });
            var parser = new SentenceParser();
            var expectedNodes = new List<Node>
            {
                new Node("TEXT", "first sentence", 1),
                new Node("TEXT", "second sentence", 1),
                new Node("TEXT", "third sentence", 1)
            };
            var expectedConsumed = 3;

            var actualNodes = Utils.ParseMultiple(tokens, parser.Parse).Item1;
            var actualConsumed = Utils.ParseMultiple(tokens, parser.Parse).Item2;

            actualNodes.Should().BeEquivalentTo(expectedNodes);
            actualConsumed.Should().BeGreaterOrEqualTo(expectedConsumed);
        }

        [Test]
        public void ParseMultiple_MultipleBoldTextSentences_Parse()
        {
            var tokens = new TokenList(new List<Token>
            {
                new Token("UNDERSCORE", "_"),
                new Token("UNDERSCORE", "_"),
                new Token("TEXT", "first"),
                new Token("UNDERSCORE", "_"),
                new Token("UNDERSCORE", "_"),
              
                new Token("UNDERSCORE", "_"),
                new Token("UNDERSCORE", "_"),
                new Token("TEXT", "second"),
                new Token("UNDERSCORE", "_"),
                new Token("UNDERSCORE", "_"),

                new Token("UNDERSCORE", "_"),
                new Token("UNDERSCORE", "_"),
                new Token("TEXT", "third"),
                new Token("UNDERSCORE", "_"),
                new Token("UNDERSCORE", "_")
            });
            var parser = new SentenceParser();
            var expectedNodes = new List<Node>
            {
                new Node("BOLD", "first", 5),
                new Node("BOLD", "second", 5),
                new Node("BOLD", "third", 5)
            };
            var expectedConsumed = 15;

            var actualNodes = Utils.ParseMultiple(tokens, parser.Parse).Item1;
            var actualConsumed = Utils.ParseMultiple(tokens, parser.Parse).Item2;

            actualNodes.Should().BeEquivalentTo(expectedNodes);
            actualConsumed.Should().BeGreaterOrEqualTo(expectedConsumed);
        }

        [Test]
        public void ParseMultiple_MultipleEmphasisTextSentences_Parse()
        {
            var tokens = new TokenList(new List<Token>
            {
                new Token("UNDERSCORE", "_"),
                new Token("TEXT", "first"),
                new Token("UNDERSCORE", "_"),
              
                new Token("UNDERSCORE", "_"),
                new Token("TEXT", "second"),
                new Token("UNDERSCORE", "_"),

                new Token("UNDERSCORE", "_"),
                new Token("TEXT", "third"),
                new Token("UNDERSCORE", "_")
            });
            var parser = new SentenceParser();
            var expectedNodes = new List<Node>
            {
                new Node("EMPHASIS", "first", 3),
                new Node("EMPHASIS", "second", 3),
                new Node("EMPHASIS", "third", 3)
            };
            var expectedConsumed = 9;

            var actualNodes = Utils.ParseMultiple(tokens, parser.Parse).Item1;
            var actualConsumed = Utils.ParseMultiple(tokens, parser.Parse).Item2;

            actualNodes.Should().BeEquivalentTo(expectedNodes);
            actualConsumed.Should().BeGreaterOrEqualTo(expectedConsumed);
        }
    }
}
