using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    internal class HtmlCreatorTests
    {
        [TestCase("_abc_", "abc", Styles.Italic, 0, 4, "_", "<em>abc</em>",
            TestName = "AddHtmlTagToText_AddCorrectItalicTag")]
        [TestCase("__abc__", "abc", Styles.Bold, 0, 5, "__", "<strong>abc</strong>",
            TestName = "AddHtmlTagToText_AddCorrectBoldTag")]
        [TestCase("#abc", "abc", Styles.Title, 0, 4, "#", "<h1>abc</h1>",
            TestName = "AddHtmlTagToText_AddCorrectTitleTagWithoutEndMarkdown")]
        public void AddHtmlTagToTextTest(string text, string tokenValue, Styles style, int startPosition,
            int? endPosition, string separator, string expected)
        {
            var actual =
                HtmlCreator.AddHtmlTagToText(text, new Token(tokenValue, style, separator, startPosition, endPosition));
            actual.Should().Be(expected);
        }
    }
}