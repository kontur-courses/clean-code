using Markdown.Lexer;
using Markdown.Tokens;

namespace Markdown.TokenConverter;

public interface ITokenConverter
{
    string ConvertToString(TokenizeResult tokenizeResult);
}