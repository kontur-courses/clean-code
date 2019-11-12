using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using ApprovalTests;
using ApprovalTests.Reporters;

namespace Markdown.Tests
{
    [TestFixture]
    public class Test_TokenReader
    {
        [Test]
        public void ReadUntil_ShouldReadUntilStopChar()
        {
            var input = "adad_ asd";
            var token = new Token{StartPosition = 0};
            var deque = new Deque<Tuple<string, Token>>();
            deque.AddLast(Tuple.Create("_", token));

            var actual = TokenReader.ReadUntil(deque, input);
            
            actual.Should().BeEquivalentTo(new Token{StartPosition = 0, Length = 4, Tag = "em", ActualEnd = 4});
        }
        
        [Test]
        public void ReadUntil_ShouldReadUntilAnyStopChar()
        {
            var input = "__asd _adad_ asS__";
            var token = new Token{StartPosition = 2, ActualStart = 0};
            var deque = new Deque<Tuple<string, Token>>();
            deque.AddLast(Tuple.Create("__", token));

            var actual = TokenReader.ReadUntil(deque, input);
            var excepted = "<strong>asd <em>adad</em> asS</strong>";
            
            actual.ConvertToHTMLTag(input).Should().Be(excepted);
        }
        
        [Test]
        public void ReadUntil_ShouldReturnString_IfTokenNotClosed()
        {
            var input = "asd adad_ asS";
            var token = new Token{StartPosition = 0, ActualStart = 0};
            var deque = new Deque<Tuple<string, Token>>();
            deque.AddLast(Tuple.Create("__", token));

            var actual = TokenReader.ReadUntil(deque, input);
            var excepted = "asd adad_ asS";
            
            actual.ConvertToHTMLTag(input).Should().Be(excepted);
        }

        [Test]
        public void StandardiseToken_ShouldReturnToken_IfTokenIsAlreadyStandard()
        {
            var token = new Token {StartPosition = 0, Length = 2, Tag = "as"};
            var excepted = new Token {StartPosition = 0, Length = 2, Tag = "as"};
            
            TokenReader.StandardiseToken(token, new HashSet<string>());
            
            token.Should().BeEquivalentTo(excepted);
        }
        
        [Test]
        public void StandardiseToken_ShouldTrowAwayLowPermissionToken_IfTokenContainExtraToken()
        {
            var token = new Token {StartPosition = 0, Length = 5, Tag = "em"};
            token.StringBlocks.AddLast(Tuple.Create(0, 2));
            token.Value.AddLast(new Token{StartPosition = 2, Length = 3, Tag = "strong"});
              
            TokenReader.StandardiseToken(token, ControlSymbols.TagCloseNextTag["em"]);

            var excepted = new Token {StartPosition = 0, Length = 5, Tag = "em"};
            excepted.StringBlocks.AddLast(Tuple.Create(0, 5));
            token.Should().BeEquivalentTo(excepted);
        }

        [Test]
        public void ClearUnClosedToken_ShouldReturnAllLine_IfNotHaveInnerToken()
        {
            var token = new Token {StartPosition = 0, ActualStart = 0};

            var actual = TokenReader.ClearUnClosedToken(token, 4);
            
            actual.Should().BeEquivalentTo(new Token{StartPosition = 0, ActualStart = 0, Length = 4});
        }
        
        [Test]
        public void ClearUnClosedToken_ShouldReturnAllLine_IfHaveInnerTokenWithoutTag()
        {
            var input = "asdf";
            var token = new Token {StartPosition = 0, ActualStart = 0};
            token.StringBlocks.AddLast(Tuple.Create(0, 1));
            token.Value.AddLast(new Token{StartPosition = 1, ActualStart = 1});

            var actual = TokenReader.ClearUnClosedToken(token, 4);
            
            actual.ConvertToHTMLTag(input).Should().Be(input);
        }
        
        [Test]
        public void ClearUnClosedToken_ShouldReturnLineWithTag_IfHaveInnerTokenWithTag()
        {
            var input = "asdfff";
            var token = new Token {StartPosition = 0, ActualStart = 0};
            token.StringBlocks.AddLast(Tuple.Create(0, 1));
            token.Value.AddLast(new Token{StartPosition = 1, ActualStart = 1, Tag = "em", Length = 2, ActualEnd = 2});

            var actual = TokenReader.ClearUnClosedToken(token, 6);
            
            actual.ConvertToHTMLTag(input).Should().Be("a<em>sd</em>fff");
        }
    }
}