using System;
using System.Collections.Generic;
using System.Linq;
using Markdown;
using NUnit.Framework;
using FluentAssertions;


namespace MarkdownTests
{
    [TestFixture]
    public class ParserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetToken_ReturnStrongToken_WhenTextContainsOneStrongTag()
        {
            var mark = new StrongMark();
            var expectedToken = new TokenMd("__aaaaa__", mark);
            expectedToken.Token = "__aaaaa__";
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser("__aaaaa__",tags);

            var tokens = parser.GetTokens("__aaaaa__");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        [Test]
        public void GetToken_ReturnWordToken_WhenWordContainsLinesAndDigits()
        {
            var expectedToken = new TokenMd("aaaa_1_2", null);
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser("aaaa_1_2",tags);

            var tokens = parser.GetTokens("aaaa_1_2");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }   
        
        [Test]
        public void GetToken_ReturnWordTokenWithInners_WhenWordContainsTag2()
        {
            var mark = new ItalicMark();
            var expectedToken = new TokenMd("aaaa_b_", null);
            expectedToken.InnerTokens.Add(new TokenMd("aaaa",null,expectedToken));
            expectedToken.InnerTokens.Add(new TokenMd("_b_",mark,expectedToken));
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser("aaaa_b_",tags);

            var tokens = parser.GetTokens("aaaa_b_");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        } 
        
        [Test]
        public void GetToken_ReturnWordTokenWithInners_WhenWordContainsTag()
        {
            var mark = new ItalicMark();
            var expectedToken = new TokenMd("aaa_b_c", null);
            expectedToken.InnerTokens.Add(new TokenMd("aaa",null,expectedToken));
            expectedToken.InnerTokens.Add(new TokenMd("_b_",mark,expectedToken));
            expectedToken.InnerTokens.Add(new TokenMd("c",null,expectedToken));
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser("aaa_b_c",tags);

            var tokens = parser.GetTokens("aaa_b_c");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        [Test]
        public void GetToken_ReturnWordToken_WhenTextContainsNotPairedTags()
        {
            var expectedToken = new TokenMd("__aaaaa_", null);
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser("__aaaaa_",tags);

            var tokens = parser.GetTokens("__aaaaa_");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        [Test]
        public void GetToken_ReturnItalicToken_WhenTextContainsOneItalicTag()
        {
            var mark = new ItalicMark();
            var expectedToken = new TokenMd("_aaaaa_", mark);
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser("_aaaaa_",tags);

            var tokens = parser.GetTokens("_aaaaa_");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        [Test]
        public void GetToken_ReturnWordToken_WhenNotWhiteSpaceAfterTag()
        {
            var expectedToken = new TokenMd("_aaaaa_a", null);
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser("_aaaaa_a",tags);

            var tokens = parser.GetTokens("_aaaaa_a");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        /*
        [Test]
        public void GetToken_ReturnThreeToken_WhenItalicTagAndWord()
        {
            var mark = new ItalicMark();
            var expectedTokens = new List<TokenMd>();
            var tokenItalic = new TokenMd("_bb_", mark);
            var innerToken = new TokenMd("bb", null);
            innerToken.InnerTokens = null;
            tokenItalic.InnerTokens.Add(innerToken);
            expectedTokens.Add(tokenItalic);
            var token2 = new TokenMd(" ", null);
            token2.InnerTokens = null;
            expectedTokens.Add(token2);
            var token3 = new TokenMd("aa", null);
            token3.InnerTokens = null;
            expectedTokens.Add(token3);
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser("_bb_ aa",tags);

            var tokens = parser.GetTokens("_bb_ aa");

            foreach (var token in tokens)
            {
                Console.WriteLine("|-"+token.Token+"-|");
            }

            tokens.Should().BeEquivalentTo(expectedTokens);
        }
        */
        
        [Test]
        public void GetToken_ReturnLinkToken_WhenTextContainsOneLinkTag()
        {
            var mark = new LinkMark();
            var expectedToken = new TokenMd("#aaaaa", mark);
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser("#aaaaa",tags);

            var tokens = parser.GetTokens("#aaaaa");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        [Test]
        public void GetToken_ReturnHeadToken_WhenTextContainsOneHeadTag()
        {
            var mark = new HeadMark();
            var expectedToken = new TokenMd("#aaaaa", mark);
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser("#aaaaa",tags);

            var tokens = parser.GetTokens("#aaaaa");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        [Test]
        public void GetToken_ReturnTokenWithoutMark_WhenTextContainsEscapingAndMarkSymbols()
        {
            var mark = new HeadMark();
            var expectedToken = new TokenMd("aaaaa", mark);
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser("aaaaa", tags);

            var tokens = parser.GetTokens("\\#aaaaa");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        [Test]
        public void GetToken_ReturnTokenWithSlash_WhenTextContainsEscapingSymbol()
        {
            var mark = new HeadMark();
            var expectedToken = new TokenMd("\\aaaaa", mark);
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser("\\aaaaa", tags);

            var tokens = parser.GetTokens("\\aaaaa");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        [Test]
        public void GetToken_ReturnTokenWithSlash_WhenTextContainsEscapingSymbolInMidle()
        {
            var mark = new HeadMark();
            var expectedToken = new TokenMd("aa\\aaa", mark);
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser("aa\\aaa", tags);

            var tokens = parser.GetTokens("aa\\aaa");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        [Test]
        public void GetToken_ReturnTokenWithMark_WhenTextContainsDoubleEscapingSymbol()
        {
            var HeadMark = new HeadMark();
            var expectedToken = new TokenMd("#aaaaa", HeadMark);
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser("#aaaaa", tags);

            var tokens = parser.GetTokens("\\\\#aaaaa");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
    }
}