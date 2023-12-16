using System.Collections;
using Markdown.Tokens;
using Markdown.Tokens.Types;

namespace MarkdownTests.Lexer.TestCases;

public class LexerTestCases
{
    public static IEnumerable InvalidParametersTests
    {
        get
        {
            yield return new LexerRegisterTokenTestData(null!, LexerRegisterTokenTestData.ValidType);
            yield return new LexerRegisterTokenTestData("", LexerRegisterTokenTestData.ValidType);
            yield return new LexerRegisterTokenTestData("_", LexerRegisterTokenTestData.InvalidType);
            yield return new LexerRegisterTokenTestData("_", null!);
        }
    }

    public static IEnumerable NoValidationTests
    {
        get
        {
            yield return new LexerLogicTestData(
                "# __text strong__ ordinary_italic_ sometext",
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
                });

            yield return new LexerLogicTestData(
                "asd# fgf",
                new List<Token>
                {
                    new(new TextToken("asd# fgf"), false, 0, 8)
                });
        }
    }

    public static IEnumerable EscapeSymbolsTests
    {
        get
        {
            yield return new LexerLogicTestData(
                @"\\\__sk \_asd_ \df",
                new List<Token>
                {
                    new(new TextToken(@"\\\__sk \_asd_ \df"), false, 0, 18)
                });

            yield return new LexerLogicTestData(
                @"\\\\_",
                new List<Token>
                {
                    new(new TextToken(@"\\\\_"), false, 0, 5)
                });

            yield return new LexerLogicTestData(
                @"\\_a_",
                new List<Token>
                {
                    new(new TextToken(@"\\"), false, 0, 2),
                    new(new EmphasisToken(), false, 2, 1),
                    new(new TextToken("a"), false, 3, 1),
                    new(new EmphasisToken(), true, 4, 1),
                });
        }
    }

    public static IEnumerable TextTokenTests
    {
        get
        {
            yield return new LexerLogicTestData(
                "_a__ __b__ te_xt_",
                new List<Token>
                {
                    new(new TextToken("_a__ __b__ te_xt_"), false, 0, 17)
                });

            yield return new LexerLogicTestData(
                "a_a a_ _a_",
                new List<Token>
                {
                    new(new TextToken("a_a a_ "), false, 0, 7),
                    new(new EmphasisToken(), false, 7, 1),
                    new(new TextToken("a"), false, 8, 1),
                    new(new EmphasisToken(), true, 9, 1),
                });
        }
    }
}