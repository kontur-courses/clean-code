using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkdownTests
    {
        public Md Markdown;
        [SetUp]
        public void Setup()
        {
            Markdown = new Md();
        }

        [Test]
        public void MarkdownToHtmlConversion_NoStyle()
        {
            const string text = "test";
            
            var actual = Markdown.Render(text);
            
            Assert.That(actual, Is.EqualTo(text));
        }
    }
}