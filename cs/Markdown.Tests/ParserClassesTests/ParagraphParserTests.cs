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

namespace Markdown.Tests.ParserClassesTests
{
    [TestFixture]
    class ParagraphParserTests
    {
        [Test]
        public void Parse_MultipleMixedTextSentences_ShouldParse()
        {
            var tokens = new TokenList(new List<Token>
            {
                new Token("UNDERSCORE", "_"),
                new Token("UNDERSCORE", "_"),
                new Token("TEXT", "first"),
                new Token("UNDERSCORE", "_"),
                new Token("UNDERSCORE", "_"),
              
                new Token("UNDERSCORE", "_"),
                new Token("TEXT", "second"),
                new Token("UNDERSCORE", "_"),

                new Token("TEXT", "third")
            });
            var parser = new SentenceParser();
            var expected = new ParagraphNode(new List<Node>
            {
                new Node("BOLD", "first", 5),
                new Node("EMPHASIS", "second", 3),
                new Node("TEXT", "third", 1)
            }, 9);

            new ParagraphParser().Parse(tokens).Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_MultiplePlainTextSentences_ShouldParse()
        {
            var tokens = new TokenList(new List<Token>
            {
                new Token("TEXT", "first"),
              
                new Token("TEXT", "second"),

                new Token("TEXT", "third"),
            });
            var parser = new SentenceParser();
            var expected = new ParagraphNode(new List<Node>
            {
                new Node("TEXT", "first", 1),
                new Node("TEXT", "second", 1),
                new Node("TEXT", "third", 1)
            }, 3);

            new ParagraphParser().Parse(tokens).Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_MultipleBoldTextSentences_ShouldParse()
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
            var expected = new ParagraphNode(new List<Node>
            {
                new Node("BOLD", "first", 5),
                new Node("BOLD", "second", 5),
                new Node("BOLD", "third", 5)
            }, 15);

            new ParagraphParser().Parse(tokens).Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_MultipleEmphasisTextSentences_ShouldParse()
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
            var expected = new ParagraphNode(new List<Node>
            {
                new Node("EMPHASIS", "first", 3),
                new Node("EMPHASIS", "second", 3),
                new Node("EMPHASIS", "third", 3)
            }, 9);

            new ParagraphParser().Parse(tokens).Should().BeEquivalentTo(expected);
        }
    }
}
