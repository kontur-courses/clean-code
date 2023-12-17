using Markdown.Tokens.Types;
using NSubstitute;

namespace MarkdownTests.Lexer.TestCases;

public class LexerRegisterTokenTestData
{
    public ITokenType TokenType { get; }

    public static readonly ITokenType ValidType;
    public static readonly ITokenType InvalidTypeWithNullRepresentation;
    public static readonly ITokenType InvalidTypeWithNullValue;
    public static readonly ITokenType InvalidTypeWithEmptyValue;

    static LexerRegisterTokenTestData()
    {
        ValidType = Substitute.For<ITokenType>();
        ValidType.Value.Returns("_");
        ValidType.Representation(Arg.Any<bool>()).Returns("<em>");

        InvalidTypeWithNullValue = Substitute.For<ITokenType>();
        InvalidTypeWithNullValue.Value.Returns((string) null!);

        InvalidTypeWithEmptyValue = Substitute.For<ITokenType>();
        InvalidTypeWithEmptyValue.Value.Returns("");

        InvalidTypeWithNullRepresentation = Substitute.For<ITokenType>();
        InvalidTypeWithNullRepresentation.Value.Returns("_");
        InvalidTypeWithNullRepresentation.Representation(Arg.Any<bool>()).Returns((string) null!);
    }

    public LexerRegisterTokenTestData(ITokenType tokenType)
    {
        TokenType = tokenType;
    }
}