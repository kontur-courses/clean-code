using System;
using NUnit.Framework;
using FluentAssertions;
using Markdown.Shell;

namespace Markdown.Tests
{
    [TestFixture]
    public class DoubleUnderline_Should
    {
        private DoubleUnderline doubleUnderline;

        [SetUp]
        public void SetUp()
        {
            doubleUnderline = new DoubleUnderline();
        }



        [Test]
        public void ContainsSingleUnderline()
        {
            doubleUnderline.Contains(new SingleUnderline()).Should().BeTrue();
        }

    }
}
