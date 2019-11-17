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
            var input = "_adad_ asd";
            
            var actual = TokenReader.ReadUntil(input, 0);
            
            actual.ConvertToHTMLTag().Should().Be("<em>adad</em>");
        }
        
        [Test]
        public void ReadUntil_ShouldReadUntilAnyStopChar()
        {
            var input = "__abc _defi_ jkl__";
            var excepted = "<strong>abc <em>defi</em> jkl</strong>";
            
            var actual = TokenReader.ReadUntil(input, 0);
                
            actual.ConvertToHTMLTag().Should().Be(excepted);
        }
        
        [Test]
        public void ReadUntil_ShouldReturnString_IfTokenNotClosed()
        {
            var input = "__asd adad_ asS";
            var excepted = "__asd adad_ asS";
            
            var actual = TokenReader.ReadUntil(input, 0);
            
            actual.ConvertToHTMLTag().Should().Be(excepted);
        }
    }
}