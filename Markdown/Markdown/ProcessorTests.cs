using System;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
    [TestFixture]
    internal class ProcessorTests
    {
        private Processor processor;

        [OneTimeSetUp]
        public void SetUp()
        {
            processor = new Processor(Syntax.InitializeDefaultSyntax());
        }

        [Test]
        public void Render_ThrowsArgumentException_IfInputIsNull()
        {
            Action act = () => processor.Render(null);
            act.Should().Throw<ArgumentException>();
        }

        [TestCase("","", TestName = "Empty string")]
        [TestCase("It Was A Good Day!", "It Was A Good Day!", TestName = "Non-empty string without attributes")]
        [TestCase(@"Hello, \World", @"Hello, \World", TestName = "Escape character with non-special character is shown")]
        [TestCase(@"Hi\! How are you\?", "Hi! How are you?", TestName = "Escape character with special character is not shown")]
        [TestCase(@"My nick is \\prokiller\\", @"My nick is \prokiller\", TestName = "Escape character is escaped")]
        [TestCase(@"\\\Foo\\\\Bar", @"\\Foo\\Bar", TestName = "Many escape characters in a row")]
        public void Render_ReturnsCorrectString_When(string input, string expected)
        {
            processor.Render(input).Should().Be(expected);
        }
    }
}
