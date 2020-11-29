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
            var parser = new MarkdownParser(tags);

            var tokens = parser.GetTokens("__aaaaa__");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        [Test]
        public void GetToken_ReturnWordToken_WhenWordContainsLinesAndDigits()
        {
            var expectedToken = new TokenMd("aaaa_1_2", new EmptyMark());
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser(tags);

            var tokens = parser.GetTokens("aaaa_1_2");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }   
        
        [Test]
        public void GetToken_ReturnWordTokenWithInners_WhenWordContainsTag2()
        {
            var mark = new ItalicMark();
            var expectedToken = new TokenMd("aaaa_b_", new EmptyMark());
            expectedToken.InnerTokens.Add(new TokenMd("aaaa",new EmptyMark(),expectedToken));
            expectedToken.InnerTokens.Add(new TokenMd("_b_",mark,expectedToken));
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser(tags);

            var tokens = parser.GetTokens("aaaa_b_");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        } 
        
        [Test]
        public void GetToken_ReturnWordTokenWithInners_WhenWordContainsTag()
        {
            var mark = new ItalicMark();
            var expectedToken = new TokenMd("aaa_b_c", new EmptyMark());
            expectedToken.InnerTokens.Add(new TokenMd("aaa",new EmptyMark(),expectedToken));
            expectedToken.InnerTokens.Add(new TokenMd("_b_",mark,expectedToken));
            expectedToken.InnerTokens.Add(new TokenMd("c",new EmptyMark(),expectedToken));
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser(tags);

            var tokens = parser.GetTokens("aaa_b_c");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        [Test]
        public void GetToken_ReturnWordToken_WhenTextContainsNotPairedTags()
        {
            var expectedToken = new TokenMd("__aaaaa_", new EmptyMark());
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser(tags);

            var tokens = parser.GetTokens("__aaaaa_");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        [Test]
        public void GetToken_ReturnItalicToken_WhenTextContainsOneItalicTag()
        {
            var mark = new ItalicMark();
            var expectedToken = new TokenMd("_aaaaa_", mark);
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser(tags);

            var tokens = parser.GetTokens("_aaaaa_");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        [Test]
        public void GetToken_ReturnWordToken_WhenNotWhiteSpaceAfterTag()
        {
            var expectedToken = new TokenMd("_aaaaa_a", new EmptyMark());
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser(tags);

            var tokens = parser.GetTokens("_aaaaa_a");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
  
        [Test]
        public void GetToken_ReturnLinkToken_WhenTextContainsOneLinkTag()
        {
            var mark = new LinkMark();
            var expectedToken = new TokenMd("#aaaaa", mark);
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser(tags);

            var tokens = parser.GetTokens("#aaaaa");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        [Test]
        public void GetToken_ReturnHeadToken_WhenTextContainsOneHeadTag()
        {
            var mark = new HeadMark();
            var expectedToken = new TokenMd("#aaaaa", mark);
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser(tags);

            var tokens = parser.GetTokens("#aaaaa");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        [Test]
        public void GetToken_ReturnTokenWithoutMark_WhenTextContainsEscapingAndMarkSymbols()
        {
            var mark = new HeadMark();
            var expectedToken = new TokenMd("aaaaa", mark);
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser(tags);

            var tokens = parser.GetTokens("\\#aaaaa");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        [Test]
        public void GetToken_ReturnTokenWithSlash_WhenTextContainsEscapingSymbol()
        {
            var mark = new HeadMark();
            var expectedToken = new TokenMd("\\aaaaa", mark);
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser(tags);

            var tokens = parser.GetTokens("\\aaaaa");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        [Test]
        public void GetToken_ReturnTokenWithSlash_WhenTextContainsEscapingSymbolInMidle()
        {
            var mark = new HeadMark();
            var expectedToken = new TokenMd("aa\\aaa", mark);
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser(tags);

            var tokens = parser.GetTokens("aa\\aaa");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        [Test]
        public void GetToken_ReturnTokenWithMark_WhenTextContainsDoubleEscapingSymbol()
        {
            var HeadMark = new HeadMark();
            var expectedToken = new TokenMd("#aaaaa", HeadMark);
            var tags = new Dictionary<Mark, IMarkParser>();
            var parser = new MarkdownParser(tags);

            var tokens = parser.GetTokens("\\\\#aaaaa");

            tokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
    }
}