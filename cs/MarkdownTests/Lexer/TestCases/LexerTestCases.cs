using System.Collections;

namespace MarkdownTests.Lexer.TestCases;

public class LexerTestCases
{
    public static IEnumerable InvalidParametersTestCases
    {
        get
        {
            yield return new LexerTestData(null!, LexerTestData.ValidType);
            yield return new LexerTestData("", LexerTestData.ValidType);
            yield return new LexerTestData("_", LexerTestData.InvalidType);
            yield return new LexerTestData("_", null!);
        }
    }
}