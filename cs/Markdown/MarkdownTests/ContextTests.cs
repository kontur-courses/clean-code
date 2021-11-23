using System;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class ContextTests
    {
        [Test]
        public void Constructor_ThrowsException_IfTextNull() =>
            Assert.That(() => new Context(null), Throws.InstanceOf<ArgumentException>());

        [TestCase("bc", ExpectedResult = true, TestName = "True if starts with value")]
        [TestCase("a", ExpectedResult = false, TestName = "Don't considers previous symbol")]
        [TestCase("cd", ExpectedResult = false, TestName = "Don't skip current symbol")]
        public bool StartsWith(string value)
            => new Context("abcd", 1).StartsWith(value);

        [TestCase(0, ExpectedResult = true)]
        [TestCase(1, ExpectedResult = false)]
        [TestCase(-1, ExpectedResult = false)]
        public bool IsStringBeginning(int index) =>
            new Context("abcd", index).IsStringBeginning();

        [TestCase(3, ExpectedResult = true, TestName = "True if Index == Length - 1")]
        [TestCase(2, ExpectedResult = false, TestName = "False if Index < Length - 1")]
        [TestCase(10, ExpectedResult = false, TestName = "False if Index >= Length")]
        public bool IsStringEnding(int index) =>
            new Context("abcd", index).IsStringEnding();

        [TestCase(1)]
        [TestCase(0)]
        [TestCase(-1)]
        public void Skip(int value)
        {
            var context = new Context("qw");
            var actual = context.Skip(value);
            actual.Index.Should().Be(context.Index + value);
        }

        [Test]
        public void Skip_ReturnsNewContext()
        {
            var context = new Context("qw");
            var actual = context.Skip(1);
            actual.Should().NotBeSameAs(context);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(32)]
        public void CharAfter_ReturnCorrectChar_IfCharsBeforeCount(int length)
        {
            var line = new string('a', length);
            var symbol = '!';
            var context = new Context(line + symbol + line);

            context.CharAfter(line, c =>
            {
                c.Should().Be(symbol);
                return true;
            });
        }

        [Test]
        public void CharBefore_ReturnCorrectChar()
        {
            var context = new Context("abc", 1);

            context.CharBefore(c =>
            {
                c.Should().Be('a');
                return true;
            });
        }

        [Test]
        public void CurrentSymbol_ThrowsException_IfIndexOutOfRange()
        {
            var context = new Context("abcd").Skip(1000);

            Assert.That(() => context.CurrentSymbol, Throws.InstanceOf<ArgumentOutOfRangeException>());
        }
    }
}