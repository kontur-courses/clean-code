using System.Collections;

namespace MarkdownTests.Lexer.TestCases;

public class LexerBuilderTestCases
{
    public static IEnumerable InvalidParametersTests
    {
        get
        {
            yield return new TestCaseData(LexerBuilderTestData.InvalidTypeWithNullValue)
                .SetName("Type's value cannot be null.");
            yield return new TestCaseData(LexerBuilderTestData.InvalidTypeWithNullRepresentation)
                .SetName("Type's representation cannot be null.");
            yield return new TestCaseData(null!)
                .SetName("Token type cannot be null");
        }
    }
}