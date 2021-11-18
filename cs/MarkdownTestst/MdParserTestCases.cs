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
                yield return new TestCaseData(string.Empty, new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenStringIsEmpty");
                yield return new TestCaseData("a", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenThereAreNoMarkdowns");
                yield return new TestCaseData("____", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenBoldTokenContentIsEmpty");
                yield return new TestCaseData("__", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenItalicTokenContentIsEmpty");

                yield return new TestCaseData("_a__a_a__", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenThereIsStyleTokenIntersectionWhenItalicFirst");
                yield return new TestCaseData("__a_a__a_", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenThereIsStyleTokenIntersectionWhenBoldFirst");
                yield return new TestCaseData("__a _a", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenThereAreUnpairedTokens");
                yield return new TestCaseData("__a_a\na_a__a", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenThereAreUnpairedTokensInParagraph");

                yield return new TestCaseData("__ a__", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenThereIsIncorrectOpenBoldToken");
                yield return new TestCaseData("__a __", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenThereIsIncorrectCloseBoldToken");
                yield return new TestCaseData("a__a a__a", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenThereIsBoldTokenInDifferentWords");
                yield return new TestCaseData("1__1__1", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenThereIsBoldTokenInTextWithDigits");

                yield return new TestCaseData("_ a_", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenThereIsIncorrectOpenItalicToken");
                yield return new TestCaseData("_a _", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenThereIsIncorrectCloseItalicToken");
                yield return new TestCaseData("a_a a_a", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenThereIsItalicTokenInDifferentWords");
                yield return new TestCaseData("1_1_1", new List<Token>())
                    .SetName("ReturnEmptyIEnumerable_WhenThereIsItalicTokenInTextWithDigits");

                yield return new TestCaseData("#a", new List<Token>() { new HeaderToken(0, 2) })
                    .SetName("ReturnHeaderToken_WhenThereIsHeaderToken");
                yield return new TestCaseData("\\#a", new List<Token>() { new ScreeningToken(0, 0) })
                    .SetName("ReturnScreeningToken_WhenThereIsScreeningTag");
                yield return new TestCaseData("#a\n#a", new List<Token>() { new HeaderToken(0, 2), new HeaderToken(3, 5) })
                    .SetName("ReturnHeaderToken_WhenThereAreManyHeaderTokensAndManyString");

                yield return new TestCaseData("_a_", new List<Token>() { new ItalicToken(0, 2) })
                    .SetName("ReturnItalicToken_WhenThereIsItalicToken");
                yield return new TestCaseData("__a__", new List<Token>() { new BoldToken(0, 3) })
                    .SetName("ReturnBoldToken_WhenItsThereIsBoldToken");
                yield return new TestCaseData("_a__a__a_", new List<Token>() { new ItalicToken(0, 8) })
                    .SetName("ReturnItalicToken_WhenBoldTokenInsideItalicToken");
                yield return new TestCaseData("__a_a_a__", new List<Token>() { new BoldToken(0, 7), new ItalicToken(3, 5) })
                    .SetName("ReturnBoldTokenAndItalicToken_WhenItalicTokenInsideBoldToken");

                yield return new TestCaseData("\\__a\\__", new List<Token>() { new ScreeningToken(0, 0) , new ScreeningToken(4, 4) })
                    .SetName("ReturnScreeningTokens_WhenThereIsScreenedBoldToken");
                yield return new TestCaseData("\\_a\\_", new List<Token>() { new ScreeningToken(0, 0), new ScreeningToken(3, 3) })
                    .SetName("ReturnScreeningTokens_WhenThereIsScreenedItalicToken");
                yield return new TestCaseData("\\\\a", new List<Token>() { new ScreeningToken(0, 0)})
                    .SetName("ReturnScreeningToken_WhenThereIsScreenedScreeningToken");

                yield return new TestCaseData("# __a__ _a_ __a_a_a__ \\_a\\_ _a__a__a_ __a_a__a_ _a__a_a__ \n", new List<Token>()
                    {
                        new HeaderToken(0, 58),
                        new BoldToken(2, 5),
                        new ItalicToken(8, 10),
                        new BoldToken(12, 19),
                        new ItalicToken(15, 17),
                        new ScreeningToken(22, 22),
                        new ScreeningToken(25, 25),
                        new ItalicToken(28, 36)
                    })
                    .SetName("ReturnAllTokens_WhenThereIsManyInteractingTokens");
            }
        }
    }
}