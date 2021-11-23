using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class EscapedTextTests
    {
        private EscapedText escapedText;
        private List<int> escapedSymbolsBefore;

        [SetUp]
        public void SetUp()
        {
            escapedSymbolsBefore = new List<int> {1, 2, 3};
            escapedText = new EscapedText("qwe", escapedSymbolsBefore);
        }

        [Test]
        public void Constructor_ThrowsException_IfTextNull() =>
            Assert.That(() => new EscapedText(null, new List<int> {1, 2, 3}), Throws.InstanceOf<ArgumentException>());

        [Test]
        public void Constructor_ThrowsException_IfEscapedSymbolsBeforeIsNull() =>
            Assert.That(() => new EscapedText("qwe", null), Throws.InstanceOf<ArgumentException>());

        [Test]
        public void GetPositionOffset_ReturnsOffset_IfPositionInArrayBound()
        {
            for (var i = 0; i < escapedSymbolsBefore.Count; i++)
                escapedText.GetPositionOffset(i).Should().Be(escapedSymbolsBefore[i]);
        }

        [Test]
        public void GetPositionOffset_ReturnsLastOffset_IfPositionOutArrayBound() =>
            escapedText.GetPositionOffset(1000).Should().Be(escapedSymbolsBefore.Last());
    }
}