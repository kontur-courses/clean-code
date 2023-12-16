using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace MarkdownTask
{
    [TestFixture]
    public class MarkdownTests
    {
        [TestCase("Simple text", "Simple text")]
        [TestCase("_Italic text_", "<em>Italic text</em>")]
        [TestCase("__Halfbold text__", "<strong>Halfbold text</strong>")]
        [TestCase("# This is a header", "<h1>This is a header</h1>")]
        [TestCase("", "")]
        [TestCase("# Header __with _different_ tags__", "<h1> Header <strong>with <em>different</em> tags</strong></h1>")]

        public void NumberValidator_CorrectValue_NotThrowException(string markdownString, string htmlString)
        {
            var md = new Markdown();

            md.Render(markdownString).Should().Be(htmlString);
        }

    }
}
