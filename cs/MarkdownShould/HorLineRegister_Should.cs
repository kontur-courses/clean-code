using Markdown;
using NUnit.Framework;

namespace MarkdownShould
{
    [TestFixture]
    public class HorLineRegister_Should
    {
        private Md parser;

        [SetUp]
        public void SetUp()
        {
            parser = new Md();
        }
    }
}
