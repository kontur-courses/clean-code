using FluentAssertions;
using NUnit.Framework;
using System;

namespace Markdown
{
    public class MarkdownTests
    {
        private static readonly MarkdownToHtmlParser parser = new MarkdownToHtmlParser();

        [Test]
        public void Shoud_ShieldUnderscores()
        {
            var markdownText = "word \\_italic\\_";
            var expectedHtmlText = "word _italic_";

            var actualHtmlText = parser.Render(markdownText);

            actualHtmlText.Should().Be(expectedHtmlText);
        }

        [TestCase(null, TestName = "{m}_WhenInputIsNull")]
        [TestCase("", TestName = "{m}_WhenInputIsEmpty")]
        public void Should_ThrowArgumentException(string input)
        {
            var action = new Action(() => parser.Render(input));

            action.Should().Throw<ArgumentException>();
        }
    }
}