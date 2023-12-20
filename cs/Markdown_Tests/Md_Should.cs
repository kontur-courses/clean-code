using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace Markdown_Tests
{
    public class Md_Should
    {
        [TestCaseSource(typeof(TestMdData), nameof(TestMdData.ItalicText))]
        public void RenderItalicText_Correctly(string input, string expected)
        {
            var result = Md.Render(input);
            result.Should().Be(expected);
        }
        
        [TestCaseSource(typeof(TestMdData), nameof(TestMdData.BoldText))]
        public void RenderBoldText_Correctly(string input, string expected)
        {
            var result = Md.Render(input);
            result.Should().Be(expected);
        }

        [Test]
        public void Header()
        {
            var str = "__З__";
            var expected = "_abob2a";
            var res = Md.GetTokens(str).EscapeTags().EscapeInvalidTokens().EscapeNonPairTokens();
            ;
        }
    }
}
