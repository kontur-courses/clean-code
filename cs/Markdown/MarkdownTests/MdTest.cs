using NUnit.Framework;
using FluentAssertions;
using Markdown;


namespace MarkdownTests
{
    
    public class MdTest
    {
        private Md _md;
        [SetUp]
        public void Setup()
        {
            _md = new Md();
        }

        [TestCase("_text_", "<em>text</em>")]
        [TestCase("__text__", "<strong>text</strong>")]
        public void Test1(string text, string exp)
        {
            var result = _md.Render(text);
            result.Should().BeEquivalentTo(exp);
        }
    }   
}