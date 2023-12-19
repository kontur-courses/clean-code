using System.Collections;
using Markdown.Tokens;
using Markdown.Tokens.Types;

namespace MarkdownTests.Lexer.TestCases;

public class LexerTestCases
{
    public static IEnumerable NoValidationTests
    {
        get
        {
            yield return new TestCaseData(
                    "# __text strong__ ordinary_italic_ sometext")
                .Returns(
                    new List<Token>
                    {
                        new(new HeaderToken(), false, 0, 2),
                        new(new StrongToken(), false, 2, 2),
                        new(new TextToken("text strong"), false, 4, 11),
                        new(new StrongToken(), true, 15, 2),
                        new(new TextToken(" ordinary"), false, 17, 9),
                        new(new EmphasisToken(), false, 26, 1),
                        new(new TextToken("italic"), false, 27, 6),
                        new(new EmphasisToken(), true, 33, 1),
                        new(new TextToken(" sometext"), false, 34, 9)
                    })
                .SetName("Correctly renders header, emphasis and strong tokens.");

            yield return new TestCaseData(
                    "asd# fgf")
                .Returns(
                    new List<Token>
                    {
                        new(new TextToken("asd# fgf"), false, 0, 8)
                    })
                .SetName("Doesnt render header tag if it's not in the beginning of the line.");
        }
    }

    public static IEnumerable EscapeSymbolsTests
    {
        get
        {
            yield return new TestCaseData(
                    @"\\\__sk \_asd_ \df")
                .Returns(
                    new List<Token>
                    {
                        new(new TextToken(@"\\\__sk \_asd_ \df"), false, 0, 18)
                    })
                .SetName("Multiple escape symbols in a row, escaping a tag, escaping nothing");

            yield return new TestCaseData(
                    @"\\_a_")
                .Returns(
                    new List<Token>
                    {
                        new(new TextToken(@"\\"), false, 0, 2),
                        new(new EmphasisToken(), false, 2, 1),
                        new(new TextToken("a"), false, 3, 1),
                        new(new EmphasisToken(), true, 4, 1),
                    })
                .SetName("Does not escape token in escape symbol is escaped itself.");
        }
    }

    public static IEnumerable TextTokenTests
    {
        get
        {
            yield return new TestCaseData(
                    "_a__ __b__ te_xt_")
                .Returns(
                    new List<Token>
                    {
                        new(new TextToken("_a__ __b__ te_xt_"), false, 0, 17)
                    })
                .SetName("Transforms registered tokens that failed validation into a text token.");

            yield return new TestCaseData(
                    "a_a a_ _a_")
                .Returns(
                    new List<Token>
                    {
                        new(new TextToken("a_a a_ "), false, 0, 7),
                        new(new EmphasisToken(), false, 7, 1),
                        new(new TextToken("a"), false, 8, 1),
                        new(new EmphasisToken(), true, 9, 1),
                    })
                .SetName("Does not transform tokens that passed validation into a text token.");
        }
    }
}