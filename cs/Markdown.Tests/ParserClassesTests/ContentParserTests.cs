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
    class ContentParserTests
    {
        public void Parse_OneParagraphText_ShouldParse()
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
            var parser = new ParagraphParser();
            var expected = new ContentNode(new List<ParagraphNode>
            {
                new ParagraphNode(new List<Node>
                {
                    new Node("BOLD", "first", 5),
                    new Node("EMPHASIS", "second", 3),
                    new Node("TEXT", "third", 1)
                }, 9)
            }, 9);

            new ContentParser().Parse(tokens).Should().BeEquivalentTo(expected);
        }
    }
}
