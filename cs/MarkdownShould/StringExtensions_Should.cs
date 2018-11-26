using Markdown;
using NUnit.Framework;

namespace MarkdownShould
{
    [TestFixture]
    public class StringExtensions_Should
    {
        private Md parser;

        [SetUp]
        public void SetUp()
        {
            parser = new Md();
        }
    }
}
