using Markdown.Tokens.Types;
using NSubstitute;

namespace MarkdownTests.Lexer.TestCases;

public class LexerTestData
{
    public string TypeSymbol { get; }
    public ITokenType TokenType { get; }

    public static readonly ITokenType ValidType;
    public static readonly ITokenType InvalidType;

    static LexerTestData()
    {
        ValidType = Substitute.For<ITokenType>();
        ValidType.Representation(Arg.Any<bool>()).Returns("<em>");

        InvalidType = Substitute.For<ITokenType>();
        InvalidType.Representation(Arg.Any<bool>()).Returns((string) null!);
    }
        
    public LexerTestData(string typeSymbol, ITokenType tokenType)
    {
        TypeSymbol = typeSymbol;
        TokenType = tokenType;
    }
}