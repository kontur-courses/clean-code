using FluentAssertions;
using Markdown;
using Markdown.MdParsing;
using NUnit.Framework;

namespace Markdown_Tests
{
    public class Md_Should
    {
        [TestCaseSource(typeof(TestMdData), nameof(TestMdData.TestTextWithTags))]
        public void RenderText_WithTags_Correctly(string input, string expected)
        {
            var result = Md.Render(input);
            result.Should().Be(expected);
        }
        
        [TestCaseSource(typeof(TestHtmlConverterData), nameof(TestHtmlConverterData.TextNoTags))]
        public void RenderText_NoTags_Returns_Input(string input)
        {
            var result = Md.Render(input);
            result.Should().Be(input);
        }
    }
}
