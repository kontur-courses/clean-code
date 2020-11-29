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
            var mark = new StrongMark();
            var expectedToken = new TokenMd("\\<strong>12345\\</strong>", mark);
            var tags = new Dictionary<Mark, IMarkProcessor>();
            var tokens = new List<TokenMd>();
            tokens.Add(new TokenMd("__12345__", mark));
            var processor = new MarkdownProcessor(tags);
            
            var formattedTokens = processor.FormatTokens(tokens);

            formattedTokens.First().Token.Should().BeEquivalentTo(expectedToken.Token);
        }
        
        
        [Test]
        public void FormatToken_ReturnItalicFormattedToken_WhenTokenContainsOneItalicTag()
        {
            var MP = new ItalicProcessor();
            var mark = new ItalicMark();
            var expectedToken = new TokenMd("\\<em>12345\\</em>", mark);
            var tags = new Dictionary<Mark, IMarkProcessor>();
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
    }
}