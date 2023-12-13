using System.Collections;

namespace MarkdownTests.Lexer;

public class RegisterTokenTypeTestCases
{
    public static IEnumerable InvalidParametersTestCases
    {
        get
        {
            yield return new RegisterTokenTypeTestData(null!, RegisterTokenTypeTestData.ValidType);
            yield return new RegisterTokenTypeTestData("", RegisterTokenTypeTestData.ValidType);
            yield return new RegisterTokenTypeTestData("_", RegisterTokenTypeTestData.InvalidType);
            yield return new RegisterTokenTypeTestData("_", null!);
        }
    }
}