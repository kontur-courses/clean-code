using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    [TestFixture]
    class EncloserTests
    {
        private Tokenizer tokenizer;
        private Md md;

        [SetUp]
        public void BaseSetup()
        {
            md = new Md();
            tokenizer = new Tokenizer(md.elementSigns);
        }

        [Test]
        public void Tokenize_ShouldThrowArgumentNullException_OnNullText()
        {
            Action act = () => tokenizer.Tokenize(null);
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
