using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace MarkdownTests
{
    [TestFixture]
    class MarkupExtensionsTests
    {
        [TestCase("f _d", 2, ExpectedResult = true, TestName = "After whitespace")]
        [TestCase("_d", 0, ExpectedResult = true, TestName = "In paragraph beginning")]
        [TestCase("f_d", 1, ExpectedResult = false, TestName = "Without whitespace before")]
        public bool ValidOpeningPositionTest(string text, int openingPosition)
        {
            var markup = new Markup("singleUnderscore", "_", "em");

            return markup.ValidOpeningPosition(text, openingPosition);
        }

        [TestCase("d_ d", 1, ExpectedResult = true, TestName = "Before whitespace")]
        [TestCase("d_", 1, ExpectedResult = true, TestName = "In paragraph ending")]
        [TestCase("d_f", 1, ExpectedResult = false, TestName = "Without whitespace after")]
        public bool ValidClosingPositionTest(string text, int closingPosition)
        {
            var markup = new Markup("singleUnderscore", "_", "em");

            return markup.ValidClosingPosition(text, closingPosition);
        }

        [TestCase("__f",0, ExpectedResult = "doubleUnderscore", TestName = "Find double underscore")]
        [TestCase("_f", 0, ExpectedResult = "simpleUnderscore", TestName = "Find simple underscore")]
        public string GetOpeningMarkupTest(string text, int startIndex)
        {
            var markupsList = new List<Markup>
            {
                new Markup("doubleUnderscore", "__", "strong"),
                new Markup("simpleUnderscore", "_", "em")
            };

            return markupsList.GetOpeningMarkup(text, startIndex).Name;
        }

        [TestCase("f__", 1, ExpectedResult = "doubleUnderscore", TestName = "Find double underscore")]
        [TestCase("f_", 1, ExpectedResult = "simpleUnderscore", TestName = "Find simple underscore")]
        public string GetClosingMarkupTest(string text, int startIndex)
        {
            var markupsList = new List<Markup>
            {
                new Markup("doubleUnderscore", "__", "strong"),
                new Markup("simpleUnderscore", "_", "em")
            };

            return markupsList.GetClosingMarkup(text, startIndex).Name;
        }
    }
}
