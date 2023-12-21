using Markdown.Lexer;

namespace Markdown.TokenConverter;

public interface ITokenConverter
{
    TokenConversionResult ConvertToString(TokenizeResult tokenizeResult);
}