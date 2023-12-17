using Markdown.Lexer;

namespace Markdown.TokenConverter;

public interface ITokenConverter
{
    string ConvertToString(TokenizeResult tokenizeResult);
}