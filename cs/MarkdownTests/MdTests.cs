using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Markdown;

namespace MarkdownTests
{
    public class MdTests
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            var stringTranslator = new MdToTokenTranslator();
            var tokenTranslator = new TokenToHtmlTranslator();
            md = new Md(stringTranslator, tokenTranslator);
        }

        [TestCaseSource(nameof(CasesForHeading))]
        [TestCaseSource(nameof(CasesForUnderscore))]
        [TestCaseSource(nameof(CasesForShielding))]
        [TestCaseSource(nameof(CasesForDoubleUnderscore))]
        [TestCaseSource(nameof(CasesForInteractBetweenUnderscores))]
        [TestCase("No Header", "No Header", TestName = "No tags do not create any tag")]
        public void Render_Should(string markdown, string expected)
        {
            md.Render(markdown).Should().Be(expected);
        }

        private static IEnumerable<TestCaseData> CasesForHeading
        {
            get
            {
                yield return new TestCaseData("#Header", "<h1>Header</h1>")
                    .SetName("Sharp Creates Tag Header");
                yield return new TestCaseData("#Header _with_ underscore",
                        "<h1>Header <em>with</em> underscore</h1>")
                    .SetName("Underscore creates tag in heading");
                yield return new TestCaseData("#Header with __double__",
                        "<h1>Header with <strong>double</strong></h1>")
                    .SetName("Double underscore creates tag in heading");
                yield return new TestCaseData("#Header _with_ __both__ underscores",
                        "<h1>Header <em>with</em> <strong>both</strong> underscores</h1>")
                    .SetName("Header with both underscores create tags after each other");
                yield return new TestCaseData("#Header __works _with_ tag__ inside",
                        "<h1>Header <strong>works <em>with</em> tag</strong> inside</h1>")
                    .SetName("Header with underscores inside create tags");
                yield return new TestCaseData("#abc _test __def__ test_",
                        "<h1>abc _test __def__ test_</h1>")
                    .SetName("Ignore double inside unary underscore");
                yield return new TestCaseData("#abc __test _def__ test_",
                        "<h1>abc __test _def__ test_</h1>")
                    .SetName("Ignore intersections of underscores");
                yield return new TestCaseData("#aa\naa",
                        "<h1>aa</h1>\naa")
                    .SetName("Heading ends on different paragraphs");
            }
        }

        private static IEnumerable<TestCaseData> CasesForUnderscore
        {
            get
            {
                yield return new TestCaseData("_text_", "<em>text</em>")
                    .SetName("Underscore Creates italic tag");
                yield return new TestCaseData("_insi_de",
                        "<em>insi</em>de")
                    .SetName("Underscore creates tag inside word at the beginning");
                yield return new TestCaseData("in_side_",
                        "in<em>side</em>")
                    .SetName("Underscore creates tag inside word at the end");
                yield return new TestCaseData("in_si_de",
                        "in<em>si</em>de")
                    .SetName("Underscore creates tag inside word");
                yield return new TestCaseData("te_xt in_side",
                        "te_xt in_side")
                    .SetName("Underscores don't create tags inside other words");
                yield return new TestCaseData("_test",
                        "_test")
                    .SetName("Underscore doesn't create tag when not closed");
                yield return new TestCaseData("test_ test_",
                        "test_ test_")
                    .SetName("Underscore at end of word not creates tag");
                yield return new TestCaseData("_test _test",
                        "_test _test")
                    .SetName("Underscore at begin of word not close tag");
                yield return new TestCaseData("1_23_",
                    "1_23_")
                    .SetName("Underscore doesn't create tag inside digits");
                yield return new TestCaseData("__", "__")
                    .SetName("Underscore doesn't create tag when empty string");
            }
        }

        private static IEnumerable<TestCaseData> CasesForShielding
        {
            get
            {
                yield return new TestCaseData("\\#Header", "#Header")
                    .SetName("Escaped sharp doesn't create tag header");
                yield return new TestCaseData("\\_test\\_", "_test_")
                    .SetName("Escaped underscore doesn't create tag em");
                yield return new TestCaseData("te\\st", "te\\st")
                    .SetName("Escaped symbol doesn't hide when no service symbol");
                yield return new TestCaseData("\\\\_Header_", "\\<em>Header</em>")
                    .SetName("Escape symbol also escaped");
            }
        }

        private static IEnumerable<TestCaseData> CasesForDoubleUnderscore
        {
            get
            {
                yield return new TestCaseData("__text__", "<strong>text</strong>")
                    .SetName("Double underscore creates bold tag");
                yield return new TestCaseData("__insi__de",
                        "<strong>insi</strong>de")
                    .SetName("Double underscore creates tag inside word at the beginning");
                yield return new TestCaseData("in__side__",
                        "in<strong>side</strong>")
                    .SetName("Double underscore creates tag inside word at the end");
                yield return new TestCaseData("in__si__de",
                        "in<strong>si</strong>de")
                    .SetName("Double underscore creates tag inside word");
                yield return new TestCaseData("te__xt in__side",
                        "te__xt in__side")
                    .SetName("Double underscores don't create tags inside other words");
                yield return new TestCaseData("__test",
                        "__test")
                    .SetName("Double underscore doesn't create tag when not closed");
                yield return new TestCaseData("1__23__",
                        "1__23__")
                    .SetName("Double underscore doesn't create tag inside digits");
                yield return new TestCaseData("____", "____")
                    .SetName("Double underscore doesn't create tag when empty string");
                yield return new TestCaseData("test__ test__",
                        "test__ test__")
                    .SetName("Double underscore at end of word not creates tag");
                yield return new TestCaseData("__test __test",
                        "__test __test")
                    .SetName("Double underscore at begin of word not close tag");
            }
        }

        private static IEnumerable<TestCaseData> CasesForInteractBetweenUnderscores
        {
            get
            {
                yield return new TestCaseData("__text_", "__text_")
                    .SetName("Different underscores dont't create bold tag");
                yield return new TestCaseData("a __aaa _aaa_ aaa__ a",
                        "a <strong>aaa <em>aaa</em> aaa</strong> a")
                    .SetName("Double underscore inside unary creates both tags");
                yield return new TestCaseData("_aaa __aaa__ aaa_",
                        "_aaa __aaa__ aaa_")
                    .SetName("Unary underscore inside double doesn't create any tags");
                yield return new TestCaseData("__a _a\na_ a__",
                        "__a _a\na_ a__")
                    .SetName("Underscores don't create any tag in different paragraphs");
                yield return new TestCaseData("__a _a__ a_",
                        "__a _a__ a_")
                    .SetName("Intersected underscores don't create any tag");
            }
        }
    }
}