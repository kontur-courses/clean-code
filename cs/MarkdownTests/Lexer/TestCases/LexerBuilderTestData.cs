using Markdown.Tokens.Types;
using NSubstitute;

namespace MarkdownTests.Lexer.TestCases;

public static class LexerBuilderTestData
{
    public static readonly ITokenType InvalidTypeWithNullRepresentation;
    public static readonly ITokenType InvalidTypeWithNullValue;

    static LexerBuilderTestData()
    {
        InvalidTypeWithNullValue = Substitute.For<ITokenType>();
        InvalidTypeWithNullValue.Value.Returns((string) null!);

        InvalidTypeWithNullRepresentation = Substitute.For<ITokenType>();
        InvalidTypeWithNullRepresentation.Value.Returns("_");
        InvalidTypeWithNullRepresentation.Representation(Arg.Any<bool>()).Returns((string) null!);
    }
}