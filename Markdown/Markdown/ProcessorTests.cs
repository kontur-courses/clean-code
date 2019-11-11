using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Markdown
{

    [TestFixture]
    class ProcessorTests
    {
        private Processor processor;

        [OneTimeSetUp]
        public void SetUp()
        {
            processor = new Processor();
        }
    }
}
