using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class MdTests
    {
        [TestCase("_em_", "<em>em</em>", TestName = "Em tag")]
        [TestCase("__strong__", "<strong>strong</strong>", TestName = "Strong tag")]
        [TestCase("#abc", "<h1>abc</h1>", TestName = "Header tag")]
        [TestCase("\\#\\__escape__", "#__escape__", TestName = "Escape symbols")]
        [TestCase("__bla_bla_bla__", "<strong>bla<em>bla</em>bla</strong>", TestName = "Em inside strong")]
        [TestCase("_bla__bla__bla_", "<em>bla__bla__bla</em>", TestName = "Strong inside em")]
        [TestCase("_123_ __123__", "_123_ __123__", TestName = "Text with nums")]
        [TestCase("_abc_de abc_de fgh_", "<em>abc</em>de abc_de fgh_", TestName = "Part of words inside tags")]
        [TestCase("__abc_abc", "__abc_abc", TestName = "Opened tags")]
        [TestCase("__ tag__ _tag _", "__ tag__ _tag _", TestName = "Spaces after opening tag and before closing tag")]
        [TestCase("__a_b__c_", "__a_b__c_", TestName = "Tags intersect")]
        [TestCase("____ __", "____ __", TestName = "Empty string")]
        [TestCase("No tags", "No tags", TestName = "Text without tags")]
        [TestCase("[bears](http://placebear.com/200/200)", "<a href=\"http://placebear.com/200/200\">bears</a>", TestName = "Link tag")]
        [TestCase("[bears](http://placebear.com/200/200", "[bears](http://placebear.com/200/200", TestName = "Unclosed link tag")]
        public void Render_ShouldReturnExpectedResults(string input, string expectedResult)
        {
            var md = new Md();
            var result = md.Render(input);
            result.Should().Be(expectedResult);
        }


        [Test, MaxTime(1000)]
        public void Render_ShouldHaveLinearTimeOfWork()
        {
            StringBuilder builder = new StringBuilder();
            var md = new Md();
            builder.Append('_');
            for (int i = 0; i < 1000000; i++)
                builder.Append("t");
            builder.Append('_');
            md.Render(builder.ToString());
            Assert.Pass();
        }
    }
}