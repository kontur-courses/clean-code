using Markdown.Parsers;

namespace Markdown.Tests
{
    public class MdParserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Parse_PassText_DoesNotThrow()
        {
            var text = "hello world";
            var parser = new MdParser();

            Assert.DoesNotThrow(() => parser.Parse(text));
        }
    }
}