using Markdown.Token;

namespace MarkdownTests;

public class MarkupConverterTestCases
{
    public static IEnumerable<TestCaseData> RenderTestCases
    {
        get
        {
            yield return new TestCaseData(new List<IToken>(), string.Empty, string.Empty)
                .SetName("ReturnEmptyString_WhenInputIsEmpty");
            yield return new TestCaseData(new List<IToken>(), "a", "a")
                .SetName("ReturnSameString_WhenInputContainsNoTags");

            yield return new TestCaseData(new List<IToken> { new HeaderToken(0), new HeaderToken(3, true) }, "# a",
                    "<h1>a</h1>\n")
                .SetName("ReturnStringWithHeader_WhenHeaderTagProvided");
            yield return new TestCaseData(new List<IToken> { new BoldToken(0), new BoldToken(3, true) }, "__a__",
                    "<strong>a</strong>")
                .SetName("ReturnStringWithStrongTag_WhenStrongTagProvided");
            yield return new TestCaseData(new List<IToken> { new ItalicToken(0), new ItalicToken(2, true) }, "_a_",
                    "<em>a</em>")
                .SetName("ReturnStringWithEmphasisTag_WhenEmphasisTagProvided");
            yield return new TestCaseData(new List<IToken> { new EscapeToken(0) }, "\\_a_", "_a_")
                .SetName("ReturnCorrectString_WhenTagWasScreened");
            yield return new TestCaseData(new List<IToken> { new NewLineToken(3) }, "aba\ncaba", "aba\ncaba")
                .SetName("ReturnCorrectString_WhenTextHasNewLine");
            yield return new TestCaseData(new List<IToken> { new NewLineToken(3, "\r\n") }, "aba\r\ncaba",
                    "aba\r\ncaba")
                .SetName("ReturnCorrectString_WhenTextHasWindowsNewLine");
            {
                var imageToken = new ImageToken(0);
                imageToken.Parameters = new List<string> { "aba", "caba" };
                imageToken.TokenSymbolsShift = 11;

                yield return new TestCaseData(new List<IToken> { imageToken }, "![aba](caba)",
                        "<img src=\"aba\" alt=\"caba\">")
                    .SetName("ReturnCorrectString_WhenImageTag");
            }

            {
                var imageToken = new ImageToken(34);
                imageToken.Parameters = new List<string> { "aba", "caba" };
                imageToken.TokenSymbolsShift = 11;

                yield return new TestCaseData(new List<IToken>
                        {
                            new HeaderToken(0), new BoldToken(6), new ItalicToken(8), new ItalicToken(13, true),
                            new BoldToken(23, true), new EscapeToken(29), new HeaderToken(32, true, 2), imageToken
                        }, "# Text___with_different__tags\\__\r\n![aba](caba)",
                        "<h1>Text<strong><em>with</em>different</strong>tags__</h1>\n<img src=\"aba\" alt=\"caba\">")
                    .SetName("ReturnCorrectString_WhenStringContainsMultipleTags");
            }
        }
    }
}