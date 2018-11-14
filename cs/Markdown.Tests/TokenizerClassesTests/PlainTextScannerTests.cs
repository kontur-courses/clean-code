using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown.TokenizerClasses;
using Markdown.TokenizerClasses.Scanners;
using NUnit.Framework;

namespace Markdown.Tests.TokenizerClassesTests
{
    [TestFixture]
    class PlainTextScannerTests
    {
        private readonly PlainTextScanner scanner = new PlainTextScanner();

        [TestCase("abcde", TestName = "plain text")]
        [TestCase("anoth_", TestName = "breaks by underscore")]
        [TestCase("one m\\", TestName = "breaks by escape-backslash")]
        [TestCase("maybe\n", TestName = "breaks by newline")]
        public void Scan_ValidText_ShouldScan(string text)
        {
            var expected = new Token("TEXT", text.Substring(0, 5));

            scanner.Scan(text).Should().BeEquivalentTo(expected);
        }

        [TestCase("", TestName = "empty string")]
        [TestCase(null, TestName = "null")]
        public void Scan_InvalidText_ReturnNull(string text)
        {
            scanner.Scan(text).Should().BeNull();
        }
    }
}
