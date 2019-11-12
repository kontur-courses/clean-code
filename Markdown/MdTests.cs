using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
    class MdTests
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [TestCase("abcd", "<p>abcd</p>", TestName = "One simple line")]
        [TestCase("abcd\nabcd", "<p>abcd\nabcd</p>", TestName = "Two lines but one paragraph")]
        [TestCase("abcd\n\nabcd", "<p>abcd</p>\n<p>abcd</p>", TestName = "Two paragraphs")]
        [TestCase("\n\nabcd", "<p>abcd</p>", TestName = "Skip empty paragraphs")]
        public void TranslateRender_ReturnsCorrectAnswer(string inputLine, string outputLine)
        {
            md.Render(inputLine).Should().Be(outputLine);
        }

        [Test]
        public void DoSomething_WhenSomething()
        {
            
        }
    }
}
