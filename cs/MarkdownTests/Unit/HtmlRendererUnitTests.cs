using NUnit.Framework;
using Markdown;
using System.Security.AccessControl;

namespace MarkdownTests.Unit
{
    [TestFixture]
    internal class HtmlRendererUnitTests
    {
        private static HtmlRenderer renderer;

        [SetUp]
        public Setup()
        {
            renderer = new HtmlRenderer();
        }

        [TestCase]
        public string Render_ThrowException_WhenInvalidData(List<Token> tokens)
        {
            
        }
    }
}