using Markdown.Tokens.Types;
using NSubstitute;

namespace MarkdownTests.Lexer;

public class RegisterTokenTypeTestData
{
    public string TypeSymbol { get; }
    public ITokenType TokenType { get; }

    public static readonly ITokenType ValidType;
    public static readonly ITokenType InvalidType;

    static RegisterTokenTypeTestData()
    {
        ValidType = Substitute.For<ITokenType>();
        ValidType.Representation(Arg.Any<bool>()).Returns("<em>");

        InvalidType = Substitute.For<ITokenType>();
        InvalidType.Representation(Arg.Any<bool>()).Returns((string) null!);
    }
        
    public RegisterTokenTypeTestData(string typeSymbol, ITokenType tokenType)
    {
        TypeSymbol = typeSymbol;
        TokenType = tokenType;
    }
}