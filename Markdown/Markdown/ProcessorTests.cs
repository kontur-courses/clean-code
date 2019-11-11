using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Markdown
{
    [TestFixture]
    internal class ProcessorTests
    {
        private Processor processor;

        [OneTimeSetUp]
        public void SetUp()
        {
            processor = new Processor();
        }
    }
}
