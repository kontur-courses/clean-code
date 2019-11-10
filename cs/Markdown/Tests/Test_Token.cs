using System;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests
{
    [TestFixture]
    public class Test_Token
    {
        [Test]
        public void ConvertTokenToHTML_ShouldReturnSimpleString_OnSimplestInput()
        {
            var input = "asda";
            var token = new Token {StartPosition = 0, Length = 4, Tag = ""};

            var actual = token.ConvertToHTMLTag(input);

            actual.Should().Be(input);
        }
        
        [Test]
        public void ConvertTokenToHTML_ShouldReturnStringInTag_OnInputWithTag()
        {
            var input = "asda";
            var token = new Token {StartPosition = 0, Length = 4, Tag = "em"};
            var excepted = "<em>asda</em>";
            var actual = token.ConvertToHTMLTag(input);

            actual.Should().Be(excepted);
        }
        
        [Test]
        public void ConvertTokenToHTML_ShouldReturnStringWithDoubleTag_IfTokenInMiddleOfToken()
        {
            var input = "asda";
            var token = new Token {StartPosition = 0, Length = 4, Tag = "all"};
            token.StringBlocks.Enqueue(new Tuple<int, int>(0, 1));
            token.Value.Enqueue(new Token{StartPosition = 1, Length = 2, Tag = "inside"});
            token.StringBlocks.Enqueue(new Tuple<int, int>(3, 1));
            var excepted = "<all>a<inside>sd</inside>a</all>";
            
            var actual = token.ConvertToHTMLTag(input);

            actual.Should().Be(excepted);
        }
        
        [Test]
        public void ConvertTokenToHTML_ShouldReturnStringWithDoubleTag_IfTokenInRightCornerOfToken()
        {
            var input = "asda";
            var token = new Token {StartPosition = 0, Length = 4, Tag = "all"};
            token.StringBlocks.Enqueue(new Tuple<int, int>(0, 2));
            token.Value.Enqueue(new Token{StartPosition = 2, Length = 2, Tag = "inside"});
            var excepted = "<all>as<inside>da</inside></all>";
            
            var actual = token.ConvertToHTMLTag(input);

            actual.Should().Be(excepted);
        }
        
        [Test]
        public void ConvertTokenToHTML_ShouldReturnStringWithDoubleTag_IfTokenInLeftCornerOfToken()
        {
            var input = "asda";
            var token = new Token {StartPosition = 0, Length = 4, Tag = "all"};
            token.StringBlocks.Enqueue(new Tuple<int, int>(0, 0));
            token.Value.Enqueue(new Token{StartPosition = 0, Length = 2, Tag = "inside"});
            token.StringBlocks.Enqueue(new Tuple<int, int>(2, 2));
            var excepted = "<all><inside>as</inside>da</all>";
            
            var actual = token.ConvertToHTMLTag(input);

            actual.Should().Be(excepted);
        }
        
        [Test]
        public void ConvertTokenToHTML_ShouldReturnStringWithDoubleTag_IfTokenIsWholeToken()
        {
            var input = "asda";
            var token = new Token {StartPosition = 0, Length = 4, Tag = "all"};
            token.StringBlocks.Enqueue(new Tuple<int, int>(0, 0));
            token.Value.Enqueue(new Token{StartPosition = 0, Length = 4, Tag = "inside"});
            var excepted = "<all><inside>asda</inside></all>";
            
            var actual = token.ConvertToHTMLTag(input);

            actual.Should().Be(excepted);
        }
        
        [Test]
        public void ConvertTokenToHTML_ShouldReturnStringSquenceTag_IfTokensInToken()
        {
            var input = "asda";
            var token = new Token {StartPosition = 0, Length = 4, Tag = "all"};
            token.StringBlocks.Enqueue(new Tuple<int, int>(0, 0));
            token.Value.Enqueue(new Token{StartPosition = 0, Length = 1, Tag = "1"});
            token.StringBlocks.Enqueue(new Tuple<int, int>(1, 0));
            token.Value.Enqueue(new Token{StartPosition = 1, Length = 1, Tag = "2"});
            token.StringBlocks.Enqueue(new Tuple<int, int>(2, 1));
            token.Value.Enqueue(new Token{StartPosition = 3, Length = 1, Tag = "3"});
            var excepted = "<all><1>a</1><2>s</2>d<3>a</3></all>";
            
            var actual = token.ConvertToHTMLTag(input);

            actual.Should().Be(excepted);
        }
        
        [Test]
        public void ConvertTokenToHTML_ShouldReturnRightAnswer_IfTokenVeryDifficulte()
        {
            var input = "abcdefg";
            var token = new Token {StartPosition = 0, Length = 7, Tag = "all"};
            var firstToken = new Token {StartPosition = 0, Length = 4, Tag = "1"};
            var innerTokenOfFirst = new Token {StartPosition = 1, Length = 3, Tag = "11"};
            var secondToken = new Token {StartPosition = 4, Length = 3, Tag = "2"};
            var firstInnerTokenOfSecondToken = new Token {StartPosition = 4, Length = 1, Tag = "21"};
            var secondInnerTokenOfSecondToken = new Token {StartPosition = 5, Length = 2, Tag = "22"};
            
            firstToken.StringBlocks.Enqueue(new Tuple<int, int>(0,1));
            firstToken.Value.Enqueue(innerTokenOfFirst);

            secondToken.StringBlocks.Enqueue(new Tuple<int, int>(4, 0));
            secondToken.Value.Enqueue(firstInnerTokenOfSecondToken);
            secondToken.StringBlocks.Enqueue(new Tuple<int, int>(5, 0));
            secondToken.Value.Enqueue(secondInnerTokenOfSecondToken);
            
            token.StringBlocks.Enqueue(new Tuple<int, int>(0, 0));
            token.Value.Enqueue(firstToken);
            token.StringBlocks.Enqueue(new Tuple<int, int>(4, 0));
            token.Value.Enqueue(secondToken);
            
            var excepted = "<all><1>a<11>bcd</11></1><2><21>e</21><22>fg</22></2></all>";
            
            var actual = token.ConvertToHTMLTag(input);

            actual.Should().Be(excepted);
        }
    }
}