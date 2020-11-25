using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown;
using Markdown.Conversion.MarkdownProcessors;
using Markdown.MarkdownProcessors;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class MdProcessorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void FormatToken_ReturnStrongFormattedToken_WhenTokenContainsOneStrongTag()
        {
            var MP = new StrongProcessor();
            var mark = new StrongMark();
            var expectedToken = new TokenMd("\\<strong>12345\\</strong>", mark);
            var tags = new Dictionary<Mark, IMarkProcessor>();
            //tags.Add(mark, MP);
            var tokens = new List<TokenMd>();
            tokens.Add(new TokenMd("__12345__", mark));
            var processor = new MarkdownProcessor(tags);
            
            var formattedTokens = processor.FormatTokens(tokens);

            formattedTokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        /*
        [Test]
        public void FormatToken_ReturnLinkFormattedToken_WhenTokenContainsOneLinkTag()
        {
            //\<h1><a href="bb"> \<strong>aa \<em>b\</em>b\<em> \</em>aa\</strong></a>\</h1>
            var mark = new LinkMark();
            var expectedToken = new TokenMd("<a href=\"bb\">aa</strong></a>", mark);
            var tags = new Dictionary<Mark, IMarkProcessor>();
            //tags.Add(mark, MP);
            var tokens = new List<TokenMd>();
            tokens.Add(new TokenMd("[aa](bb)", mark));
            var processor = new MarkdownProcessor("[aa](bb)",tags);
            
            var formattedTokens = processor.FormatTokens(tokens);

            formattedTokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        */
        
        [Test]
        public void FormatToken_ReturnItalicFormattedToken_WhenTokenContainsOneItalicTag()
        {
            var MP = new ItalicProcessor();
            var mark = new ItalicMark();
            var expectedToken = new TokenMd("\\<em>12345\\</em>", mark);
            var tags = new Dictionary<Mark, IMarkProcessor>();
            //tags.Add(mark, MP);
            var tokens = new List<TokenMd>();
            tokens.Add(new TokenMd("_12345_", mark));
            var processor = new MarkdownProcessor(tags);
            
            var formattedTokens = processor.FormatTokens(tokens);

            formattedTokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }

        [Test]
        public void Equals()
        {
            var headingMark = new HeadMark();
            var otherheadingMark = new HeadMark();

            headingMark.Equals(otherheadingMark).Should().BeTrue();
        }        
        
        [Test]
        public void GetHashC()
        {
            var headingMark = new HeadMark();
            var otherheadingMark = new HeadMark();

            headingMark.GetHashCode().Equals(otherheadingMark.GetHashCode()).Should().BeTrue();
        }
        
        [Test]
        public void FormatToken_ReturnHeadFormattedToken_WhenTokenContainsOneHeadTag()
        {
            var MP = new HeadProcessor();
            var mark = new HeadMark();
            var expectedToken = new TokenMd("\\<h1>12345\\</h1>", mark);
            var tags = new Dictionary<Mark, IMarkProcessor>();
            //tags.Add(mark, MP);
            var tokens = new List<TokenMd>();
            tokens.Add(new TokenMd("#12345", mark));
            var processor = new MarkdownProcessor(tags);
            
            var formattedTokens = processor.FormatTokens(tokens);

            formattedTokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        /*
        [Test]
        public void FormatToken_ReturnTokensWithoutMark_WhenItalicAndStrongIntersect()
        {
            var strongProcessor = new StrongProcessor();
            var strongMark = new StrongMark();
            var italicProcessor = new ItalicProcessor();
            var italicMark = new ItalicMark();
            var headProcessor = new HeadProcessor();
            var headMark = new HeadMark();
            
            var strongExpectedToken = new TokenMd("__123 _45__", null);
            var italicExpectedToken = new TokenMd("67_", null);
            var expectedTokens = new List<TokenMd>{strongExpectedToken,italicExpectedToken};
            
            var Tags = new Dictionary<Mark, IMarkProcessor>();
            //Tags.Add(strongMark, strongProcessor);
            //Tags.Add(italicMark, italicProcessor);
            //Tags.Add(headMark, headProcessor);
            
            var tokens = new List<TokenMd>();
            tokens.Add(new TokenMd("__123 _45__", strongMark));
            tokens.Add(new TokenMd(" ", null));
            tokens.Add(new TokenMd("67_", null));

            var text = "__123 _45__ 67_";
            var processor = new MarkdownProcessor(text,Tags);

            var formattedTokens = processor.FormatTokens(tokens);

            var res = new List<TokenMd>();
            foreach (var e in formattedTokens)
            {
                res.Add(new TokenMd(e.Token, null));
                Console.WriteLine("res=  "+e.Token);
            }
            res.Should().BeEquivalentTo(expectedTokens);
        }
        */
    }
}