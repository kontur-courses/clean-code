using FluentAssertions;
using Markdown.TokenizerClasses;
using Markdown.TokenizerClasses.Scanners;
using NUnit.Framework;

namespace Markdown.Tests.TokenizerClassesTests
{
    [TestFixture]
    class TagScannerTests
    {
        private readonly TagScanner scanner = new TagScanner();

        [TestCase("_", TestName="underscore")]
        [TestCase("\\", TestName="escape backslash")]
        [TestCase("\n", TestName="newline")]
        public void Scan_ValidTag_ShouldScan(string text)
        {
            var expected = new Token(TagScanner.TokenType[text], text);

            scanner.Scan(text).Should().BeEquivalentTo(expected);
        }

        [TestCase(null, TestName = "null")]
        [TestCase("", TestName = "empty string")]
        [TestCase("*", TestName = "asterisk")]
        [TestCase("9", TestName = "number")]
        [TestCase("a", TestName = "alphabetical")]
        public void Scan_InvalidText_ReturnNull(string text)
        {
            scanner.Scan(text).Should().BeNull();
        }
    }
}
