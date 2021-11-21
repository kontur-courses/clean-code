using System.Collections.Generic;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    internal class HtmlRendererTestCases
    {
        public static IEnumerable<TestCaseData> RenderTestCases
        {
            get
            {
                yield return new TestCaseData(new List<Token>(), string.Empty, string.Empty)
                    .SetName("ReturnEmptyString_WhenInputIsEmpty");
                yield return new TestCaseData(new List<Token>(), "a", "a")
                    .SetName("ReturnCorrectString_WhenThereAreNoMarkdownsInput");
                yield return new TestCaseData(new List<Token> { new HeaderToken(0, 3) }, "# a", "<h1>a</h1>")
                    .SetName("ReturnCorrectString_WhenThereIsHeaderTagWithEOF");
                yield return new TestCaseData(new List<Token> { new HeaderToken(0, 3) }, "# a\n", "<h1>a</h1>\n")
                    .SetName("ReturnCorrectString_WhenThereIsHeaderTagWithLf");
                yield return new TestCaseData(new List<Token> { new BoldToken(0, 3) }, "__a__", "<strong>a</strong>")
                    .SetName("ReturnCorrectString_WhenThereIsBoldTag");
                yield return new TestCaseData(new List<Token> { new ItalicToken(0, 2) }, "_a_", "<em>a</em>")
                    .SetName("ReturnCorrectString_WhenThereIsItalicTag");
                yield return new TestCaseData(new List<Token> { new ScreeningToken(0, 0) }, "\\_a_", "_a_")
                    .SetName("ReturnCorrectString_WhenThereIsScreeningTag");
                yield return new TestCaseData(
                        new List<Token>
                        {
                            new HeaderToken(0, 58),
                            new BoldToken(2, 5),
                            new ItalicToken(8, 10),
                            new BoldToken(12, 19),
                            new ItalicToken(15, 17),
                            new ScreeningToken(22, 22),
                            new ScreeningToken(25, 25),
                            new ItalicToken(28, 36)
                        },
                        "# __a__ _a_ __a_a_a__ \\_a\\_ _a__a__a_ __a_a__a_ _a__a_a__ \n",
                        "<h1><strong>a</strong> <em>a</em> <strong>a<em>a</em>a</strong> _a_ <em>a__a__a</em> __a_a__a_ _a__a_a__ </h1>\n")
                    .SetName("ReturnCorrectString_WhenThereAreManyTags");
            }
        }
    }
}