using System.Collections;

namespace MarkdownTests.Lexer.TestCases;

public class LexerBuilderTestCases
{
    public static IEnumerable InvalidParametersTests
    {
        get
        {
            yield return new LexerBuilderTestData(LexerBuilderTestData.InvalidTypeWithNullValue);
            yield return new LexerBuilderTestData(LexerBuilderTestData.InvalidTypeWithEmptyValue);
            yield return new LexerBuilderTestData(LexerBuilderTestData.InvalidTypeWithNullRepresentation);
            yield return new LexerBuilderTestData(null!);
        }
    }
}