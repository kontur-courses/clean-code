using FluentAssertions;
using Markdown.Shell;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class SingleUnderline_Should
    {
        private SingleUnderline singleUnderline;

        [SetUp]
        public void SetUp()
        {
            singleUnderline = new SingleUnderline();
        }


        [Test]
        public void NotContainsDoubleUnderline()
        {
            singleUnderline.Contains(new DoubleUnderline()).Should().BeFalse();
        }

    }
}
