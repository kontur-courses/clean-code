using System.Collections.Generic;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    internal class MdParserTestCases
    {
        public static IEnumerable<TestCaseData> ParseTokenTestCases
        {
            get
            {
                yield return new TestCaseData("a", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenThereAreNoMarkdowns");
                yield return new TestCaseData("____", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenBoldTokenContentIsEmpty");
                yield return new TestCaseData("__", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenItalicTokenContentIsEmpty");
                yield return new TestCaseData("\a", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenThereIsNothingToScreen");
                yield return new TestCaseData("__a _a", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenThereAreUnpairedTokens");
                yield return new TestCaseData("#a", new List<Token>() { new HeaderToken(0, 2) })
                    .SetName("ReturnHeaderToken_WhenItsOnlyOne");
                yield return new TestCaseData("\\#a", new List<Token>() { new ScreeningToken(0, 0) })
                    .SetName("ReturnScreeningToken_WhenThereIsScreeningTag");
                yield return new TestCaseData("_a_", new List<Token>() { new ItalicToken(0, 2) })
                    .SetName("ReturnItalicToken_WhenItsOnlyOne");
                yield return new TestCaseData("__a__", new List<Token>() { new BoldToken(0, 3) })
                    .SetName("ReturnBoldToken_WhenItsOnlyOne");
                yield return new TestCaseData("\\__a\\__", new List<Token>() { new ScreeningToken(0, 0) , new ScreeningToken(4, 4) })
                    .SetName("ReturnScreeningTokens_WhenItsScreenedBoldToken");
                yield return new TestCaseData("\\_a\\_", new List<Token>() { new ScreeningToken(0, 0), new ScreeningToken(3, 3) })
                    .SetName("ReturnScreeningTokens_WhenItsScreenedItalicToken");
                yield return new TestCaseData("\\\\a", new List<Token>() { new ScreeningToken(0, 0)})
                    .SetName("ReturnScreeningToken_WhenItsScreenedScreeningToken");
            }
        }
    }
}