using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using NUnit.Framework;

namespace Markdown.Tests
{
    class TextParser_Should
    {
        //TODO НЕ ЗАБЫТЬ: ОДИН ТЕСТ - ОДИН КОММИТ!
        [Test]
        public void GetTextTokens_ThrowArgumentException_NullText()
        {
            var textParser = new TextParser();
            Action act = () => textParser.GetTextTokens(null);

            act.Should().Throw<ArgumentException>().WithMessage("string was null");

        }

        [Test]
        public void GetTextTokens_ReturnEmptyList_EmptyText()
        {
            var textParser = new TextParser();
            var expectedLength = 0;

            var textTokens = textParser.GetTextTokens("");
            var actualLength = textTokens.Count;

            actualLength.Should().Be(expectedLength);

        }
    }
}
