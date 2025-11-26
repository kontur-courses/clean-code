using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests
{
    [TestFixture]
    public class ParserTests
    {
        private Parser parser;


        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            parser = new Parser();
        }





        [Test]
        public void ParseTokensToTree_MultipleListItems_ReturnsListNodeWithMultipleItems()
        {
            var tokens = new List<Token>
            {
                new Token(TokenType.ListItem, "-", 0),
                new Token(TokenType.Space, " ", 1),
                new Token(TokenType.Text, "первый", 2),
                new Token(TokenType.NextLine, "\n", 8),
                new Token(TokenType.ListItem, "-", 9),
                new Token(TokenType.Space, " ", 10),
                new Token(TokenType.Text, "второй", 11),

            };

            var result = parser.ParseTokensToTree(tokens);

            result.Children.Should().HaveCount(1);
            result.Children[0].Should().BeOfType<ListNode>();
            var list = (ListNode)result.Children[0];
            list.Children.Should().HaveCount(2);
            
            ((ListItemNode)list.Children[0]).Children.Should().HaveCount(1);
            ((ListItemNode)list.Children[1]).Children.Should().HaveCount(1);

        }
        [Test]
        public void ParseTokensToTree_StrongWithValidContent_ParsesCorrectly()
        {
            var tokens = new List<Token>
            {
                new Token(TokenType.Strong, "__", 0),
                new Token(TokenType.Text, "жирный", 2),
                new Token(TokenType.Strong, "__", 8)
            };

            var result = parser.ParseTokensToTree(tokens);

            result.Children.Should().HaveCount(1);
            result.Children[0].Should().BeOfType<StrongNode>();
            var strong = (StrongNode)result.Children[0];
            strong.Children.Should().HaveCount(1);
            ((TextNode)strong.Children[0]).Text.Should().Be("жирный");
        }
        


        [Test]
        public void ParseTokensToTree_EmphasisWithValidContent_ParsesCorrectly()
        {
            var tokens = new List<Token>
            {
                new Token(TokenType.Emphasis, "_", 0),
                new Token(TokenType.Text, "курсив", 1),
                new Token(TokenType.Emphasis, "_", 7)
            };

            var result = parser.ParseTokensToTree(tokens);

            result.Children.Should().HaveCount(1);
            result.Children[0].Should().BeOfType<EmphasisNode>();
            var emphasis = (EmphasisNode)result.Children[0];
            emphasis.Children.Should().HaveCount(1);
            ((TextNode)emphasis.Children[0]).Text.Should().Be("курсив");
        }

        [Test]
        public void ParseTokensToTree_HeaderWithContent_ParsesCorrectly()
        {
            var tokens = new List<Token>
            {
                new Token(TokenType.Header, "#", 0),
                new Token(TokenType.Space, " ", 1),
                new Token(TokenType.Text, "Заголовок", 2),
                new Token(TokenType.Space, " ", 11),
                new Token(TokenType.Strong, "__", 12),
                new Token(TokenType.Text, "жирный", 14),
                new Token(TokenType.Strong, "__", 20)
            };

            var result = parser.ParseTokensToTree(tokens);

            result.Children.Should().HaveCount(1);
            result.Children[0].Should().BeOfType<HeaderNode>();
            var header = (HeaderNode)result.Children[0];
            header.Children.Should().HaveCount(3);
            
            ((TextNode)header.Children[0]).Text.Should().Be("Заголовок");
            ((TextNode)header.Children[1]).Text.Should().Be(" ");
            header.Children[2].Should().BeOfType<StrongNode>();
        }

        

        

        [Test]
        public void ParseTokensToTree_NestedEmphasisInStrong_ParsesCorrectly()
        {
            var tokens = new List<Token>
            {
                new Token(TokenType.Strong, "__", 0),
                new Token(TokenType.Text, "жирный ", 2),
                new Token(TokenType.Emphasis, "_", 9),
                new Token(TokenType.Text, "курсив", 10),
                new Token(TokenType.Emphasis, "_", 16),
                new Token(TokenType.Strong, "__", 17)
            };

            var result = parser.ParseTokensToTree(tokens);

            result.Children.Should().HaveCount(1);
            result.Children[0].Should().BeOfType<StrongNode>();
            var strong = (StrongNode)result.Children[0];
            strong.Children.Should().HaveCount(2);
            
            ((TextNode)strong.Children[0]).Text.Should().Be("жирный ");
            strong.Children[1].Should().BeOfType<EmphasisNode>();
            
            var emphasis = (EmphasisNode)strong.Children[1];
            emphasis.Children.Should().HaveCount(1);
            ((TextNode)emphasis.Children[0]).Text.Should().Be("курсив");
        }

        [Test]
        public void ParseTokensToTree_MixedContentWithHeaderAndList_ParsesCorrectly()
        {
            var tokens = new List<Token>
            {
                new Token(TokenType.Header, "#", 0),
                new Token(TokenType.Space, " ", 1),
                new Token(TokenType.Text, "Заголовок", 2),
                new Token(TokenType.NextLine, "\n", 11),
                new Token(TokenType.ListItem, "-", 12),
                new Token(TokenType.Space, " ", 13),
                new Token(TokenType.Text, "пункт", 14),
            };

            var result = parser.ParseTokensToTree(tokens);
            
            result.Children.Should().HaveCount(3);
            result.Children[0].Should().BeOfType<HeaderNode>();
            result.Children[1].Should().BeOfType<NextLineNode>();
            result.Children[2].Should().BeOfType<ListNode>();
            
            var header = (HeaderNode)result.Children[0];
            header.Children.Should().HaveCount(1);
            ((TextNode)header.Children[0]).Text.Should().Be("Заголовок");

            var list = (ListNode)result.Children[2];
            list.Children.Should().HaveCount(1);
            var listItem = (ListItemNode)list.Children[0];
            listItem.Children.Should().HaveCount(1);
            ((TextNode)listItem.Children[0]).Text.Should().Be("пункт");
        }

        [Test]
        public void ParseTokensToTree_InvalidListItemWithoutSpace_ReturnsTextNode()
        {
            var tokens = new List<Token>
            {
                new Token(TokenType.ListItem, "-", 0),
                new Token(TokenType.Text, "пункт", 1)
            };

            var result = parser.ParseTokensToTree(tokens);

            result.Children.Should().HaveCount(2);
            result.Children[0].Should().BeOfType<TextNode>();
            result.Children[1].Should().BeOfType<TextNode>();
            ((TextNode)result.Children[0]).Text.Should().Be("-");
            ((TextNode)result.Children[1]).Text.Should().Be("пункт");
        }

        [Test]
        public void ParseTokensToTree_InvalidHeaderWithoutSpace_ReturnsTextNodes()
        {
            var tokens = new List<Token>
            {
                new Token(TokenType.Header, "#", 0),
                new Token(TokenType.Text, "Заголовок", 1)
            };

            var result = parser.ParseTokensToTree(tokens);

            result.Children.Should().HaveCount(2);
            result.Children[0].Should().BeOfType<TextNode>();
            ((TextNode)result.Children[0]).Text.Should().Be("#");
            ((TextNode)result.Children[1]).Text.Should().Be("Заголовок");
        }

        [Test]
        public void ParseTokensToTree_UnclosedEmphasis_ReturnsTextNode()
        {
            var tokens = new List<Token>
            {
                new Token(TokenType.Emphasis, "_", 0),
                new Token(TokenType.Text, "незакрытый текст", 1)
            };

            var result = parser.ParseTokensToTree(tokens);

            result.Children.Should().HaveCount(2);
            result.Children[0].Should().BeOfType<TextNode>();
            ((TextNode)result.Children[0]).Text.Should().Be("_");
            ((TextNode)result.Children[1]).Text.Should().Be("незакрытый текст");
        }
        

    }
}